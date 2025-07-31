// Copyright (c) 2025 tranminhducsoftware. Author: Tran Minh Duc. Licensed under MIT.

using Microsoft.Extensions.DependencyInjection;
using CleanArchExample.Infrastructure.Services;
using CleanArchExample.Infrastructure.Identity;
using Microsoft.Extensions.Configuration;
using CleanArchExample.Application.Common.Interfaces;
using CleanArchExample.Application.Interfaces.Services;
using CleanArchExample.Infrastructure.Logging;
using CleanArchExample.Infrastructure.Health;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CleanArchExample.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<IEmailService, EmailService>();
            // services.AddScoped(typeof(ILoggerService<>), typeof(LoggerService<>));
            services.AddScoped<ILoggerService, LoggerService>();
            services.AddHttpContextAccessor(); // Nếu cần UserId theo HttpContext

            services.Configure<JwtOptions>(config.GetSection("Jwt"));
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();



            // 1. Đăng ký Redis (cần trước để RedisCacheService dùng được)
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = config.GetSection("Redis:Configuration").Value;
                // options.InstanceName = "CleanArch_";
            });

            // 2. Đăng ký MemoryCache (nếu có dùng MemoryCacheService)
            services.AddMemoryCache();

            // 3. Đăng ký provider ICacheService chọn động theo config
            var provider = config.GetValue<string>("Cache:CacheProvider");
            if (provider == "Redis")
                services.AddScoped<ICacheService, RedisCacheService>();
            else if (provider == "Hybrid")
                services.AddScoped<ICacheService, HybridCacheService>();
            // Mặc định là MemoryCache nếu không có cấu hình rõ ràng
            else
                services.AddScoped<ICacheService, MemoryCacheService>();

            services.AddSingleton<IAppHealthService, AppHealthService>();
            services.AddHealthChecks()
                .AddCheck("self", () => HealthCheckResult.Healthy(), tags: ["live"])
                .AddCheck<ReadinessHealthCheck>("readiness_check", tags: ["ready"])
            .AddSqlServer(config.GetConnectionString("DefaultConnection") ?? "", name: "sql", tags: ["ready"])
            .AddRedis(config["Redis:Configuration"] ?? "", name: "redis", tags: ["ready"]);
            return services;
        }
    }
}