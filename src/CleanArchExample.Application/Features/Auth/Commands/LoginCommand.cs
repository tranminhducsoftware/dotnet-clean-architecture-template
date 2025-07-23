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