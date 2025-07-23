using CleanArchExample.Domain.Interfaces;
using CleanArchExample.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchExample.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services)
        {
            // Nếu bạn DI cấu hình DbContext ở ngoài (Program.cs), thì không cần thêm ở đây
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}