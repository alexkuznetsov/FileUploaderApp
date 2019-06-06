using System;

namespace FileUploadApp.Interfaces
{
    public interface IContentDownloaderFactory<TOut>
    {
        IContentDownloader<TOut> Create(Uri uri);
    }

}
