// Copyright (c) 2025 tranminhducsoftware. Author: Tran Minh Duc. Licensed under MIT.

using CleanArchExample.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchExample.Persistence.Configurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> b)
    {
        b.ToTable("Users");

        b.HasKey(x => x.Id);

        b.Property(x => x.Username)
            .IsRequired()
            .HasMaxLength(128);

        b.HasIndex(x => x.Username)
            .IsUnique();

        b.Property(x => x.PasswordHash)
            .IsRequired()
            .HasMaxLength(256);

        b.Property(x => x.Email)
            .HasMaxLength(256);

        b.Property(x => x.IsActive)
            .HasDefaultValue(true);

        b.Property(x => x.CreatedAt)
            .IsRequired();

        b.Property(x => x.ModifiedAt);
    }
}