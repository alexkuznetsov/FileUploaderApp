using System;
using System.Diagnostics;
using System.Globalization;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Microsoft.Extensions.DependencyInjection;

namespace FileUploadApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((s) => s.AddEnvironmentVariables(prefix: Strings.EnvPrefix))
                .UseKestrel(o =>
                {
                    var limit = Environment.GetEnvironmentVariable(Strings.EnvUploadLim) ?? Strings.LimitNo;

                    if (limit.Equals(Strings.LimitNo))
                        o.Limits.MaxRequestBodySize = null;
                    else
                    {
                        if (long.TryParse(limit
                            , NumberStyles.Number | NumberStyles.AllowThousands
                            , CultureInfo.InvariantCulture
                            , out var longLimit))
                        {
                            o.Limits.MaxRequestBodySize = longLimit;
                        }
                        else
                        {
                            var logger = o.ApplicationServices.GetService<ILogger<Program>>();
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
                })
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    logging.AddConsole();
                    logging.AddDebug();
                    logging.AddSerilog();
                })
                .UseSerilog((builder, logConfig) =>
                {
                    logConfig.ReadFrom.Configuration(builder.Configuration)
                        .Enrich
                        .FromLogContext()
                        .MinimumLevel.Information();
                })
                .UseShutdownTimeout(TimeSpan.FromSeconds(60)) // set timeout value here
                .UseStartup<Startup>();
    }
}