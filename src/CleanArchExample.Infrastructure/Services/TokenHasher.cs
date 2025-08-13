// Copyright (c) 2025 tranminhducsoftware. Author: Tran Minh Duc. Licensed under MIT.

using System.Security.Cryptography;
using System.Text;

namespace CleanArchExample.Infrastructure.Services;

public static class TokenHasher
{
    // HMAC nhanh, đủ an toàn vì RT là random 256-bit. Đặt secret trong ENV/KeyVault.
    private static readonly byte[] Secret = Encoding.UTF8.GetBytes(
        Environment.GetEnvironmentVariable("RT_HMAC_SECRET") ?? throw new InvalidOperationException("Missing RT_HMAC_SECRET"));

    public static string Hash(string token)
    {
        using var h = new HMACSHA256(Secret);
        return Convert.ToHexString(h.ComputeHash(Encoding.UTF8.GetBytes(token))); // 64 hex
    }

    public static string NewOpaque(int byteLen = 32) // 256-bit
    {
        var b = RandomNumberGenerator.GetBytes(byteLen);
        return Convert.ToBase64String(b).TrimEnd('=').Replace('+', '-').Replace('/', '_');
    }
}