﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;

namespace FileUploadApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var appBuilder = CreateHostBuilder(args)
                 ;

            var startup = new Startup(appBuilder.Configuration, appBuilder.Environment);

            startup.ConfigureServices(appBuilder.Services);

            var app = appBuilder.Build();

            startup.Configure(app);

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
                options.Limits.MaxRequestBodySize = null;
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
}