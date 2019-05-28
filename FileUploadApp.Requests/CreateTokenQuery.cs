using FileUploadApp.Domain;
using MediatR;
using System.Collections.Generic;

namespace FileUploadApp.Requests
{
    public class CreateTokenQuery : IRequest<Token>
    {
        public CreateTokenQuery(string userId, string role = null, IReadOnlyDictionary<string, string> claims = null)
        {
            UserId = userId;
            Role = role;
            Claims = claims;
        }

        public string UserId { get; }
        public string Role { get; }
        public IReadOnlyDictionary<string, string> Claims { get; }
    }
}
