// Copyright (c) 2025 tranminhducsoftware. Author: Tran Minh Duc. Licensed under MIT.

using EFCoreSecondLevelCacheInterceptor;

using StackExchange.Redis;

namespace CleanArchExample.API.Extensions
{
    public static class CacheServiceCollectionExtensions
    {
        public static IServiceCollection AddAppSecondLevelCache(this IServiceCollection services, IConfiguration config)
        {
            var cacheSection = config.GetSection("Cache");
            var cacheProvider = cacheSection.GetValue<string>("CacheProvider");
            var enabled = cacheSection.GetValue<bool>("Enabled");
            var expiration = cacheSection.GetValue<int>("ExpirationInMinutes");

            if (!enabled) return services;

            if (string.Equals(cacheProvider, "Redis", StringComparison.OrdinalIgnoreCase))
            {
                var redisConfig = config.GetSection("Redis:Configuration").Value ?? "";
                var redisOptions = ConfigurationOptions.Parse(redisConfig);

                services.AddEFSecondLevelCache(options =>
                {
                    options.UseStackExchangeRedisCacheProvider(
                        redisOptions,
                        TimeSpan.FromMinutes(expiration)
                    );
                });
            }
            else
            {
                services.AddEFSecondLevelCache(options =>
                {
                    options.UseMemoryCacheProvider();
                });
            }

            return services;
        }
    }
}