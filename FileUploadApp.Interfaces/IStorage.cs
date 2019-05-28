using System.Threading;
using System.Threading.Tasks;

namespace FileUploadApp.Interfaces
{
    public interface IStore<in TKey, TIn, TOut>
    {
        Task<TOut> StoreAsync(TIn file, CancellationToken cancellationToken = default);

        Task<TIn> ReceiveAsync(TKey fileId, CancellationToken cancellationToken = default);
    }
}