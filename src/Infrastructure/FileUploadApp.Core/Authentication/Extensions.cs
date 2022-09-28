using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;

namespace FileUploadApp.Core.Authentication;

public static class AuthenticationExtensions
{
    public static void AddJwt(this IServiceCollection services)
    {
        if (services == null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        IConfiguration configuration;

        using (var serviceProvider = services.BuildServiceProvider())
        {
            configuration = serviceProvider.GetService<IConfiguration>();
        }

        var section = configuration.GetSection(JwtOptions.SectionName);
        var options = configuration.BindTo<JwtOptions>(JwtOptions.SectionName);

        if (string.IsNullOrEmpty(options.SecretKey))
        {
            throw new ArgumentException(
                "JWT Secret key must be configured in appsettings.json or as env variable");
        }

        services.Configure<JwtOptions>(section);
        services.AddSingleton(options);
        services.AddTransient<IAccessTokenService, AccessTokenService>();
        services.AddTransient<AccessTokenValidatorMiddleware>();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(cfg =>
            {
                cfg.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(options.SecretKey)),
                    ValidIssuer = options.Issuer,
                    ValidAudience = options.ValidAudience,
                    ValidateAudience = options.ValidateAudience,
                    ValidateLifetime = options.ValidateLifetime
                };
            });
    }

    public static IApplicationBuilder UseAccessTokenValidator(this IApplicationBuilder app)
    {
        app.UseMiddleware<AccessTokenValidatorMiddleware>();

        return app;
    }

    public static long ToTimestamp(this DateTime dateTime)
    {
        var centuryBegin = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var expectedDate = dateTime.Subtract(new TimeSpan(centuryBegin.Ticks));

        return expectedDate.Ticks / 10000;
    }
}