using System.Runtime.Serialization;

namespace FileUploadApp.Core.Authentication
{
    [DataContract]
    public class JwtOptions
    {
        [DataMember] public string SecretKey { get; set; }
        [DataMember] public string Issuer { get; set; }
        [DataMember] public int ExpiryMinutes { get; set; }
        [DataMember] public bool ValidateLifetime { get; set; }
        [DataMember] public bool ValidateAudience { get; set; }
        [DataMember] public string ValidAudience { get; set; }
    }
}