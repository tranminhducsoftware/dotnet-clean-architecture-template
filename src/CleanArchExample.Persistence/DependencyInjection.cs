// Copyright (c) 2025 tranminhducsoftware. Author: Tran Minh Duc. Licensed under MIT.

using CleanArchExample.Domain.Interfaces;
using CleanArchExample.Persistence.Contexts;
using CleanArchExample.Persistence.Interceptors;
using CleanArchExample.Persistence.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using EFCoreSecondLevelCacheInterceptor;

namespace CleanArchExample.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration config)
        {
            // Nếu bạn DI cấu hình DbContext ở ngoài (Program.cs), thì không cần thêm ở đây
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<AuditableEntityInterceptor>();
            services.AddSingleton<CommandLoggingInterceptor>();

            services.AddDbContext<AppDbContext>((sp, options) =>
            {
                var audit = sp.GetRequiredService<AuditableEntityInterceptor>();
                var sqlLog = sp.GetRequiredService<CommandLoggingInterceptor>();
                var cacheInterceptor = sp.GetRequiredService<SecondLevelCacheInterceptor>();

                options.UseSqlServer(config.GetConnectionString("DefaultConnection"));
                options.AddInterceptors(audit, sqlLog, cacheInterceptor);
            });

            return services;
        }
    }
}