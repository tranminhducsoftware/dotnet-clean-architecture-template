// Copyright (c) 2025 tranminhducsoftware. Author: Tran Minh Duc. Licensed under MIT.

using CleanArchExample.Application.Interfaces;

using MediatR;

namespace CleanArchExample.Application.Features.Auth.Commands;

public sealed class LogoutAllCommandHandler : IRequestHandler<LogoutAllCommand>
{
    private readonly IRefreshTokenStore _store;

    public LogoutAllCommandHandler(IRefreshTokenStore store) => _store = store;

    public async Task Handle(LogoutAllCommand req, CancellationToken ct)
    {
        await _store.RevokeAllForUserAsync(req.UserId, req.Reason ?? "logout-all", ct);
    }
}