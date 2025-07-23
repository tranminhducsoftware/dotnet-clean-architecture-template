using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanArchExample.Application.Features.Auth.Dtos;
using MediatR;

namespace CleanArchExample.Application.Features.Auth.Commands
{
    public class RefreshTokenCommand : IRequest<RefreshTokenResultDto>
    {
          public required string RefreshToken { get; set; }
    }
}