// Copyright (c) 2025 tranminhducsoftware. Author: Tran Minh Duc. Licensed under MIT.

using CleanArchExample.Application.Features.Auth.Commands;
using CleanArchExample.Application.Features.Auth.Dtos;
using CleanArchExample.Application.Interfaces;

using MediatR;

namespace CleanArchExample.Application.Features.Auth.Handler;

public sealed class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, RefreshTokenResultDto>
{
    private readonly IRefreshTokenStoreEx _store;
    private readonly IUserRepository _users;
    private readonly IJwtProvider _jwt;
    private readonly AuthOptions _opt;

    public RefreshTokenCommandHandler(IRefreshTokenStoreEx store, IUserRepository users, IJwtProvider jwt, AuthOptions opt)
    {
        _store = store;
        _users = users;
        _jwt = jwt;
        _opt = opt;
    }

    public async Task<RefreshTokenResultDto> Handle(RefreshTokenCommand req, CancellationToken ct)
    {
        var (ok, newRt, newSid) = await _store.RotateAsync(
            req.RefreshToken, req.SessionId, req.FamilyId,
            TimeSpan.FromDays(_opt.RefreshTokenDays), ct);

        if (!ok) throw new UnauthorizedAccessException("Invalid or reused refresh token");

        var uid = await _store.GetUserIdBySessionAsync(newSid!, ct)
                  ?? throw new UnauthorizedAccessException("Session not found");

        var user = await _users.GetByIdAsync(uid, ct)
                   ?? throw new UnauthorizedAccessException("User not found");

        var at = _jwt.CreateAccessToken(
            user.Id, user.Username,
            Enumerable.Empty<string>(), DateTime.UtcNow);

        return new RefreshTokenResultDto(at, newRt!, newSid!, new List<string> { });
    }
}