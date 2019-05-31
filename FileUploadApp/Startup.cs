using FileUploadApp.Core.Authentication;
using FileUploadApp.Core.Serialization;
using FileUploadApp.Domain;
using FileUploadApp.Domain.Dirty;
using FileUploadApp.Events;
using FileUploadApp.Handlers;
using FileUploadApp.Interfaces;
using FileUploadApp.Requests;
using FileUploadApp.Services;
using FileUploadApp.Storage;
using FileUploadApp.Storage.Filesystem;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;

namespace FileUploadApp
{
    public class Startup
    {
        private const string ConfNode = "conf";
        private const string FileStoreNode = "fileStore";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options => { options.EnableEndpointRouting = false; })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSingleton(Configuration.BindTo<AppConfiguration>(ConfNode));
            services.AddSingleton<IContentTypeTestUtility, ContentTypeTestUtility>();
            services.AddSingleton<ISerializer, Serializer>();
            services.AddSingleton<IDeserializer, Deserializer>();
            services.AddSingleton((r) => new HttpClientHandler
            {
                AllowAutoRedirect = true,
                AutomaticDecompression = System.Net.DecompressionMethods.Deflate | System.Net.DecompressionMethods.GZip
            });

            services.AddSingleton<IContentDownloaderFactory<DownloadUriResponse>, ContentDownloaderFactory>();

            services.AddSingleton(Configuration.BindTo<StorageConfiguration>(FileStoreNode));

            services.AddSingleton<IStoreBackend<Guid, Upload>, FilesystemStoreBackend>();
            services.AddSingleton<IStoreBackend<Guid, Metadata>, MetadataFsStoreBackend>();
            services.AddSingleton<IFileStreamProvider<Guid, StreamAdapter>, FilesystemStoreBackend>();
            services.AddSingleton<IStore<Guid, Upload, UploadResultRow>, FileSystemStore>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<ServiceFactory>(p => p.GetService);

            services.Scan(scan => scan
                .FromAssembliesOf(typeof(IMediator)
                    , typeof(GenericEvent)
                    , typeof(DownloadUriQuery)
                    , typeof(UploadFilesCommandHandler))
                .AddClasses()
                .AsImplementedInterfaces());

            services.AddCors((s) =>
            {
                s.AddDefaultPolicy((c) =>
                {
                    c.AllowAnyOrigin();
                    c.AllowAnyHeader();
                    c.WithMethods("OPTIONS", "GET", "POST");
                });
            });

            services.AddJwt();
            services.AddJwtAuthenticationEndpointWithFakeService(Configuration);
            services.AddHealthChecks();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseAccessTokenValidator();
            //app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseMvc();

            app.UseCors();

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor
                                   | ForwardedHeaders.XForwardedProto
            });

            app.UseHealthChecks("/health");
        }
    }
}