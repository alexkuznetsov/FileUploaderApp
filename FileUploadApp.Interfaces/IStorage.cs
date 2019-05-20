using System;
using System.Threading;
using System.Threading.Tasks;

namespace FileUploadApp.Interfaces
{
    public interface IStorage<TIn, TOut>
    {
        Task<TOut> StoreAsync(TIn file, CancellationToken cancellationToken = default);

        Task<TIn> ReceiveAsync(Guid fileId, CancellationToken cancellationToken = default);
    }
}
