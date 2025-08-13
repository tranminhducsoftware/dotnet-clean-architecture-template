// Copyright (c) 2025 tranminhducsoftware. Author: Tran Minh Duc. Licensed under MIT.

using CleanArchExample.Application.Features.Auth.Dtos;

using MediatR;

namespace CleanArchExample.Application.Features.Auth.Commands;

/// <summary>
/// Command for user authentication.
/// </summary>
/// <param name="Username">The username of the user.</param>
/// <param name="Password">The password of the user.</param>
/// <param name="DeviceId">Optional device identifier.</param>
/// <param name="UserAgent">Optional user agent string.</param>
/// <param name="Ip">Optional IP address.</param>
/// <remarks>
/// This command facilitates user authentication by requiring a username and password.
/// Additional optional parameters such as device ID, user agent, and IP address can be provided for enhanced tracking.
/// </remarks>
///
/// <example>
/// Example usage:
/// <code>
/// var command = new LoginCommand("exampleUser", "examplePassword", "device123", "Mozilla/5.0", "192.168.1.1");
/// </code>
/// </example>
///
/// <seealso cref="LoginResultDto"/>
public record LoginCommand(string Username, string Password, string? DeviceId, string? UserAgent, string? Ip) : IRequest<LoginResultDto>;
