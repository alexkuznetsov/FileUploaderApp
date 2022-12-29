using FileUploadApp.Domain;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace FileUploadApp.Storage.Filesystem;

public class FilesystemStoreBackend : FileStoreBackendBase
    , IStoreBackend<Guid, Metadata, Upload>
    , IFileStreamProvider<Guid, Stream>
{
    public FilesystemStoreBackend(StorageConfiguration storageConfiguration
        , ILogger<FilesystemStoreBackend> logger)
        : base(storageConfiguration, logger)
    {
    }

    public Task<Upload> FindAsync(Guid key, CancellationToken cancellationToken = default) 
        => throw new NotImplementedException();

    public Stream GetStream(Guid id)
    {
        var path = BuildPathAndCheckDir(id, false);

        return File.OpenRead(path);
    }

    public async Task SaveAsync(Upload upload, CancellationToken cancellationToken = default)
    {
        var filePath = BuildPathAndCheckDir(upload.Id, true);

        using var wri = File.OpenWrite(filePath);
        await upload.Stream.CopyToAsync(wri, cancellationToken).ConfigureAwait(false);
        await wri.FlushAsync(cancellationToken).ConfigureAwait(false);
    }

    public Task DeleteAsync(Metadata metadata, CancellationToken cancellationToken = default)
    {
        try
        {
            cancellationToken.ThrowIfCancellationRequested();

            var filePath = BuildPathAndCheckDir(metadata.Id, false);

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