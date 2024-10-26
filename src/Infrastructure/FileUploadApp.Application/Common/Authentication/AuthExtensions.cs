using System;
using System.Data.Common;
using System.Diagnostics;

using FileUploadApp.Application.Common.Mvc;
using FileUploadApp.Application.Services;
using FileUploadApp.Domain;
using FileUploadApp.Interfaces;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace FileUploadApp.Application.Common.Authentication;

internal static class Extensions
{
    /// <summary>
    /// Register DB Connection factory and connection factory
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    internal static IServiceCollection AddJwtAuthenticationEndpoint(
        this IServiceCollection services
        , IConfiguration configuration)
    {
        services.AddSingleton((r) =>
        {
            var conf = r.GetRequiredService<IOptions<AuthConfiguration>>().Value;

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

    internal static IServiceCollection AddJwtAuthenticationEndpointWithInMemoryService(
        this IServiceCollection services)
    {
        services.ConfigureOptions<AuthConfigurationOptionsSetup>();
        services.ConfigureOptions<InMemoryCheckUserServiceOptionsSetup>();

        services.AddSingleton<ICheckUserService<User>, InMemoryCheckUserService>();

        return services;
    }


    internal static void AddJwt(this IServiceCollection services, IConfiguration configuration)
    {
        var section = configuration.GetSection(JwtOptions.SectionName);
        var jwtOptions = new JwtOptions();

        configuration.GetSection(JwtOptions.SectionName).Bind(jwtOptions);

        if (string.IsNullOrEmpty(jwtOptions.SecretKey))
        {
            throw new ArgumentException(
                "JWT Secret key must be configured in appsettings.json or as env variable");
        }

        services.Configure<JwtOptions>(section);
        services.AddSingleton(jwtOptions);
        services.AddTransient<IAccessTokenService, AccessTokenService>();
        services.AddTransient<AccessTokenValidatorMiddleware>();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(cfg => cfg.TokenValidationParameters = new TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtOptions.SecretKey)),
                ValidIssuer = jwtOptions.Issuer,
                ValidAudience = jwtOptions.ValidAudience,
                ValidateAudience = jwtOptions.ValidateAudience,
                ValidateLifetime = jwtOptions.ValidateLifetime
            });
    }

    public static long ToTimestamp(this DateTime dateTime)
    {
        var centuryBegin = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var expectedDate = dateTime.Subtract(new TimeSpan(centuryBegin.Ticks));

        return expectedDate.Ticks / 10000;
    }
}
