// Copyright (c) 2025 tranminhducsoftware. Author: Tran Minh Duc. Licensed under MIT.

using Microsoft.Extensions.DependencyInjection;
using CleanArchExample.Infrastructure.Services;
using CleanArchExample.Infrastructure.Identity;
using Microsoft.Extensions.Configuration;
using CleanArchExample.Application.Common.Interfaces;
using CleanArchExample.Application.Interfaces.Services;

namespace CleanArchExample.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped(typeof(ILoggerService<>), typeof(LoggerService<>));
            services.AddMemoryCache();
            services.AddScoped<ICacheService, MemoryCacheService>();


            services.Configure<JwtOptions>(config.GetSection("Jwt"));
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            return services;
        }
    }
}