using FileUploadApp.Domain;
using FileUploadApp.Interfaces.Authentication;
using System.Data.Common;
using System.Threading.Tasks;

namespace FileUploadApp.Services.Authentication
{
    public sealed class CheckUserService : ICheckUserService<User>
    {
        private readonly IPasswordHasher _hasher;

        private DbConnection DbContext { get; set; }

        public CheckUserService(DbConnection dBContext, IPasswordHasher hasher)
        {
            DbContext = dBContext;
            _hasher = hasher;
        }

        public async Task<User> FindByNameAsync(string username)
            => await DbContext.FindClientByUserNameAsync(username).ConfigureAwait(false);

        public async Task<bool> AuthenticateAsync(string username, string password)
        {
            var user = await FindByNameAsync(username).ConfigureAwait(false);

            return Authenticate(user, password);
        }

        public bool Authenticate(User user, string password)
        {
            if (user == null)
            {
                throw new System.ArgumentNullException(nameof(user));
            }

            return _hasher.VerifyHashedPassword(user.Passwhash, password);
        }
    }
}
