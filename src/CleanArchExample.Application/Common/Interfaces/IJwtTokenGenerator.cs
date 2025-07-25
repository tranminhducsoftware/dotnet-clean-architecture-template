// Copyright (c) 2025 tranminhducsoftware. Author: Tran Minh Duc. Licensed under MIT.

namespace CleanArchExample.Application.Common.Interfaces
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(string username, string userId);

    }
}