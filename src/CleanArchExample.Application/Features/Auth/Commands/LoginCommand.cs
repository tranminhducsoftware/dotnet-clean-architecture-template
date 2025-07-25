// Copyright (c) 2025 tranminhducsoftware. Author: Tran Minh Duc. Licensed under MIT.

using CleanArchExample.Application.Features.Auth.Dtos;
using MediatR;

namespace CleanArchExample.Application.Features.Auth.Commands
{
    public class LoginCommand : IRequest<LoginResultDto>
    {
        public required string Username { get; set; } 
        public required string Password { get; set; }
    }
}