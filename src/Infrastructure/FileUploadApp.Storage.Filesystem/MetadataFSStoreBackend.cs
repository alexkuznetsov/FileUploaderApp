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

    public Task SaveAsync(Metadata file, CancellationToken cancellationToken = default)
    {
        var path = BuildPathAndCheckDir(file.Id, true);
        var specFilePath = FormatSpecFilePath(path);

        return serializer.SerializeAsync(file, specFilePath, cancellationToken);
    }

    public ValueTask<Metadata?> FindAsync(Guid key, CancellationToken cancellationToken = default)
    {
        var path = BuildPathAndCheckDir(key, false);
        var specFilePath = FormatSpecFilePath(path);

        return !File.Exists(specFilePath) 
            ? default 
            : deserializer.DeserializeAsync<Metadata>(specFilePath, cancellationToken);
    }

    public Task DeleteAsync(Metadata metadata, CancellationToken cancellationToken = default)
    {
        try
        {
            cancellationToken.ThrowIfCancellationRequested();

            var filePath = BuildPathAndCheckDir(metadata.Id, false);
            var specFilePath = FormatSpecFilePath(filePath);

            if (File.Exists(specFilePath))
            {
                File.Delete(specFilePath);
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