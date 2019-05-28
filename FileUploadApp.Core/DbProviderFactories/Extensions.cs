using FileUploadApp.Domain;
using Microsoft.Extensions.DependencyInjection;
using System.Data.Common;
using System.Diagnostics;

namespace FileUploadApp.Core.DbProviderFactories
{
    public static class Extensions
    {
        /// <summary>
        /// Register DB Connection factory and connection factory
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddDbConnection(this IServiceCollection services)
        {
            services.AddSingleton((r) =>
            {
                var conf = r.GetRequiredService<AppConfiguration>();

                return DbProviderFactoriesFake.GetFactory(conf.ConnectionString.ProviderName);
            });

            services.AddTransient((r) =>
            {
                var factory = r.GetRequiredService<DbProviderFactory>();
                var connection = factory.CreateConnection();
                var conf = r.GetRequiredService<AppConfiguration>();

                Debug.Assert(connection != null, nameof(connection) + " != null");
                connection.ConnectionString = conf.ConnectionString.ConnectionString;

                return connection;
            });

            return services;
        }
    }
}
