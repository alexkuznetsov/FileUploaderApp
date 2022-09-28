using FileUploadApp.Authentication.Services;
using FileUploadApp.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Data.Common;
using System.Diagnostics;

namespace FileUploadApp.Authentication;

public static class Extensions
{
    /// <summary>
    /// Register DB Connection factory and connection factory
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddJwtAuthenticationEndpoint(
        this IServiceCollection services
        , IConfiguration configuration)
    {
        var conf = new AuthConfiguration();
        configuration.GetSection(AuthConfiguration.SectionKey).Bind(conf);

        services.AddSingleton(conf);

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

        return services;
    }

    public static IServiceCollection AddJwtAuthenticationEndpointWithInMemoryService(
        this IServiceCollection services
        , IConfiguration configuration
        , Action<InMemoryCheckUserServiceOptions> configure)
    {
        var conf = new AuthConfiguration();
        configuration.GetSection(AuthConfiguration.SectionKey).Bind(conf);

        services.AddSingleton(conf);
        services.AddOptions<InMemoryCheckUserServiceOptions>();

        services.AddSingleton<ICheckUserService<User>, InMemoryCheckUserService>();
        services.Configure<InMemoryCheckUserServiceOptions>(configure);

        return services;
    }
}
