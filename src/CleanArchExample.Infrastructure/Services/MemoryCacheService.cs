// Copyright (c) 2025 tranminhducsoftware. Author: Tran Minh Duc. Licensed under MIT.

using CleanArchExample.Application.Interfaces.Services;

using Microsoft.Extensions.Caching.Memory;

namespace CleanArchExample.Infrastructure.Services
{
    public class MemoryCacheService(IMemoryCache memoryCache) : ICacheService
    {
        private readonly IMemoryCache _memoryCache = memoryCache;

        public Task SetAsync(string key, string value, TimeSpan? expiry = null)
        {
            var options = new MemoryCacheEntryOptions();
            if (expiry.HasValue)
                options.AbsoluteExpirationRelativeToNow = expiry;
            _memoryCache.Set(key, value, options);
            return Task.CompletedTask;
        }

        public Task<string?> GetAsync(string key)
        {
            _memoryCache.TryGetValue(key, out string? value);
            return Task.FromResult(value);
        }

        public Task RemoveAsync(string key)
        {
            _memoryCache.Remove(key);
            return Task.CompletedTask;
        }
    }
}