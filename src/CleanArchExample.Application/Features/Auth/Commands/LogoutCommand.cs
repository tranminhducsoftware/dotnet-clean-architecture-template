using MediatR;

namespace CleanArchExample.Application.Features.Auth.Commands
{
    public class LogoutCommand : IRequest<Unit>
    {
        public required string UserName { get; set; }
    }
}