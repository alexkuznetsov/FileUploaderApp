using System;
using System.Diagnostics;
using System.Globalization;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

namespace FileUploadApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var appBuilder = CreateHostBuilder(args);

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
            ;
            return builder;

        }

        //public static IWebHostBuilder CreateHostBuilder(string[] args) =>
        //   new WebHostBuilder()

        //        .ConfigureLogging((hostingContext, logging) =>
        //        {
        //            logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
        //            logging.AddConsole();
        //            logging.AddDebug();
        //            logging.AddSerilog();
        //        })
        //         .UseSerilog((builder, logConfig) =>
        //         {
        //             logConfig.ReadFrom.Configuration(builder.Configuration)
        //                 .Enrich
        //                 .FromLogContext()
        //                 .MinimumLevel.Information();
        //         })
        //        .UseShutdownTimeout(TimeSpan.FromSeconds(60)) // set timeout value here
        //      .UseContentRoot(Directory.GetCurrentDirectory())
        //      .UseStartup<Startup>()
        //      //.UseIISIntegration()
        //      //.UseIIS()
        //  ;

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