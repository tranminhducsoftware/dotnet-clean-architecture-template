// Copyright (c) 2025 tranminhducsoftware. Author: Tran Minh Duc. Licensed under MIT.

namespace CleanArchExample.Application.Features.Auth.Dtos;

/// <summary>
/// Represents the result of a login operation.
/// </summary>
/// <param name="AccessToken">The access token issued upon successful authentication.</param>
/// <param name="RefreshToken">The refresh token issued for obtaining new access tokens.</param>
/// <param name="UserName">The username of the authenticated user.</param>
/// <param name="Roles">The list of roles assigned to the authenticated user.</param>
public record LoginResultDto(string AccessToken, string RefreshToken, string SessionId, List<string> Roles);