using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace CleanArchExample.Application.Features.Auth.Commands
{
    public class ResetPasswordCommand : IRequest<Unit>
    {
        public required string Email { get; set; }
        public required string NewPassword { get; set; }
    }
}