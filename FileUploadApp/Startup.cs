using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FileUploadApp.Core.Configuration;
using FileUploadApp.Domain;
using FileUploadApp.Handlers;
using FileUploadApp.Interfaces;
using FileUploadApp.Middlewares;
using FileUploadApp.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

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

            services.AddSingleton(Configuration.BindTo<AppConfiguration>("conf"));
            services.AddSingleton<IContentTypeTestUtility, ContentTypeTestUtility>();

            services.AddScoped<IFileDataPayloadHandler<Base64FilePayload[]>, FileAsBase64DataPayloadHadnler>();
            services.AddScoped<IFileDataPayloadHandler<string[]>, FileAsUriDataPayloadHandler>();

            services.AddSingleton<IMultipathFormPayloadHandler<HttpContext>, FormDataPayloadHandler>();
            services.AddScoped<IFormJsonPayloadHandler<HttpContext>, DataPayloadHandler>();
            services.AddScoped<IPlaintextPayloadHandler<HttpContext>, FileAsPlainTextLinkDataPayloadHandler>();

            services.AddScoped<PayloadTypeProcessorHelper>();

            services.AddSingleton((r) =>
            {
                return new HttpClientHandler
                {
                    AllowAutoRedirect = true,
                    AutomaticDecompression = System.Net.DecompressionMethods.Deflate | System.Net.DecompressionMethods.GZip
                };
            });

            services.AddSingleton<ContentDownloaderFactory>();

            services.AddScoped<IDownloadHelper, DownloadHelper>();
            services.AddScoped<IUploadService, UploadService>();

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
