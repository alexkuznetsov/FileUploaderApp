using FileUploadApp.Application.Common.Authentication;
using FileUploadApp.Application.Common.Mvc;
using FileUploadApp.Application.Services;
using FileUploadApp.Application.Services.Serialization;
using FileUploadApp.Domain;
using FileUploadApp.Domain.Raw;
using FileUploadApp.Interfaces;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FileUploadApp.Application;

public static class DependencyInjection
{
    private const string ConfNode = "conf";

    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        var appConfig = new AppConfiguration();

        configuration.GetSection(ConfNode).Bind(appConfig);

        services.AddSingleton(appConfig);
        services.AddDistributedMemoryCache();
        services.AddOptions();
        services.AddSingleton<ICache, DefaultCacheImpl>();
        services.AddSingleton<IContentTypeTestUtility, ContentTypeTestUtility>();
        services.AddSingleton<ISerializer, Serializer>();
        services.AddSingleton<IDeserializer, Deserializer>();

        services.AddHttpClient<IContentDownloader<DownloadUriResponse>, ContentDownloader>((s, client) =>
        {
            var c = s.GetRequiredService<AppConfiguration>();
            client.DefaultRequestHeaders.Add(ContentDownloader.UserAgentField
                , c.DefaultUserAgent);
        }).AddStandardResilienceHandler();

        services.AddHttpContextAccessor();

        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
            config.RegisterGenericHandlers = false;
        });

        services.AddJwt(configuration);
        services.AddJwtAuthenticationEndpointWithInMemoryService();

        return services;
    }


    public static WebApplication UseApplication(this WebApplication app)
    {
        app.UseMiddleware<AccessTokenValidatorMiddleware>();

        return app;
    }
}
