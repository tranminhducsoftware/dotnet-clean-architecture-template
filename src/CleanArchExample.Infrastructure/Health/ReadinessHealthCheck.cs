// Copyright (c) 2025 tranminhducsoftware. Author: Tran Minh Duc. Licensed under MIT.

using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CleanArchExample.Infrastructure.Health;

public class ReadinessHealthCheck : IHealthCheck
{
    private readonly IAppHealthService _appHealth;
    public ReadinessHealthCheck(IAppHealthService appHealth) => _appHealth = appHealth;
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken ct)
    {
        return _appHealth.IsReady
            ? Task.FromResult(HealthCheckResult.Healthy("Ready"))
            : Task.FromResult(HealthCheckResult.Unhealthy("Not Ready"));
    }
}