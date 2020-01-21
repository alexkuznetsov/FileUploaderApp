using FileUploadApp.Domain;
using FileUploadApp.Interfaces.Authentication;
using System;
using System.Threading.Tasks;

namespace FileUploadApp.Services.Authentication
{
    public sealed class FakeCheckUserService : ICheckUserService<User>
    {
        public bool Authenticate(User user, string password)
            => user.Passwhash == password;

        public async Task<bool> AuthenticateAsync(string username, string password)
        {
            var user = await FindByNameAsync(username).ConfigureAwait(false);

            return Authenticate(user, password);
        }

        public Task<User> FindByNameAsync(string username)
        {
            if (username.Equals("rex"))
                return Task.FromResult(new User
                {
                    CreatedAt = DateTime.Now,
                    Id = 1,
                    Passwhash = "1qaz!QAZ",
                    UpdatedAt = null,
                    Username = username
                });

            return Task.FromResult<User>(null);
        }
    }
}
