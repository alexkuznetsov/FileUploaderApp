using FileUploadApp.Domain;
using MediatR;

namespace FileUploadApp.Requests
{
    public class CheckUserQuery : IRequest<CheckUserResult>
    {
        public CheckUserQuery(string username, string password)
        {
            Username = username;
            Password = password;
        }

        public string Username { get; }
        public string Password { get; }
    }
}
