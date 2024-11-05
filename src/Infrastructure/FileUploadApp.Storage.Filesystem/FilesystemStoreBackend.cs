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
        
        await upload.Stream.CopyToAsync(wri, cancellationToken);
        
        wri.Flush(true);
        wri.Close();
        wri.Dispose();
    }

    public Task DeleteAsync(Metadata metadata, CancellationToken cancellationToken = default)
    {
        try
        {
            cancellationToken.ThrowIfCancellationRequested();

            var filePath = BuildPathAndCheckDir(metadata.Id, false);
            int tries = 0;
            bool removed = false;

            while (tries < 3)
            {
                if (!UnlinkFile(filePath, out var err))
                {
                    logger.LogError(err, "Can't delete the file");
                    tries++;
                    GC.Collect();
                }
                else
                {
                    removed = true;
                    break;
                }
            }

            if (!removed)
            {
                throw new InvalidOperationException($"File not removed: {metadata}");
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