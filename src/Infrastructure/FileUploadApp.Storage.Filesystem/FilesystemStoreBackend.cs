using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using FileUploadApp.Domain;

using Microsoft.Extensions.Logging;

namespace FileUploadApp.Storage.Filesystem;

internal sealed class FilesystemStoreBackend(StorageConfiguration storageConfiguration
        , ILogger<FilesystemStoreBackend> logger) : FileStoreBackendBase(storageConfiguration, logger)
    , IStoreBackend<Guid, Metadata, Upload>
    , IFileStreamProvider<Guid, Stream>
{

    public ValueTask<Upload?> FindAsync(Guid key, CancellationToken cancellationToken = default)
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