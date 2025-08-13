// Copyright (c) 2025 tranminhducsoftware. Author: Tran Minh Duc. Licensed under MIT.

using CleanArchExample.Application.Features.Auth.Commands;
using CleanArchExample.Application.Interfaces;

using MediatR;

namespace CleanArchExample.Application.Features.Auth.Handler;

public sealed class LogoutCommandHandler : IRequestHandler<LogoutCommand>
{
    private readonly IRefreshTokenStore _store;

    public LogoutCommandHandler(IRefreshTokenStore store) => _store = store;

    public async Task Handle(LogoutCommand req, CancellationToken ct)
    {
        await _store.RevokeSessionAsync(req.SessionId, "logout", ct);
    }
}