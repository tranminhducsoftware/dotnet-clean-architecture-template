// Copyright (c) 2025 tranminhducsoftware. Author: Tran Minh Duc. Licensed under MIT.

namespace CleanArchExample.Application.Interfaces;

public interface IJwtProvider
{
    string CreateAccessToken(Guid userId, string username, IEnumerable<string> roles, DateTime utcNow);

}