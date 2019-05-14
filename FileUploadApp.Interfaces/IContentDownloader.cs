using System;
using System.Threading.Tasks;

namespace FileUploadApp.Interfaces
{
    public interface IContentDownloader
    {
        Task<byte[]> DownloadAsync(System.Threading.CancellationToken cancellationToken = default);
    }

}
