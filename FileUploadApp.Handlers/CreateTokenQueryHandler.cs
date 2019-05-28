using FileUploadApp.Core.Authentication;
using FileUploadApp.Domain;
using FileUploadApp.Requests;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FileUploadApp.Handlers
{
    public class CreateTokenQueryHandler : IRequestHandler<CreateTokenQuery, Token>
    {
        private readonly JwtOptions options;
        private readonly SigningCredentials signingCredentials;

        public CreateTokenQueryHandler(JwtOptions options)
        {
            this.options = options;
            signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.SecretKey))
                , SecurityAlgorithms.HmacSha256);
        }

        public Task<Token> Handle(CreateTokenQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.UserId))
            {
                throw new ArgumentException("User id claim can not be empty.", nameof(request.UserId));
            }

            var now = DateTime.UtcNow;
            var jwtClaims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, request.UserId),
                new Claim(JwtRegisteredClaimNames.UniqueName, request.UserId),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, now.ToTimestamp().ToString()),
            };

            if (!string.IsNullOrWhiteSpace(request.Role))
            {
                jwtClaims.Add(new Claim(ClaimTypes.Role, request.Role));
            }

            var customClaims = request.Claims?.Select(claim => new Claim(claim.Key, claim.Value)).ToArray()
                               ?? Array.Empty<Claim>();

            jwtClaims.AddRange(customClaims);

            var expires = now.AddMinutes(options.ExpiryMinutes);
            var jwt = new JwtSecurityToken(
                issuer: options.Issuer,
                claims: jwtClaims,
                notBefore: now,
                expires: expires,
                signingCredentials: signingCredentials
            );

            var token = new JwtSecurityTokenHandler().WriteToken(jwt);

            return Task.FromResult(new Token
            {
                AccessToken = token,
                RefreshToken = string.Empty,
                Expires = expires.ToTimestamp(),
                Id = request.UserId,
                Role = (string.IsNullOrEmpty(request.Role) ? string.Empty : request.Role),
                Claims = customClaims.ToDictionary(c => c.Type, c => c.Value)
            });
        }
    }
}
