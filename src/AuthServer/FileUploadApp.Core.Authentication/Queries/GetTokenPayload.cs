using FileUploadApp.Core.Authentication;
using FileUploadApp.Domain;
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

namespace FileUploadApp.Authentication.Queries;

public class GetTokenPayload
{
    public class Query : IRequest<TokenPayload>
    {
        public Query(string token)
        {
            Token = token;
        }

        public string Token { get; }
    }

    public class Handler : IRequestHandler<Query, TokenPayload>
    {
        private static readonly ISet<string> DefaultClaims = new HashSet<string>
        {
            JwtRegisteredClaimNames.Sub,
            JwtRegisteredClaimNames.UniqueName,
            JwtRegisteredClaimNames.Jti,
            JwtRegisteredClaimNames.Iat,
            ClaimTypes.Role,
        };

        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler = new();
        private readonly TokenValidationParameters _tokenValidationParameters;

        public Handler(JwtOptions options)
        {
            var issuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.SecretKey));
            _tokenValidationParameters = new TokenValidationParameters
            {
                IssuerSigningKey = issuerSigningKey,
                ValidIssuer = options.Issuer,
                ValidAudience = options.ValidAudience,
                ValidateAudience = options.ValidateAudience,
                ValidateLifetime = options.ValidateLifetime
            };
        }

        public Task<TokenPayload> Handle(Query request, CancellationToken cancellationToken)
        {
            _jwtSecurityTokenHandler.ValidateToken(request.Token, _tokenValidationParameters,
                out var validatedSecurityToken);

            if (validatedSecurityToken is not JwtSecurityToken jwt)
            {
                throw new InvalidOperationException($"{nameof(validatedSecurityToken)} is not a JwtSecurityToken. Aborting.");
            }

            return Task.FromResult(new TokenPayload
            {
                Subject = jwt.Subject,
                Role = jwt.Claims.SingleOrDefault(x => x.Type == ClaimTypes.Role)?.Value,
                Expires = jwt.ValidTo.ToTimestamp(),
                Claims = jwt.Claims.Where(x => !DefaultClaims.Contains(x.Type))
                    .ToDictionary(k => k.Type, v => v.Value)
            });
        }
    }

}
