using System;
using System.Runtime.Serialization;

namespace FileUploadApp.Domain
{
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