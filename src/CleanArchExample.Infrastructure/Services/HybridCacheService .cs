// Copyright (c) 2025 tranminhducsoftware. Author: Tran Minh Duc. Licensed under MIT.

using CleanArchExample.Application.Interfaces.Services;

using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;

namespace CleanArchExample.Infrastructure.Services
{
    public class HybridCacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IDistributedCache _distributedCache;
        private readonly TimeSpan _memoryCacheExpiry; // default expiry cho memory

        public HybridCacheService(
            IMemoryCache memoryCache,
            IDistributedCache distributedCache,
            TimeSpan? memoryCacheExpiry = null)
        {
            _memoryCache = memoryCache;
            _distributedCache = distributedCache;
            _memoryCacheExpiry = memoryCacheExpiry ?? TimeSpan.FromMinutes(5); // mặc định 5 phút
        }

        public async Task SetAsync(string key, string value, TimeSpan? expiry = null)
        {
            // 1. Ghi vào Redis
            var distOptions = new DistributedCacheEntryOptions();
            if (expiry.HasValue) distOptions.AbsoluteExpirationRelativeToNow = expiry;
            await _distributedCache.SetStringAsync(key, value, distOptions);

            // 2. Ghi vào Memory cache (luôn có giá trị, có thể set thời gian ngắn hơn Redis)
            var memOptions = new MemoryCacheEntryOptions();
            memOptions.AbsoluteExpirationRelativeToNow = expiry ?? _memoryCacheExpiry;
            _memoryCache.Set(key, value, memOptions);
        }

        public async Task<string?> GetAsync(string key)
        {
            // 1. Thử lấy từ memory trước (siêu nhanh)
            if (_memoryCache.TryGetValue(key, out string? value) && value != null)
                return value;

            // 2. Nếu không có, lấy từ Redis
            value = await _distributedCache.GetStringAsync(key);

            // 3. Nếu Redis có thì update lại Memory cache
            if (value != null)
            {
                _memoryCache.Set(key, value, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = _memoryCacheExpiry
                });
            }
            return value;
        }

        public async Task RemoveAsync(string key)
        {
            // Xóa ở cả 2 tầng
            _memoryCache.Remove(key);
            await _distributedCache.RemoveAsync(key);
        }
    }
}