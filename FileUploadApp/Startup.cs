using FileUploadApp.Commands;
using FileUploadApp.Core;
using FileUploadApp.Core.Configuration;
using FileUploadApp.Core.Serialization;
using FileUploadApp.Domain;
using FileUploadApp.Events;
using FileUploadApp.Handlers;
using FileUploadApp.Interfaces;
using FileUploadApp.Middlewares;
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

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

            services.AddSingleton<ContentDownloaderFactory>();
            services.AddSingleton<SpecHandler>();
            services.AddSingleton(Configuration.BindTo<StorageConfiguration>(ConfigConstants.FileStoreNode));
            services.AddSingleton<IStorageProvider<Upload, UploadResultRow>, FilesystemStorageProvider>();

            services.AddScoped<EventGenerator>();
            services.AddScoped<UploadsContext>();
            services.AddScoped<ServiceFactory>(p => p.GetService);

            services.Scan(scan => scan
               .FromAssembliesOf(typeof(IMediator)
                    , typeof(FileUploadEvent)
                    , typeof(DownloadUriCommand)
                    , typeof(UploadRequestEventHandler))
               .AddClasses()
               .AsImplementedInterfaces());

            services.AddTransient<UploadedDataPreprocessMiddleware>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseMiddleware<UploadedDataPreprocessMiddleware>();

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
        }
    }
}
