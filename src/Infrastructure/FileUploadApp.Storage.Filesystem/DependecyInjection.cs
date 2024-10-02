using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FileUploadApp.Domain;

using FileUploadApp.Interfaces;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FileUploadApp.Storage.Filesystem;
public static class DependecyInjection
{
    private const string FileStoreNode = "fileStore";

    public static IServiceCollection AddFileStorage(this IServiceCollection services, IConfiguration configuration)
    {
        var storageConfig = new StorageConfiguration();

        configuration.GetSection(FileStoreNode).Bind(storageConfig);

        services.AddSingleton(storageConfig);
        services.AddSingleton<IStoreBackend<Guid, Metadata, Upload>, FilesystemStoreBackend>();
        services.AddSingleton<IStoreBackend<Guid, Metadata, Metadata>, MetadataFsStoreBackend>();
        services.AddSingleton<IFileStreamProvider<Guid, Stream>, FilesystemStoreBackend>();
        services.AddSingleton<IStore<Guid, Upload, UploadResultRow>, FileSystemStore>();

        return services;

    }
}
