using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using FileUploadApp.Application.Common.Authentication;
using FileUploadApp.Domain;

using MediatR;

using Microsoft.IdentityModel.Tokens;

namespace FileUploadApp.Application.Authentication.Commands;

public static class CreateToken
{
    public record Command(string UserId, string? Role = null, IReadOnlyDictionary<string, string>? Claims = null)
        : IRequest<Token>;

    public sealed class Handler(JwtOptions options) : IRequestHandler<Command, Token>
    {
        private readonly SigningCredentials _signingCredentials = new(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.SecretKey))
                , SecurityAlgorithms.HmacSha256);

        public Task<Token> Handle(Command request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.UserId))
                throw new ArgumentException($"User id claim can not be empty.");

            var now = DateTime.UtcNow;
            var jwtClaims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, request.UserId),
                new(JwtRegisteredClaimNames.UniqueName, request.UserId),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Iat, now.ToTimestamp().ToString()),
            };

            if (!string.IsNullOrWhiteSpace(request.Role))
                jwtClaims.Add(new Claim(ClaimTypes.Role, request.Role));

            var customClaims = request.Claims?.Select(claim => new Claim(claim.Key, claim.Value))
                .ToArray() ?? [];

            jwtClaims.AddRange(customClaims);

            var expires = now.AddMinutes(options.ExpiryMinutes);
            var jwt = new JwtSecurityToken(
                issuer: options.Issuer,
                claims: jwtClaims,
                notBefore: now,
                expires: expires,
                signingCredentials: _signingCredentials
            );

            var token = new JwtSecurityTokenHandler().WriteToken(jwt);

            return Task.FromResult(new Token
            {
                AccessToken = token,
                RefreshToken = string.Empty,
                Expires = expires.ToTimestamp(),
                Id = request.UserId,
                Role = string.IsNullOrEmpty(request.Role) ? string.Empty : request.Role,
                Claims = customClaims.ToDictionary(c => c.Type, c => c.Value)
            });
        }
    }
}
