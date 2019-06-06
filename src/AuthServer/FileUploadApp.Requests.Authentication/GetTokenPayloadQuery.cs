using FileUploadApp.Domain;
using MediatR;

namespace FileUploadApp.Requests
{
    public class GetTokenPayloadQuery : IRequest<TokenPayload>
    {
        public GetTokenPayloadQuery(string token)
        {
            Token = token;
        }

        public string Token { get; }
    }
}
