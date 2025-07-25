// Copyright (c) 2025 tranminhducsoftware. Author: Tran Minh Duc. Licensed under MIT.

using MediatR;

namespace CleanArchExample.Application.Features.Auth.Commands
{
    public class ForgotPasswordCommand : IRequest<Unit>
    {
        public required string Email { get; set; }
    }
}