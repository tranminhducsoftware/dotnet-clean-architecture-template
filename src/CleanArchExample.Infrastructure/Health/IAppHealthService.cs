// Copyright (c) 2025 tranminhducsoftware. Author: Tran Minh Duc. Licensed under MIT.

namespace CleanArchExample.Infrastructure.Health;

public interface IAppHealthService
{
    bool IsReady { get; }
    void MarkAsNotReady();
}
