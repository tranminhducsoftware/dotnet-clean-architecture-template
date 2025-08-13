// Copyright (c) 2025 tranminhducsoftware. Author: Tran Minh Duc. Licensed under MIT.

using CleanArchExample.Application.Interfaces;

using StackExchange.Redis;

namespace CleanArchExample.Infrastructure.Services;

public sealed class RedisRefreshTokenStore : IRefreshTokenStore, IRefreshTokenStoreEx
{
    private readonly IDatabase _db;
    private readonly TimeSpan _skew = TimeSpan.FromSeconds(30);

    public RedisRefreshTokenStore(IConnectionMultiplexer mux) => _db = mux.GetDatabase();

    private static string SKey(string sid) => $"session:{sid}";
    private static string UKey(Guid uid) => $"user:{uid}:sessions";
    private static string FActive(string fid) => $"family:{fid}:active";

    public async Task<(string refreshToken, string sessionId)> IssueAsync(
        Guid userId, string familyId, TimeSpan ttl,
        string? deviceId, string? userAgent, string? ip,
        CancellationToken ct)
    {
        var sid = Guid.NewGuid().ToString("N");
        var rt = TokenHasher.NewOpaque();
        var rtHash = TokenHasher.Hash(rt);
        var exp = DateTimeOffset.UtcNow.Add(ttl).ToUnixTimeSeconds();

        var t = _db.CreateTransaction();
        _ = t.HashSetAsync(SKey(sid), new HashEntry[] {
        new("userId", userId.ToString()),
        new("rtHash", rtHash),
        new("exp", exp),
        new("deviceId", deviceId ?? ""),
        new("userAgent", userAgent ?? ""),
        new("ip", ip ?? ""),
        new("familyId", familyId),
        new("replacedBy", ""),
        new("revokedAt", ""),
        new("reason", "")
    });
        _ = t.KeyExpireAsync(SKey(sid), ttl + _skew);
        _ = t.SetAddAsync(UKey(userId), sid);
        _ = t.StringSetAsync(FActive(familyId), sid, ttl + _skew);
        await t.ExecuteAsync();

        return (rt, sid);
    }

    // Lua cho rotation + reuse-detection (atomic):
    // KEYS[1]=sessionKey, KEYS[2]=familyActiveKey, KEYS[3]=newSessionKey, KEYS[4]=userSessionsSet
    // ARGV: [1]=presentedRtHash, [2]=nowUnix, [3]=newSid, [4]=newRtHash, [5]=newExpUnix, [6]=ttlSeconds
    private const string RotateLua = @"
local sess = redis.call('HGETALL', KEYS[1])
if (#sess == 0) then return {0} end
local map = {}
for i=1,#sess,2 do map[sess[i]] = sess[i+1] end

if (map['revokedAt'] ~= '' and map['revokedAt'] ~= false) then return {0} end
if (tonumber(map['exp']) <= tonumber(ARGV[2])) then return {0} end
if (map['rtHash'] ~= ARGV[1]) then return {0} end

local activeSid = redis.call('GET', KEYS[2])
if (not activeSid or activeSid ~= string.match(KEYS[1], 'session:(.+)')) then
  -- reuse detected: you may choose to revoke whole family here
  return {-1} -- signal reuse
end

-- mark old
redis.call('HSET', KEYS[1], 'replacedBy', ARGV[3], 'revokedAt', ARGV[2], 'reason', 'rotated')

-- create new
redis.call('HMSET', KEYS[3],
  'userId', map['userId'],
  'rtHash', ARGV[4],
  'exp', ARGV[5],
  'deviceId', map['deviceId'],
  'userAgent', map['userAgent'],
  'ip', map['ip'],
  'familyId', map['familyId'],
  'replacedBy', '',
  'revokedAt', '',
  'reason', '')

redis.call('EXPIRE', KEYS[3], tonumber(ARGV[6]))
redis.call('SET', KEYS[2], ARGV[3], 'EX', tonumber(ARGV[6]))
redis.call('SADD', KEYS[4], ARGV[3])

return {1}";

    public async Task<(bool ok, string? newRefreshToken, string? newSessionId)> RotateAsync(
        string presentedRefreshToken, string sessionId, string familyId, TimeSpan ttl, CancellationToken ct)
    {
        var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        var newSid = Guid.NewGuid().ToString("N");
        var newRt = TokenHasher.NewOpaque();
        var newHash = TokenHasher.Hash(newRt);
        var newExp = DateTimeOffset.UtcNow.Add(ttl).ToUnixTimeSeconds();

        var res = (int[]?)await _db.ScriptEvaluateAsync(RotateLua,
            keys: new RedisKey[] { SKey(sessionId), FActive(familyId), SKey(newSid), UKey(await GetUserIdBySessionRequired(sessionId)) },
            values: new RedisValue[] { TokenHasher.Hash(presentedRefreshToken), now, newSid, newHash, newExp, (long)(ttl + _skew).TotalSeconds });

        if (res == null || res.Length == 0) return (false, null, null);
        if (res[0] == -1)
        {
            // REUSE DETECTED: revoke family (đơn giản: revoke user all)
            var uid = await GetUserIdBySessionRequired(sessionId);
            await RevokeAllForUserAsync(uid, "reuse-detected", ct);
            return (false, null, null);
        }
        if (res[0] != 1) return (false, null, null);

        return (true, newRt, newSid);
    }

    public async Task RevokeSessionAsync(string sessionId, string reason, CancellationToken ct)
    {
        var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        await _db.HashSetAsync(SKey(sessionId), new HashEntry[] {
        new("revokedAt", now), new("reason", reason)
    });
        await _db.KeyExpireAsync(SKey(sessionId), TimeSpan.FromMinutes(5));
    }

    public async Task RevokeAllForUserAsync(Guid userId, string reason, CancellationToken ct)
    {
        var setKey = UKey(userId);
        var sids = await _db.SetMembersAsync(setKey);
        var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        var t = _db.CreateTransaction();
        foreach (var sid in sids)
        {
            _ = t.HashSetAsync(SKey(sid!), new HashEntry[] { new("revokedAt", now), new("reason", reason) });
            _ = t.KeyExpireAsync(SKey(sid!), TimeSpan.FromMinutes(5));
        }
        _ = t.KeyDeleteAsync(setKey);
        await t.ExecuteAsync();
    }

    public async Task<Guid?> GetUserIdBySessionAsync(string sessionId, CancellationToken ct)
    {
        var v = await _db.HashGetAsync(SKey(sessionId), "userId");
        if (v.IsNullOrEmpty) return null;
        return Guid.TryParse(v!, out var g) ? g : null;
    }

    private async Task<Guid> GetUserIdBySessionRequired(string sessionId)
        => await GetUserIdBySessionAsync(sessionId, default) ?? throw new UnauthorizedAccessException("Session not found");
}