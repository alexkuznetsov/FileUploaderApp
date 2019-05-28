using System;
using System.Runtime.Serialization;

namespace FileUploadApp.Domain
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

    [DataContract]
    public class User : IHaveId<int>
    {
        [DataMember] public int Id { get; set; }

        [DataMember] public DateTime CreatedAt { get; set; }

        [DataMember] public DateTime? UpdatedAt { get; set; }

        [DataMember] public string Username { get; set; }

        // ReSharper disable once IdentifierTypo
        [DataMember] public string Passwhash { get; set; }
    }
}