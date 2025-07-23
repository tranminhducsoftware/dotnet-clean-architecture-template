using CleanArchExample.Application.Common.Interfaces;
using CleanArchExample.Application.Features.Auth.Commands;
using CleanArchExample.Application.Features.Auth.Dtos;
using MediatR;

namespace CleanArchExample.Application.Features.Users.Handler
{
    public class LoginUserCommandHandler : IRequestHandler<LoginCommand, LoginResultDto>
    {
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public LoginUserCommandHandler(IJwtTokenGenerator jwtTokenGenerator)
        {
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<LoginResultDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            // TODO: check DB/identity real
            if (request.Username == "admin" && request.Password == "123456")
            {
                var token = _jwtTokenGenerator.GenerateToken(request.Username, "adminId");
                var output = new LoginResultDto
                {
                    RefreshToken = Guid.NewGuid().ToString(), // Generate a new refresh token
                    AccessToken = token,
                    UserName = request.Username,
                    Roles = ["Admin"] // Example roles, replace with actual roles from DB
                };
                return output;
            }
            throw new UnauthorizedAccessException("Invalid username or password");
        }
    }
}