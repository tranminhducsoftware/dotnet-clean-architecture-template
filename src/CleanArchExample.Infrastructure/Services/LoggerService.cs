// Copyright (c) 2025 tranminhducsoftware. Author: Tran Minh Duc. Licensed under MIT.

using System.Diagnostics;

using CleanArchExample.Application.Interfaces.Services;

using Microsoft.Extensions.Logging;

namespace CleanArchExample.Infrastructure.Logging;

public class LoggerService : ILoggerService
{
    private readonly ILogger<LoggerService> _logger;

    public LoggerService(ILogger<LoggerService> logger)
    {
        _logger = logger;
    }

    private string Format(string message, string? userId)
    {
        var traceId = Activity.Current?.TraceId.ToString() ?? "N/A";
        var requestId = Activity.Current?.Id ?? "N/A";
        return $"[TraceId:{traceId} RequestId:{requestId} UserId:{userId ?? "anonymous"}] {message}";
    }

    public void LogInformation(string message, string? userId = null)
    {
        _logger.LogInformation(Format(message, userId));
    }

    public void LogWarning(string message, string? userId = null)
    {
        _logger.LogWarning(Format(message, userId));
    }

    public void LogError(string message, Exception? ex = null, string? userId = null)
    {
        _logger.LogError(ex, Format(message, userId));
    }
}
