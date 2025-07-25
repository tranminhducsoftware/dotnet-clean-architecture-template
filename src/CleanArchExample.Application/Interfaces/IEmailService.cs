// Copyright (c) 2025 tranminhducsoftware. Author: Tran Minh Duc. Licensed under MIT.

namespace CleanArchExample.Application.Interfaces.Services
{
    public interface IEmailService
    {
         Task SendAsync(string to, string subject, string body);
    }
}