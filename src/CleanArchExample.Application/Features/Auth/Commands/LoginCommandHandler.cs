// Copyright (c) 2025 tranminhducsoftware. Author: Tran Minh Duc. Licensed under MIT.

using CleanArchExample.Application.Features.Auth;
using CleanArchExample.Application.Features.Auth.Commands;
using CleanArchExample.Application.Features.Auth.Dtos;
using CleanArchExample.Application.Interfaces;

using MediatR;

namespace CleanArchExample.Application.Features.Users.Handler;

public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResultDto>
{
    private readonly IUserRepository _users;
    private readonly IJwtProvider _jwt;
    private readonly IRefreshTokenStore _store;
    private readonly AuthOptions _opt;
    private readonly IPasswordHasher _hasher;

    public LoginCommandHandler(IUserRepository users, IJwtProvider jwt, IRefreshTokenStore store, AuthOptions opt, IPasswordHasher hasher)
    {
        _users = users;
        _jwt = jwt;
        _store = store;
        _opt = opt;
        _hasher = hasher;

    }

    public async Task<LoginResultDto> Handle(LoginCommand req, CancellationToken ct)
    {
        var user = await _users.FindByUsernameAsync(req.Username, ct)
                   ?? throw new UnauthorizedAccessException("Invalid credentials");
        if (!user.IsActive) throw new UnauthorizedAccessException("User disabled");

        if (!_hasher.Verify(req.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Invalid credentials");

        // Family per-login (hoặc theo thiết bị)
        var familyId = Guid.NewGuid().ToString("N");

        var (rt, sid) = await _store.IssueAsync(user.Id, familyId, TimeSpan.FromDays(_opt.RefreshTokenDays), req.DeviceId, req.UserAgent, req.Ip, ct);

        var at = _jwt.CreateAccessToken(user.Id, user.Username, Enumerable.Empty<string>(), DateTime.UtcNow);

        return new LoginResultDto(at, rt, sid, new List<string>() { });
    }
}