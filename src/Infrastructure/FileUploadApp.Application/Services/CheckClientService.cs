using System.Data.Common;
using System.Threading.Tasks;

using FileUploadApp.Domain;
using FileUploadApp.Interfaces;

namespace FileUploadApp.Application.Services;

internal sealed class CheckUserService : ICheckUserService<User>
{
    private readonly IPasswordHasher _hasher;

    private DbConnection DbContext { get; set; }

    public CheckUserService(DbConnection dBContext, IPasswordHasher hasher)
    {
        DbContext = dBContext;
        _hasher = hasher;
    }

    public Task<User?> FindByNameAsync(string username)
        => DbContext.FindClientByUserNameAsync(username);

    public async Task<bool> AuthenticateAsync(string username, string password)
    {
        var user = await FindByNameAsync(username).ConfigureAwait(false);

        return user != null && Authenticate(user, password);
    }

    public bool Authenticate(User? user, string password)
    {
        return _hasher.VerifyHashedPassword(user?.Passwhash ?? "", password);
    }
}
