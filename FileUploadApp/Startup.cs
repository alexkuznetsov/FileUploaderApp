using FileUploadApp.Requests;
using FileUploadApp.Core.Serialization;
using FileUploadApp.Domain;
using FileUploadApp.Events;
using FileUploadApp.Handlers;
using FileUploadApp.Interfaces;
using FileUploadApp.Services;
using FileUploadApp.Storage.Filesystem;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using System.Linq;
using System.Net.Http;
using FileUploadApp.Storage;
using System;
using FileUploadApp.Domain.Dirty;

namespace FileUploadApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options =>
            {
                options.EnableEndpointRouting = false;
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSingleton(Configuration.BindTo<AppConfiguration>(ConfigConstants.ConfNode));
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

            services.AddSingleton(Configuration.BindTo<StorageConfiguration>(ConfigConstants.FileStoreNode));

            services.AddSingleton<IStoreBackend<Guid, Upload>, FilesystemStoreBackend>();
            services.AddSingleton<IStoreBackend<Guid, Metadata>, MetadataFSStoreBackend>();
            services.AddSingleton<IFileStreamProvider<Guid, StreamAdapter>, FilesystemStoreBackend>();
            services.AddSingleton<IStore<Upload, UploadResultRow>, FileSystemStore>();

            services.AddScoped<ServiceFactory>(p => p.GetService);

            services.Scan(scan => scan
               .FromAssembliesOf(typeof(IMediator)
                    , typeof(GenericEvent)
                    , typeof(DownloadUriQuery)
                    , typeof(UploadFilesCommandHandler))
               .AddClasses()
               .AsImplementedInterfaces());

            //services.AddTransient<UploadedDataPreprocessMiddleware>();
            services.AddCors((s) =>
            {
                s.AddDefaultPolicy((c) =>
                {
                    c.AllowAnyOrigin();
                    c.AllowAnyHeader();
                    c.WithMethods("OPTIONS", "GET", "POST");
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //app.UseWhen(x => x.Request.Path.StartsWithSegments(ConfigConstants.UploadFile), c =>
            //{
            //    c.UseMiddleware<UploadedDataPreprocessMiddleware>();
            //});

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
            app.UseCors();
        }
    }
}
