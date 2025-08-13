// Copyright (c) 2025 tranminhducsoftware. Author: Tran Minh Duc. Licensed under MIT.

namespace CleanArchExample.API.Contracts.Auth;

public sealed record RefreshTokenRequest(string RefreshToken, string SessionId, string FamilyId);