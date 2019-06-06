using System.Collections.Generic;
using System.Runtime.Serialization;

namespace FileUploadApp.Domain
{
    [DataContract]
    public class Token
    {
        [DataMember] public string AccessToken { get; set; }
        [DataMember] public string RefreshToken { get; set; }
        [DataMember] public long Expires { get; set; }
        [DataMember] public string Id { get; set; }
        [DataMember] public string Role { get; set; }
        [DataMember] public IDictionary<string, string> Claims { get; set; }
    }
}