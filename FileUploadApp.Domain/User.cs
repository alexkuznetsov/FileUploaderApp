using System;

namespace FileUploadApp.Domain
{
    public class CheckUserResult
    {
        public static readonly CheckUserResult NotFound = new CheckUserResult(null, false);

        public static readonly CheckUserResult WrongPassw = new CheckUserResult(null, false);

        public static CheckUserResult Ok(User user) => new CheckUserResult(user, true);

        public CheckUserResult(User user, bool status)
        {
            User = user;
            Status = status;
        }

        public User User { get; }
        public bool Status { get; }

        public bool UserNotFound() => ReferenceEquals(this, NotFound);
        public bool UserPasswordMismatch() => ReferenceEquals(this, WrongPassw);
    }
    public class User : IHaveId<int>
    {
        public int Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public string Username { get; set; }

        public string Passwhash { get; set; }
    }
}
