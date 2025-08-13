// Copyright (c) 2025 tranminhducsoftware. Author: Tran Minh Duc. Licensed under MIT.

namespace CleanArchExample.Application.Interfaces;

public interface IRefreshTokenStoreEx : IRefreshTokenStore
{
    Task<Guid?> GetUserIdBySessionAsync(string sessionId, CancellationToken ct);
}