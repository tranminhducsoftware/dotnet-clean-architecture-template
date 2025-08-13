// Copyright (c) 2025 tranminhducsoftware. Author: Tran Minh Duc. Licensed under MIT.

using CleanArchExample.Application.Interfaces;
using CleanArchExample.Domain.Entities;
using CleanArchExample.Persistence.Contexts;

using Microsoft.EntityFrameworkCore;

namespace CleanArchExample.Persistence.Repositories;

public sealed class UserRepository : IUserRepository
{
    private readonly AppDbContext _db;
    public UserRepository(AppDbContext db) => _db = db;

    public Task<User?> FindByUsernameAsync(string username, CancellationToken ct)
        => _db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Username == username, ct);

    public Task<User?> GetByIdAsync(Guid id, CancellationToken ct)
        => _db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id, ct);
}