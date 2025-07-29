// Copyright (c) 2025 tranminhducsoftware. Author: Tran Minh Duc. Licensed under MIT.

namespace CleanArchExample.Application.Interfaces.Services
{
    public interface ILoggerService
    {
        void LogInformation(string message, string? userId = null);
        void LogWarning(string message, string? userId = null);
        void LogError(string message, Exception? ex = null, string? userId = null);
    }
}