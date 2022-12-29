using System;
using System.Threading;
using System.Threading.Tasks;

namespace FileUploadApp.Interfaces
{
    public interface IContentDownloader<TOut>
    {
        Task<TOut> DownloadAsync(Uri uri,CancellationToken cancellationToken = default);
    }

}
