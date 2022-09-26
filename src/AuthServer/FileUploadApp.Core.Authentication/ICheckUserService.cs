using System.Threading.Tasks;

namespace FileUploadApp.Authentication;

public interface ICheckUserService<TUser>
{
    bool Authenticate(TUser user, string password);
    Task<bool> AuthenticateAsync(string username, string password);
    Task<TUser> FindByNameAsync(string username);
}