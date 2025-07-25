// Copyright (c) 2025 tranminhducsoftware. Author: Tran Minh Duc. Licensed under MIT.

namespace CleanArchExample.Application.Interfaces.Services
{
    public interface ILoggerService<T>
    {
        void LogInfo(string message);
        void LogError(string message, Exception ex);
    }
}