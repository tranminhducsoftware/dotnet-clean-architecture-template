// Copyright (c) 2025 tranminhducsoftware. Author: Tran Minh Duc. Licensed under MIT.


using CleanArchExample.Domain.Interfaces;

namespace CleanArchExample.Domain.Entities;


public sealed class User : IAuditableEntity
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Username { get; private set; } = default!;
    public string PasswordHash { get; private set; } = default!;
    public string? Email { get; private set; }
    public bool IsActive { get; private set; } = true;

    // Audit fields
    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }

    // EF Core yêu cầu constructor không tham số
    private User() { }

    public User(string username, string passwordHash, string? email = null)
    {
        SetUsername(username);
        SetPassword(passwordHash);
        Email = email;
    }

    // Domain methods
    public void SetUsername(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("Username is required.", nameof(username));

        Username = username.Trim();
        ModifiedAt = DateTime.UtcNow;
    }

    public void SetPassword(string hash)
    {
        if (string.IsNullOrWhiteSpace(hash))
            throw new ArgumentException("Password hash is required.", nameof(hash));

        PasswordHash = hash;
        ModifiedAt = DateTime.UtcNow;
    }

    public void ChangeEmail(string? email)
    {
        if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("Email required");
        Email = email.Trim();
        ModifiedAt = DateTime.UtcNow;
    }

    public void ChangePassword(string newHash)
    {
        if (string.IsNullOrWhiteSpace(newHash)) throw new ArgumentException("Hash required");
        PasswordHash = newHash;
        ModifiedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        ModifiedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        IsActive = true;
        ModifiedAt = DateTime.UtcNow;
    }
}
