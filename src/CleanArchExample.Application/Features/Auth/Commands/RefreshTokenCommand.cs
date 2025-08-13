
// Copyright (c) 2025 tranminhducsoftware. Author: Tran Minh Duc. Licensed under MIT.

using CleanArchExample.Application.Features.Auth.Dtos;

using MediatR;

namespace CleanArchExample.Application.Features.Auth.Commands;

/// <summary>
/// Represents a command to refresh an authentication token.
/// </summary>
/// <param name="RefreshToken">The refresh token used to obtain a new access token.</param>
/// <param name="SessionId">The identifier for the current session.</param>
/// <param name="FamilyId">The identifier for the token family, used for managing token hierarchies.</param>
/// <returns>A <see cref="RefreshTokenResultDto"/> containing the result of the refresh operation.</returns>
public sealed record RefreshTokenCommand(string RefreshToken, string SessionId, string FamilyId) : IRequest<RefreshTokenResultDto>;