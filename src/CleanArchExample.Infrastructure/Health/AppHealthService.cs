// Copyright (c) 2025 tranminhducsoftware. Author: Tran Minh Duc. Licensed under MIT.

namespace CleanArchExample.Infrastructure.Health
{
    public class AppHealthService : IAppHealthService
    {
        private bool _isReady = true;
        public bool IsReady => _isReady;
        public void MarkAsNotReady() => _isReady = false;
    }
}