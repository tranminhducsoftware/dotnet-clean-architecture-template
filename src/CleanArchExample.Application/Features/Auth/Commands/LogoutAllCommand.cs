
// Copyright (c) 2025 tranminhducsoftware. Author: Tran Minh Duc. Licensed under MIT.

using MediatR;

namespace CleanArchExample.Application.Features.Auth.Commands;

/// <summary>
/// Represents a command to log out all active sessions for a specific user.
/// </summary>
/// <param name="UserId">The unique identifier of the user whose sessions are to be logged out.</param>
/// <param name="Reason">An optional reason for logging out all sessions.</param>
public sealed record LogoutAllCommand(Guid UserId, string? Reason = null) : IRequest;