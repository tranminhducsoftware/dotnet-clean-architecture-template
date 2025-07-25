// Copyright (c) 2025 tranminhducsoftware. Author: Tran Minh Duc. Licensed under MIT.

using System.IdentityModel.Tokens.Jwt;

using CleanArchExample.Application.Features.Auth.Commands;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchExample.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AccountController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterCommand command)
        {
            try
            {
                var userId = await _mediator.Send(command);
                return CreatedAtAction(nameof(Register), new { Id = userId }, null);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpGet("profile")]
        [Authorize]
        public IActionResult GetProfile()
        {
            // Retrieve user profile information from the token
            var userId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User not authenticated");
            }
            // Here you would typically retrieve user details from the database
            return Ok(new { UserId = userId, Message = "User profile retrieved successfully" });
        }
    }
}