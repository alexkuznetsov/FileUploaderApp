using System.Threading;
using System.Threading.Tasks;

namespace FileUploadApp.Interfaces
{
    public interface IContentDownloader<TOut>
    {
        Task<TOut> DownloadAsync(CancellationToken cancellationToken = default);
    }

}
