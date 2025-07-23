using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace CleanArchExample.Application.Features.Auth.Commands
{
    public class ForgotPasswordCommand : IRequest<Unit>
    {
        public required string Email { get; set; }
    }
}