using System;
using System.Threading;
using System.Threading.Tasks;

namespace FileUploadApp.Interfaces;
public interface ICache
{
    Task<string?> GetStringAsync(string key, CancellationToken cancellationToken = default);

    Task SetStringAsync(string key, string value, Action<ICacheOptions> configure, CancellationToken cancellationToken = default);
}


public interface ICacheOptions
{
    TimeSpan AbsoluteExpirationRelativeToNow { get; set; }
}