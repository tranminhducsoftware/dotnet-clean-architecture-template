// Copyright (c) 2025 tranminhducsoftware. Author: Tran Minh Duc. Licensed under MIT.

using CleanArchExample.Domain.Entities;

namespace CleanArchExample.Application.Interfaces;

public interface IUserRepository
{
    Task<User?> FindByUsernameAsync(string username, CancellationToken ct);
    Task<User?> GetByIdAsync(Guid id, CancellationToken ct);
}