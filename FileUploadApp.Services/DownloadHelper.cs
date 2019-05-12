using FileUploadApp.Core.Streams;
using FileUploadApp.Domain;
using FileUploadApp.Interfaces;
using System;
using System.IO;
using System.Threading.Tasks;

namespace FileUploadApp.Services
{
    public class DownloadHelper : IDownloadHelper
    {
        private readonly ContentDownloaderFactory downloaderFactory;
        private readonly IUploadService uploadService;
        private readonly IContentTypeTestUtility contentTypeChecker;
        private readonly int MaxBitsCountForBase64 = 4;

        public DownloadHelper(ContentDownloaderFactory downloaderFactory, IUploadService uploadService, IContentTypeTestUtility contentTypeChecker)
        {
            this.downloaderFactory = downloaderFactory;
            this.uploadService = uploadService;
            this.contentTypeChecker = contentTypeChecker;
        }

        public Task Download(Uri u)
        {
            var downloader = downloaderFactory.Create(u);
            return downloader.Download().ContinueWith(r => AfterDownloadCompleated(u, r));
        }

        private void AfterDownloadCompleated(Uri u, Task<ReadOnlyMemory<byte>> r)
        {
            var byteArray = r.Result;
            var base64 = Convert.ToBase64String(byteArray.Slice(0, MaxBitsCountForBase64).ToArray());
            var contentType = contentTypeChecker.DetectContentType(base64);

            if (contentTypeChecker.IsAllowed(contentType))
            {
                uploadService.UploadedFiles.Add(new UploadedFile(
                  name: Path.GetFileName(u.LocalPath),
                  contentType: contentType,
                  height:0,
                  width:0,
                  streamWrapper: new ByteaStreamWrapper(byteArray)
                 )
                );
            }
        }
    }
}
