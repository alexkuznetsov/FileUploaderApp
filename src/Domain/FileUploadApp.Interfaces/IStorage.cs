using System.Threading;
using System.Threading.Tasks;

namespace FileUploadApp.Interfaces
{
    public interface IStore<in TKey, TIn, TOut>
        where TKey : struct
        where TIn : class
    {
        Task<TOut> StoreAsync(TIn file, CancellationToken cancellationToken = default);

        Task<TIn> ReceiveAsync(TKey fileId, CancellationToken cancellationToken = default);

        Task<bool> DeleteAsync(TKey fileId, CancellationToken cancellationToken = default);
    }
}