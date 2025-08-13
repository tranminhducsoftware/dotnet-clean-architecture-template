// Copyright (c) 2025 tranminhducsoftware. Author: Tran Minh Duc. Licensed under MIT.

using CleanArchExample.API.Contracts.Auth;
using CleanArchExample.Application.Features.Auth.Commands;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace CleanArchExample.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]

    [Route("api/v{version:apiVersion}/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>Login và phát hành Access/Refresh token</summary>
        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginRequest dto, CancellationToken ct)
        {
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
            var cmd = new LoginCommand(dto.Username, dto.Password, dto.DeviceId, dto.UserAgent, ip);
            var result = await _mediator.Send(cmd, ct);
            return Ok(new LoginResponse(result.AccessToken, result.RefreshToken, result.SessionId, result.Roles));
        }


        [HttpPost("refresh")]
        [ProducesResponseType(typeof(RefreshTokenResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest dto, CancellationToken ct)
        {
            var cmd = new RefreshTokenCommand(dto.RefreshToken, dto.SessionId, dto.FamilyId);
            var result = await _mediator.Send(cmd, ct);
            return Ok(new RefreshTokenResponse(result.AccessToken, result.RefreshToken, result.SessionId, result.Roles));
        }

        [HttpPost("logout")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Logout([FromBody] LogoutRequest dto, CancellationToken ct)
        {
            await _mediator.Send(new LogoutCommand(dto.SessionId), ct);
            return NoContent();
        }

        [HttpPost("logout-all")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> LogoutAll([FromBody] LogoutAllRequest dto, CancellationToken ct)
        {
            var userId = Guid.Parse(User.FindFirst("sub")!.Value);
            await _mediator.Send(new LogoutAllCommand(userId, dto.Reason), ct);
            return NoContent();
        }

    }
}
