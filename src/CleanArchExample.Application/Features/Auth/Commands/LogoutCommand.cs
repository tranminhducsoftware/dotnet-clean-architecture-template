
// Copyright (c) 2025 tranminhducsoftware. Author: Tran Minh Duc. Licensed under MIT.

using MediatR;

namespace CleanArchExample.Application.Features.Auth.Commands;

/// <summary>
/// Represents a command to log out a user by ending their session.
/// </summary>
/// <param name="SessionId">The unique identifier of the user's session to be terminated.</param>
public sealed record LogoutCommand(string SessionId) : IRequest;