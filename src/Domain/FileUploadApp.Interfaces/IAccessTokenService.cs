using System.Threading.Tasks;

namespace FileUploadApp.Interfaces;

public interface IAccessTokenService : IAccessTokenService<string>
{
}

public interface IAccessTokenService<in TUserId>
{
    Task<bool> IsTokenAlive();
    Task DeactivateCurrentAsync(TUserId userId);
    Task<bool> IsActiveAsync(string token);
    Task DeactivateAsync(TUserId userId, string token);
}