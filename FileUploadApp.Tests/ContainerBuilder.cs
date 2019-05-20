using FileUploadApp.Requests;
using FileUploadApp.Core;
using FileUploadApp.Core.Serialization;
using FileUploadApp.Domain;
using FileUploadApp.Events;
using FileUploadApp.Handlers;
using FileUploadApp.Interfaces;
using FileUploadApp.Services;
using FileUploadApp.Storage;
using FileUploadApp.Storage.Filesystem;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net.Http;
using FileUploadApp.Tests.Fakes;
using FileUploadApp.Domain.Dirty;

namespace FileUploadApp.Tests
{
    class ContainerBuilder
    {
        public static Dictionary<string, string> arrayDict = new Dictionary<string, string>
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

        internal static class ConfigConstants
        {
            public static readonly string ConfNode = "conf";
            public static readonly string FileStoreNode = "fileStore";
            public static readonly string UploadFile = "/api/upload";
        }

        IConfiguration CreateConfiguration(IServiceCollection services)
        {
            var configBuilder = new ConfigurationBuilder();
            configBuilder.AddInMemoryCollection(arrayDict);
            var configuration = configBuilder.Build();

            services.AddSingleton(configuration);

            return configuration;
        }

        public IServiceProvider Create(Action<IServiceCollection> configureServices = null)
        {
            var services = new ServiceCollection();
            var configuration = CreateConfiguration(services);

            services.AddSingleton(configuration.BindTo<AppConfiguration>(ConfigConstants.ConfNode));
            services.AddSingleton<IContentTypeTestUtility, ContentTypeTestUtility>();
            services.AddSingleton<ISerializer, Serializer>();
            services.AddSingleton<IDeserializer, Deserializer>();
            services.AddSingleton((r) =>
            {
                return new HttpClientHandler
                {
                    AllowAutoRedirect = true,
                    AutomaticDecompression = System.Net.DecompressionMethods.Deflate | System.Net.DecompressionMethods.GZip
                };
            });

            services.AddSingleton<IContentDownloaderFactory<DownloadUriResponse>, ContentDownloaderFactory>();
            services.AddSingleton(configuration.BindTo<StorageConfiguration>(ConfigConstants.FileStoreNode));

            services.AddSingleton<IStoreBackend<Guid, Upload>, FakeStoreBackend>();
            services.AddSingleton<IStoreBackend<Guid, Metadata>, FakeMetadataStoreBackend>();

            services.AddSingleton<IFileStreamProvider<Guid, StreamAdapter>, FakeStoreBackend>();
            services.AddSingleton<IStore<Upload, UploadResultRow>,FakeStorage>();

            services.AddScoped<EventGenerator>();
            services.AddScoped<UploadsContext>();
            services.AddScoped<ServiceFactory>(p => p.GetService);

            services.Scan(scan => scan
               .FromAssembliesOf(typeof(IMediator)
                    , typeof(GenericEvent)
                    , typeof(DownloadUriQuery)
                    , typeof(UploadFilesCommandHandler))
               .AddClasses()
               .AsImplementedInterfaces());

            configureServices?.Invoke(services);

            var result = services.BuildServiceProvider();

            return result;
        }
    }
}
