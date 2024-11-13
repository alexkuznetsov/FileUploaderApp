using System;
using System.Threading;
using System.Threading.Tasks;

using FileUploadApp.Interfaces;

using Microsoft.Extensions.Caching.Distributed;

namespace FileUploadApp.Application.Services;

internal sealed class DefaultCacheImpl(IDistributedCache memoryCache) : ICache
{
    public Task<string?> GetStringAsync(string key, CancellationToken cancellationToken = default)
        => memoryCache.GetStringAsync(key, cancellationToken);

    public Task SetStringAsync(string key, string value, Action<ICacheOptions> configure, CancellationToken cancellationToken = default)
    {
        var o = new DefaultCacheItemOptions();
        configure(o);

        return memoryCache.SetStringAsync(key, value, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = o.AbsoluteExpirationRelativeToNow,
        }, cancellationToken);
    }


    sealed class DefaultCacheItemOptions : ICacheOptions
    {
        public TimeSpan AbsoluteExpirationRelativeToNow { get; set; }
    }
}
