using FileUploadApp.Authentication;
using FileUploadApp.Authentication.Queries;
using FileUploadApp.Authentication.Services;
using FileUploadApp.Core.Authentication;
using FileUploadApp.Core.Serialization;
using FileUploadApp.Domain;
using FileUploadApp.Domain.Raw;
using FileUploadApp.Features.Commands;
using FileUploadApp.Features.Services;
using FileUploadApp.Interfaces;
using FileUploadApp.Storage;
using FileUploadApp.Storage.Filesystem;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Net.Http;

namespace FileUploadApp
{
    public class Startup
    {
        private const string ConfNode = "conf";
        private const string FileStoreNode = "fileStore";

        private const string EnvHealthCheckEp = Strings.EnvPrefix + "P_HEALTHCHECK";

        private const string DefaultHealthCheckEndpoint = "/health";
        private readonly IWebHostEnvironment env;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            this.env = env;
        }

        private IConfiguration Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureMvc(services);
            ConfigureOptions(services);
            ConfigureDependencies(services);
        }

        private void ConfigureDependencies(IServiceCollection services)
        {
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

            services.AddMediatR(new[]
            {
                  typeof(UploadFiles.Handler).Assembly
                , typeof(CheckUser.Handler).Assembly
            });
        }

        private void ConfigureOptions(IServiceCollection services)
        {
            services.Configure<RouteOptions>(o =>
            {
                o.LowercaseUrls = true;
            });
        }

        private void ConfigureMvc(IServiceCollection services)
        {
            services.AddMvc()
                .AddJsonOptions(o =>
                {
                    o.JsonSerializerOptions.PropertyNameCaseInsensitive = false;
                    o.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
                });

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
            services.AddJwtAuthenticationEndpointWithInMemoryService(Configuration, (o) =>
            {
                //o
                //    .WithUser("rex", "1qaz!QAZ")
                //    .WithUser("admin", "admin")
                //;
                Configuration.GetSection(InMemoryCheckUserServiceOptions.SectionKey).Bind(o);
            });
            services.AddHealthChecks();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.ResolveConflictingActions(apiDescriptions =>
                {

                    return apiDescriptions.First();
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            RegisterMiddleware(app);
        }

        private void RegisterMiddleware(IApplicationBuilder app)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseAccessTokenValidator();
#if ONLY_HTTPS
            app.UseHttpsRedirection();
#endif

            app.UseCors();

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor
                                   | ForwardedHeaders.XForwardedProto
            });

            app.UseHealthChecks(Environment.GetEnvironmentVariable(EnvHealthCheckEp) ?? DefaultHealthCheckEndpoint);

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("Default", "{controller}/{action=index}/{id:int?}");
            });
        }
    }
}