using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FileUploadApp.Core.Authentication
{
    internal class AccessTokenService : IAccessTokenService
    {
        private const string DeactivatedField = "deactivated";
        private const string AuthorizationField = "authorization";

        private readonly IDistributedCache _cache;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IOptions<JwtOptions> _jwtOptions;

        public AccessTokenService(IDistributedCache cache,
            IHttpContextAccessor httpContextAccessor,
            IOptions<JwtOptions> jwtOptions)
        {
            _cache = cache;
            _httpContextAccessor = httpContextAccessor;
            _jwtOptions = jwtOptions;
        }

        public async Task<bool> IsTokenAlive()
            => await IsActiveAsync(GetCurrent()).ConfigureAwait(false);

        public async Task DeactivateCurrentAsync(string userId)
            => await DeactivateAsync(userId, GetCurrent()).ConfigureAwait(false);

        public async Task<bool> IsActiveAsync(string token)
            => string.IsNullOrWhiteSpace(await _cache.GetStringAsync(GetKey(token)).ConfigureAwait(false));

        public async Task DeactivateAsync(string userId, string token)
        {
            await _cache.SetStringAsync(GetKey(token),
                DeactivatedField, new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow =
                        TimeSpan.FromMinutes(_jwtOptions.Value.ExpiryMinutes)
                }).ConfigureAwait(false);
        }

        private string GetCurrent()
        {
            var authorizationHeader = _httpContextAccessor
                .HttpContext.Request.Headers[AuthorizationField];

            return authorizationHeader == StringValues.Empty
                ? string.Empty
                : authorizationHeader[0].Split(' ').Last();
        }

        private static string GetKey(string token)
            => $"tokens:{token}";
    }
}