// Copyright (c) 2025 tranminhducsoftware. Author: Tran Minh Duc. Licensed under MIT.

namespace CleanArchExample.Application.Features.Auth.Dtos
{
    public class RefreshTokenResultDto
    {
        public required string AccessToken { get; set; }

        public required string RefreshToken { get; set; }

        public required string UserName  { get; set; }
        
         public required List<string> Roles { get; set; }
    }
}