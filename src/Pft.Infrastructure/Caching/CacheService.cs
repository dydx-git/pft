﻿using System.Buffers;
using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Pft.Application.Abstractions.Caching;

namespace Pft.Infrastructure.Caching;
internal sealed class CacheService(IDistributedCache distributedCache) : ICacheService
{
    public async Task<T> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        var bytes = await distributedCache.GetAsync(key, cancellationToken);

        return bytes is null ? default : Deserialize<T>(bytes);
    }

    public Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        return distributedCache.RemoveAsync(key, cancellationToken);
    }

    public Task SetAsync<T>(string key, T value, TimeSpan? expiration = null, CancellationToken cancellationToken = default)
    {
        var bytes = Serialize(value);

        return distributedCache.SetAsync(key, bytes, CacheOptions.Create(expiration), cancellationToken);
    }

    private static T Deserialize<T>(byte[] bytes) => JsonSerializer.Deserialize<T>(bytes);

    private static byte[] Serialize<T>(T value)
    {
        var buffer = new ArrayBufferWriter<byte>();

        using var writer = new Utf8JsonWriter(buffer);

        JsonSerializer.Serialize(writer, value);

        return buffer.WrittenSpan.ToArray();
    }
}