using FileUploadApp.Domain;
using FileUploadApp.Domain.Authentication;
using FileUploadApp.Handlers;
using FileUploadApp.Interfaces.Authentication;
using FileUploadApp.Requests;
using FileUploadApp.Services.Authentication;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Data.Common;
using System.Diagnostics;

namespace FileUploadApp.Core.Authentication
{
    public static class Extensions
    {
        private const string ConfNode = "AuthServer";

        /// <summary>
        /// Register DB Connection factory and connection factory
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddJwtAuthenticationEndpoint(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(configuration.BindTo<AuthConfiguration>(ConfNode));

            services.AddSingleton((r) =>
            {
                var conf = r.GetRequiredService<AuthConfiguration>();

                return DbProviderFactoriesFake.GetFactory(conf.ConnectionString.ProviderName);
            });

            services.AddTransient((r) =>
            {
                var factory = r.GetRequiredService<DbProviderFactory>();
                var connection = factory.CreateConnection();
                var conf = r.GetRequiredService<AuthConfiguration>();

                Debug.Assert(connection != null, nameof(connection) + " != null");
                connection.ConnectionString = conf.ConnectionString.ConnectionString;

                return connection;
            });

            services.AddSingleton<ICheckUserService<User>, CheckUserService>();

            services.Scan(scan => scan
               .FromAssembliesOf(typeof(IMediator)
                   , typeof(CheckUserQueryHandler)
                   , typeof(CheckUserQuery))
               .AddClasses()
               .AsImplementedInterfaces());

            return services;
        }

        public static IServiceCollection AddJwtAuthenticationEndpointWithFakeService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(configuration.BindTo<AuthConfiguration>(ConfNode));

            services.AddSingleton<ICheckUserService<User>, FakeCheckUserService>();

            services.Scan(scan => scan
               .FromAssembliesOf(typeof(IMediator)
                   , typeof(CheckUserQueryHandler)
                   , typeof(CheckUserQuery))
               .AddClasses()
               .AsImplementedInterfaces());


            return services;
        }
    }
}
