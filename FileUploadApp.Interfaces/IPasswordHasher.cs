namespace FileUploadApp.Services.Accounts
{
    public interface IPasswordHasher
    {
        string HashPassword(string password);
        bool VerifyHashedPassword(string hashedPassword, string password);
    }
}
