// Copyright (c) 2025 tranminhducsoftware. Author: Tran Minh Duc. Licensed under MIT.

namespace CleanArchExample.API.Contracts.Auth;

public record RefreshTokenResponse(string AccessToken, string RefreshToken, string SessionId, List<string> Roles);