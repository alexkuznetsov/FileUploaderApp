using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;

using FileUploadApp.Application;
using FileUploadApp.Storage.Filesystem;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

using Serilog;

namespace FileUploadApp;

public class Program
{
    private const string EnvHealthCheckEp = Strings.EnvPrefix + "P_HEALTHCHECK";

    private const string DefaultHealthCheckEndpoint = "/health";


    public static void Main(string[] args)
    {
        var builder = CreateHostBuilder(args);

        builder.Services.AddControllers()
            .AddJsonOptions(o =>
            {
                o.JsonSerializerOptions.PropertyNameCaseInsensitive = false;
                o.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
            });

        builder.Services
            .AddCors((s) => s.AddDefaultPolicy((c) =>
            {
                c.AllowAnyOrigin();
                c.AllowAnyHeader();
                c.WithMethods("OPTIONS", "GET", "POST", "DELETE");
            }))
            .AddHealthChecks();

        builder.Services.AddEndpointsApiExplorer()
            .AddSwaggerGen(c =>
            {
                //FIXME 
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "File Uploader", Version = "v1" });
                c.EnableAnnotations();
            });

        builder.Services.AddApplication(builder.Configuration);
        builder.Services.AddFileStorage(builder.Configuration);

        builder.Services.Configure<RouteOptions>(o => o.LowercaseUrls = true);

        var app = builder.Build();

        if (builder.Environment.IsDevelopment())
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
        app.UseApplication();

        app.MapControllerRoute("Default", "{controller}/{action=index}/{id:int?}");

        app.Run();
    }

    public static WebApplicationBuilder CreateHostBuilder(string[] args)
    {
        var options = new WebApplicationOptions
        {
            ContentRootPath = Directory.GetCurrentDirectory(),
            Args = args,
        };

        var builder = WebApplication.CreateBuilder(options);

        builder.Configuration
            .AddEnvironmentVariables(prefix: Strings.EnvPrefix)
            .AddCommandLine(args);

        builder.Host.UseSerilog((ctx, lc) => lc
                .WriteTo.Console()
                .ReadFrom.Configuration(ctx.Configuration))
            .UseDefaultServiceProvider(options => options.ValidateOnBuild = true);

        builder.WebHost
            .ConfigureKestrel(ConfigureKestrelSettings)
            .UseShutdownTimeout(TimeSpan.FromSeconds(60)) // set timeout value here
            .UseIISIntegration()
            .UseIIS()
        ;
        return builder;

    }

    private static void ConfigureKestrelSettings(KestrelServerOptions options)
    {
        options.AddServerHeader = false;
        var limit = Environment.GetEnvironmentVariable(Strings.EnvUploadLim) ?? Strings.LimitNo;

        if (limit.Equals(Strings.LimitNo))
        {
            options.Limits.MaxRequestBodySize = null;
        }
        else
        {
            if (long.TryParse(limit
                , NumberStyles.Number | NumberStyles.AllowThousands
                , CultureInfo.InvariantCulture
                , out var longLimit))
            {
                options.Limits.MaxRequestBodySize = longLimit;
            }
            else
            {
                var logger = options.ApplicationServices.GetService<ILogger<Program>>();
                var message =
                    $"{Strings.EnvUploadLim} is not equals `{Strings.LimitNo}` or correct long value (current value is {limit}), rolling back to a default value";

                if (logger != null)
                {
                    logger.LogWarning(message);
                }
                else
                {
                    Console.WriteLine(message);
                    Debug.WriteLine(message, "Warning");
                }
            }
        }
    }


}