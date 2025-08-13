// Copyright (c) 2025 tranminhducsoftware. Author: Tran Minh Duc. Licensed under MIT.

namespace CleanArchExample.Application.Interfaces;

public interface IRefreshTokenStore
{
    // Issue a new session + refresh token
    Task<(string refreshToken, string sessionId)> IssueAsync(
        Guid userId, string familyId, TimeSpan ttl,
        string? deviceId, string? userAgent, string? ip,
        CancellationToken ct);

    // Rotate (atomic). Returns: ok, newRefreshToken, newSessionId
    Task<(bool ok, string? newRefreshToken, string? newSessionId)> RotateAsync(
        string presentedRefreshToken, string sessionId, string familyId, TimeSpan ttl, CancellationToken ct);

    Task RevokeSessionAsync(string sessionId, string reason, CancellationToken ct);
    Task RevokeAllForUserAsync(Guid userId, string reason, CancellationToken ct);
}