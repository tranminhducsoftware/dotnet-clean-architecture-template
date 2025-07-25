// Copyright (c) 2025 tranminhducsoftware. Author: Tran Minh Duc. Licensed under MIT.

using CleanArchExample.Application.Features.Auth.Dtos;

using MediatR;

namespace CleanArchExample.Application.Features.Auth.Commands
{
    public class RefreshTokenCommand : IRequest<RefreshTokenResultDto>
    {
          public required string RefreshToken { get; set; }
    }
}