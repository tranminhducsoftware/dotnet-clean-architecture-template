// Copyright (c) 2025 tranminhducsoftware. Author: Tran Minh Duc. Licensed under MIT.

using MediatR;

namespace CleanArchExample.Application.Features.Auth.Commands
{
    public class LogoutCommand : IRequest<Unit>
    {
        public required string UserName { get; set; }
    }
}