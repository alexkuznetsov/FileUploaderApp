using System.Threading;
using System.Threading.Tasks;

namespace FileUploadApp.Storage;

public interface IStoreBackend<in TKey, TMeta, TIn>
    where TKey : struct
    where TMeta : class
    where TIn : class
{
    Task SaveAsync(TIn file, CancellationToken cancellationToken = default);

    Task<TIn> FindAsync(TKey key, CancellationToken cancellationToken = default);

    Task DeleteAsync(TMeta key, CancellationToken cancellationToken = default);
}