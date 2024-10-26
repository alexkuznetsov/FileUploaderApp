using System;
using System.Collections.Generic;
using System.IO;

using FileUploadApp.Application;
using FileUploadApp.Application.Services;
using FileUploadApp.Domain;
using FileUploadApp.Interfaces;
using FileUploadApp.Storage;
using FileUploadApp.Storage.Filesystem;
using FileUploadApp.Tests.Fakes;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Serilog;

namespace FileUploadApp.Tests;

internal class ContainerBuilder
{
    private static readonly Dictionary<string, string?> ArrayDict = new()
    {
        {"fileStore:BasePath",         "e:\\temp\\uploads"            },
        {"conf:AllowedContentTypes:0", "image/jpeg"                   },
        {"conf:AllowedContentTypes:1", "image/png"                    },
        {"conf:AllowedContentTypes:2", "image/bmp"                    },
        {"conf:AllowedContentTypes:3", "image/x-windows-bmp"          },
        {"conf:AllowedContentTypes:4", "image/gif"                    },
        {"conf:AllowedContentTypes:5", "image/tiff"                   },
        {"conf:AllowedContentTypes:6", "application/x-7z-compressed"  },
        {"conf:PreviewContentType", "image/png" },

        {"Mappings:iVBORw",     "image/png"                   },
        {"Mappings:/9j/4A",     "image/jpeg"                  },
        {"Mappings:Qk0="  ,     "image/bmp"                   },
        {"Mappings:SUkq"  ,     "image/tiff"                  },
        {"Mappings:R0lG"  ,     "image/gif"                   },
        {"Mappings:N3q8rw=="  , "application/x-7z-compressed" },

        { "jwt:SecretKey"       ,   "as[pd[a0_)_22e89893edjaskld;ss" },
        { "jwt:Issuer"          ,   "FileUploadApp" },
        { "jwt:ExpiryMinutes"   ,   "60" },
        { "jwt:ValidateLifetime",   "true" },
        { "jwt:ValidateAudience",   "false" },
    };

    private static class ConfigConstants
    {
        public const string ConfNode = "conf";
        public const string FileStoreNode = "fileStore";
    }

    private static IConfiguration CreateConfiguration(IServiceCollection services)
    {
        var configBuilder = new ConfigurationBuilder();
        configBuilder.AddInMemoryCollection(ArrayDict);
        var configuration = configBuilder.Build();

        services.AddSingleton(configuration);

        return configuration;
    }

    public static IServiceProvider Create(Action<IServiceCollection>? configureServices = null)
    {
        var services = new ServiceCollection();

        var configuration = CreateConfiguration(services);


        services.AddSingleton(configuration);
        services.AddApplication(configuration);
        services.AddFileStorage(configuration);

        var fakeStoreBackend = new FakeStoreBackend();
        var store = new FakeFileSystemStore(/*fakeStoreBackend*/);

        services.AddSingleton<IStoreBackend<Guid, Metadata, Metadata>, FakeMetadataStoreBackend>();
        services.AddSingleton<IStoreBackend<Guid, Metadata, Upload>, FakeStoreBackend>((_) => fakeStoreBackend);
        services.AddSingleton<IFileStreamProvider<Guid, Stream>, FakeStoreBackend>((_) => fakeStoreBackend);
        services.AddSingleton<IStore<Guid, Upload, UploadResultRow>, FakeFileSystemStore>((_) => store);

        Log.Logger = new LoggerConfiguration()
           .ReadFrom.Configuration(configuration)
           .Enrich.FromLogContext()
           .CreateLogger();

        services.AddLogging((c) => c.AddSerilog(Log.Logger));

        services.Configure<InMemoryCheckUserServiceOptions>((o)
            => o.WithUser("admin", "1qaz!QAZ"));

        configureServices?.Invoke(services);

        var result = services.BuildServiceProvider();

        return result;
    }
}
