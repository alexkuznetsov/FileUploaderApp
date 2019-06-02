using FileUploadApp.Domain;
using FileUploadApp.StreamAdapters;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace FileUploadApp.Storage.Filesystem
{
    public class FilesystemStoreBackend : FileStoreBackendBase
        , IStoreBackend<Guid, Upload>
        , IFileStreamProvider<Guid, StreamAdapter>
    {
        public FilesystemStoreBackend(StorageConfiguration storageConfiguration
            , ILogger<FilesystemStoreBackend> logger)
            : base(storageConfiguration, logger)
        {
        }

        public Task<Upload> FindAsync(Guid key, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public StreamAdapter GetStreamAdapter(Guid id)
        {
            var path = BuildPathAndCheckDir(id, false);

            return new DownloadableStreamAdapter(path);
        }

        public async Task SaveAsync(Upload upload, CancellationToken cancellationToken = default)
        {
            var filePath = BuildPathAndCheckDir(upload.Id, true);

            using (var wri = File.OpenWrite(filePath))
            {
                await upload.Stream.CopyToAsync(wri, cancellationToken).ConfigureAwait(false);
                await wri.FlushAsync(cancellationToken).ConfigureAwait(false);
            }
        }

        public Task DeleteAsync(Guid key, CancellationToken cancellationToken = default)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                var filePath = BuildPathAndCheckDir(key, false);

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                RemoveDirIfEmpty(Path.GetDirectoryName(filePath));

                return Task.CompletedTask;
            }
            catch (OperationCanceledException)
            {
                return Task.FromCanceled(cancellationToken);
            }
        }
    }
}