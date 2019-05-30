using System.Threading;
using System.Threading.Tasks;

namespace FileUploadApp.Storage
{
    public interface IStoreBackend<in TKey, TIn>
    {
        Task SaveAsync(TIn file, CancellationToken cancellationToken = default);

        Task<TIn> FindAsync(TKey key, CancellationToken cancellationToken = default);
    }
}
