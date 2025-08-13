// Copyright (c) 2025 tranminhducsoftware. Author: Tran Minh Duc. Licensed under MIT.

using CleanArchExample.Application.Interfaces;

namespace CleanArchExample.Infrastructure.Security;

public sealed class BCryptPasswordHasher : IPasswordHasher
{
    // WorkFactor 10–12 là hợp lý cho server hiện đại
    private const int WorkFactor = 11;

    public string Hash(string password)
        => BCrypt.Net.BCrypt.HashPassword(password, workFactor: WorkFactor);

    public bool Verify(string password, string hash)
        => BCrypt.Net.BCrypt.Verify(password, hash);
}