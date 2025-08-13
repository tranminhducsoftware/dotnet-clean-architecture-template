// Copyright (c) 2025 tranminhducsoftware. Author: Tran Minh Duc. Licensed under MIT.

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using CleanArchExample.Application.Interfaces;

using Microsoft.IdentityModel.Tokens;
namespace CleanArchExample.Infrastructure.Identity;

public sealed class JwtProvider : IJwtProvider
{
    private readonly JwtOptions _opt;
    private readonly SigningCredentials _cred;

    public JwtProvider(JwtOptions opt)
    {
        _opt = opt;
        _cred = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(opt.SigningKey)), SecurityAlgorithms.HmacSha256);
    }

    public string CreateAccessToken(Guid userId, string username, IEnumerable<string> roles, DateTime utcNow)
    {
        var claims = new List<Claim>
    {
        new(JwtRegisteredClaimNames.Sub, userId.ToString()),
        new(JwtRegisteredClaimNames.UniqueName, username),
        new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N"))
    };
        claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

        var token = new JwtSecurityToken(
            issuer: _opt.Issuer,
            audience: _opt.Audience,
            claims: claims,
            notBefore: utcNow,
            expires: utcNow.AddMinutes(10), // thời lượng thực lấy từ AuthOptions tại AuthService
            signingCredentials: _cred
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}