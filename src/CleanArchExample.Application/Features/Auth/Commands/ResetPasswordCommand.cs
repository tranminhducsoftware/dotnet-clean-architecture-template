// Copyright (c) 2025 tranminhducsoftware. Author: Tran Minh Duc. Licensed under MIT.

using MediatR;

namespace CleanArchExample.Application.Features.Auth.Commands
{
    public class ResetPasswordCommand : IRequest<Unit>
    {
        public required string Email { get; set; }
        public required string NewPassword { get; set; }
    }
}