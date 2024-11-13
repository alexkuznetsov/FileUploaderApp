using System;
using System.Linq;
using System.Threading.Tasks;

using FileUploadApp.Application.Common.Authentication;
using FileUploadApp.Interfaces;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace FileUploadApp.Application.Services;

internal sealed class AccessTokenService(ICache cache,
        IHttpContextAccessor httpContextAccessor,
        IOptions<JwtOptions> jwtOptions) : IAccessTokenService
{
    private const string DeactivatedField = "deactivated";
    private const string AuthorizationField = "authorization";

    public async Task<bool> IsTokenAlive()
        => await IsActiveAsync(GetCurrent()).ConfigureAwait(false);

    public async Task DeactivateCurrentAsync(string userId)
        => await DeactivateAsync(userId, GetCurrent()).ConfigureAwait(false);

    public async Task<bool> IsActiveAsync(string token)
        => string.IsNullOrWhiteSpace(await cache.GetStringAsync(GetKey(token)).ConfigureAwait(false));

    public async Task DeactivateAsync(string userId, string token)
    {
        await cache.SetStringAsync(GetKey(token),
            DeactivatedField, (o) =>
            {
                o.AbsoluteExpirationRelativeToNow =
                        TimeSpan.FromMinutes(jwtOptions.Value.ExpiryMinutes);
            }).ConfigureAwait(false);
    }

    private string GetCurrent()
    {
        var ctx = httpContextAccessor.HttpContext;
        if (ctx == null)
            return string.Empty;

        var authorizationHeader = ctx.Request.Headers[AuthorizationField];

        return authorizationHeader == StringValues.Empty
            ? string.Empty
            : (authorizationHeader.First() ?? "").Split(' ').LastOrDefault() ?? "";
    }

    private static string GetKey(string token)
        => $"tokens:{token}";
}