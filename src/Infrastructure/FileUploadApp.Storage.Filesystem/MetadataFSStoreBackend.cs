using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using FileUploadApp.Interfaces;

using Microsoft.Extensions.Logging;

namespace FileUploadApp.Storage.Filesystem;

public class MetadataFsStoreBackend : FileStoreBackendBase, IStoreBackend<Guid, Metadata, Metadata>
{
    private const string SpecFileExtension = ".spec";

    private readonly ISerializer _serializer;
    private readonly IDeserializer _deserializer;

    public MetadataFsStoreBackend(StorageConfiguration storageConfiguration
        , ISerializer serializer
        , IDeserializer deserializer
        , ILogger<MetadataFsStoreBackend> logger)
        : base(storageConfiguration, logger)
    {
        this._serializer = serializer;
        this._deserializer = deserializer;
    }

    public Task SaveAsync(Metadata file, CancellationToken cancellationToken = default)
    {
        var path = BuildPathAndCheckDir(file.Id, true);
        var specFilePath = FormatSpecFilePath(path);

        return _serializer.SerializeAsync(file, specFilePath, cancellationToken);
    }

    public ValueTask<Metadata?> FindAsync(Guid key, CancellationToken cancellationToken = default)
    {
        var path = BuildPathAndCheckDir(key, false);
        var specFilePath = FormatSpecFilePath(path);

        if (!File.Exists(specFilePath)) return default;

        return _deserializer.DeserializeAsync<Metadata>(specFilePath, cancellationToken);
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