using System.Collections.Generic;
using System.Runtime.Serialization;

namespace FileUploadApp.Domain
{
    [DataContract]
    public class TokenPayload
    {
        [DataMember] public string Subject { get; set; }
        [DataMember] public string Role { get; set; }
        [DataMember] public long Expires { get; set; }
        [DataMember] public IDictionary<string, string> Claims { get; set; }
    }
}