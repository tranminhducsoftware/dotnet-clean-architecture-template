// Copyright (c) 2025 tranminhducsoftware. Author: Tran Minh Duc. Licensed under MIT.

using CleanArchExample.Application.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace CleanArchExample.Infrastructure.Services
{
    public class LoggerService<T> : ILoggerService<T>
    {
        private readonly ILogger<T> _logger;
        public LoggerService(ILogger<T> logger)
        {
            _logger = logger;
        }

        public void LogInfo(string message)
        {
            _logger.LogInformation(message);
        }

        public void LogError(string message, Exception ex)
        {
            _logger.LogError(ex, message);
        }
    }
}

