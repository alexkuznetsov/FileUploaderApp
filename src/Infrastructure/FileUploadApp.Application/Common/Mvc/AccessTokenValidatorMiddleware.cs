using System.Net;
using System.Threading.Tasks;

using FileUploadApp.Interfaces;

using Microsoft.AspNetCore.Http;

namespace FileUploadApp.Application.Common.Mvc;

internal class AccessTokenValidatorMiddleware : IMiddleware
{
    private readonly IAccessTokenService _accessTokenService;

    public AccessTokenValidatorMiddleware(IAccessTokenService accessTokenService)
    {
        _accessTokenService = accessTokenService;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (await _accessTokenService.IsTokenAlive())
        {
            await next(context);

            return;
        }

        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
    }
}
