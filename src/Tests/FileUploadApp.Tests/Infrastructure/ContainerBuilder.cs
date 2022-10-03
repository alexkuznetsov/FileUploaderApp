using FileUploadApp.Authentication;
using FileUploadApp.Authentication.Queries;
using FileUploadApp.Core.Serialization;
using FileUploadApp.Domain;
using FileUploadApp.Domain.Raw;
using FileUploadApp.Features;
using FileUploadApp.Features.Services;
using FileUploadApp.Interfaces;
using FileUploadApp.Storage;
using FileUploadApp.Storage.Filesystem;
using FileUploadApp.Tests.Fakes;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace FileUploadApp.Tests
{
    internal class ContainerBuilder
    {
        private static readonly Dictionary<string, string> ArrayDict = new Dictionary<string, string>
        {
            {"fileStore:BasePath", "d:\\temp\\uploads"},
            {"conf:AllowedContentTypes:0", "image/jpeg"           },
            {"conf:AllowedContentTypes:1", "image/png"            },
            {"conf:AllowedContentTypes:2", "image/bmp"            },
            {"conf:AllowedContentTypes:3", "image/x-windows-bmp"  },
            {"conf:AllowedContentTypes:4", "image/gif"            },
            {"conf:AllowedContentTypes:5", "image/tiff"           },
            {"conf:AllowedContentTypes:6", "application/x-7z-compressed" },

            {"Mappings:iVBORw", "image/png"},
            {"Mappings:/9j/4A", "image/jpeg"},
            {"Mappings:Qk0="  , "image/bmp"},
            {"Mappings:SUkq"  , "image/tiff"},
            {"Mappings:R0lG"  , "image/gif" },
            {"Mappings:N3q8rw=="  , "application/x-7z-compressed" }
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

        public static IServiceProvider Create(Action<IServiceCollection> configureServices = null)
        {
            var services = new ServiceCollection();

            var configuration = CreateConfiguration(services);

            services.AddSingleton(configuration.BindTo<AppConfiguration>(ConfigConstants.ConfNode));
            services.AddSingleton<IContentTypeTestUtility, ContentTypeTestUtility>();
            services.AddSingleton<ISerializer, Serializer>();
            services.AddSingleton<IDeserializer, Deserializer>();
            services.AddSingleton((r) => new HttpClientHandler
            {
                AllowAutoRedirect = true,
                AutomaticDecompression = System.Net.DecompressionMethods.Deflate | System.Net.DecompressionMethods.GZip
            });

            services.AddSingleton<IContentDownloaderFactory<DownloadUriResponse>, ContentDownloaderFactory>();
            services.AddSingleton(configuration.BindTo<StorageConfiguration>(ConfigConstants.FileStoreNode));

            var fakeStoreBackend = new FakeStoreBackend();

            services.AddSingleton<IStoreBackend<Guid, Metadata, Metadata>, FakeMetadataStoreBackend>();
            services.AddSingleton<IStoreBackend<Guid, Metadata, Upload>, FakeStoreBackend>((_) => fakeStoreBackend);
            services.AddSingleton<IFileStreamProvider<Guid, StreamAdapter>, FakeStoreBackend>((_) => fakeStoreBackend);
            services.AddSingleton<IStore<Guid, Upload, UploadResultRow>, FileSystemStore>();

            services.AddScoped<ServiceFactory>(p => p.GetService);

            services.AddMediatR(new[]
            {
                  typeof(GenericEvent).Assembly
                , typeof(CheckUser.Handler).Assembly
            });

            Log.Logger = new LoggerConfiguration()
               .ReadFrom.Configuration(configuration)
               .Enrich.FromLogContext()
               .CreateLogger();

            services.AddLogging((c) => c.AddSerilog(Log.Logger));

            services.AddJwtAuthenticationEndpointWithInMemoryService(configuration, (o) =>
            {
                o.WithUser("rex", "1qaz!QAZ")
                ;
            });

            configureServices?.Invoke(services);

            var result = services.BuildServiceProvider();

            return result;
        }
    }
}
