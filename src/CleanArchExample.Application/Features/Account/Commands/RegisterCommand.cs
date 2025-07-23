using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanArchExample.Application.Features.Auth.Dtos;
using MediatR;

namespace CleanArchExample.Application.Features.Auth.Commands
{
    public class RegisterCommand : IRequest<RegisterResultDto>
    {
        public required string UserName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
    }

}