namespace FileUploadApp.Domain.Authentication
{
    public class CheckUserResult
    {
        public static readonly CheckUserResult NotFound = new CheckUserResult(null);

        public static readonly CheckUserResult WrongPassword = new CheckUserResult(null);

        public static CheckUserResult Ok(User user) => new CheckUserResult(user);

        private CheckUserResult(User user)
        {
            User = user;
        }

        public User User { get; }

        public bool UserNotFound() => ReferenceEquals(this, NotFound);
        public bool UserPasswordMismatch() => ReferenceEquals(this, WrongPassword);
    }
}