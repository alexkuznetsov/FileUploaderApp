using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using FileUploadApp.Interfaces;

using Microsoft.Extensions.Logging;

namespace FileUploadApp.Storage.Filesystem;

internal sealed class MetadataFsStoreBackend(StorageConfiguration storageConfiguration
        , ISerializer serializer
        , IDeserializer deserializer
        , ILogger<MetadataFsStoreBackend> logger) : FileStoreBackendBase(storageConfiguration, logger), IStoreBackend<Guid, Metadata, Metadata>
{
    private static readonly string SpecFileExtension = ".spec";

    public async Task SaveAsync(Metadata file, CancellationToken cancellationToken = default)
    {
        var path = BuildPathAndCheckDir(file.Id, true);
        var specFilePath = FormatSpecFilePath(path);

        await serializer.SerializeAsync(file, specFilePath, cancellationToken);
    }

    public async ValueTask<Metadata?> FindAsync(Guid key, CancellationToken cancellationToken = default)
    {
        var path = BuildPathAndCheckDir(key, false);
        var specFilePath = FormatSpecFilePath(path);

        return !File.Exists(specFilePath)
            ? default
            : await deserializer.DeserializeAsync<Metadata>(specFilePath, cancellationToken);
    }

    public Task DeleteAsync(Metadata metadata, CancellationToken cancellationToken = default)
    {
        try
        {
            cancellationToken.ThrowIfCancellationRequested();

            var filePath = BuildPathAndCheckDir(metadata.Id, false);
            var specFilePath = FormatSpecFilePath(filePath);
            int tries = 0;
            bool removed = false;

            while (tries < 3)
            {
                if (!UnlinkFile(specFilePath, out var err))
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
                throw new InvalidOperationException($"Meta file not removed: {metadata}");
            }

            RemoveDirIfEmpty(Path.GetDirectoryName(specFilePath));

            return Task.CompletedTask;
        }
        catch (OperationCanceledException)
        {
            return Task.FromCanceled(cancellationToken);
        }
    }

    private static string FormatSpecFilePath(string file) => $"{file}{SpecFileExtension}";
}