// Copyright (c) 2025 tranminhducsoftware. Author: Tran Minh Duc. Licensed under MIT.

namespace CleanArchExample.Application.Features.Auth.Dtos;

public record RefreshTokenResultDto(string AccessToken, string RefreshToken, string SessionId, List<string> Roles);
