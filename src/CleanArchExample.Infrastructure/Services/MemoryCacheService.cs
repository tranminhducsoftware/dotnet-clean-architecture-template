// Copyright (c) 2025 tranminhducsoftware. Author: Tran Minh Duc. Licensed under MIT.

using CleanArchExample.Application.Interfaces.Services;
using Microsoft.Extensions.Caching.Memory;

namespace CleanArchExample.Infrastructure.Services
{
 public class MemoryCacheService : ICacheService
{
    private readonly IMemoryCache _cache;

    public MemoryCacheService(IMemoryCache cache)
    {
        _cache = cache;
    }

    public T? Get<T>(string key) => _cache.TryGetValue(key, out var value) ? (T?)value : default;

    public void Set<T>(string key, T value, TimeSpan? expiration = null)
    {
        _cache.Set(key, value, expiration ?? TimeSpan.FromMinutes(5));
    }

    public void Remove(string key) => _cache.Remove(key);
}
}