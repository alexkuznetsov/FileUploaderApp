using System.Threading.Tasks;

namespace FileUploadApp.Core.Authentication;

internal interface IAccessTokenService : IAccessTokenService<string>
{
}

internal interface IAccessTokenService<in TUserId>
{
    Task<bool> IsTokenAlive();
    Task DeactivateCurrentAsync(TUserId userId);
    Task<bool> IsActiveAsync(string token);
    Task DeactivateAsync(TUserId userId, string token);
}