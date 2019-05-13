using System.Collections.Generic;
using System.Runtime.Serialization;

namespace FileUploadApp.Domain
{
    [DataContract]
    public class UploadRequest
    {
        [DataMember(Name = "files", IsRequired = false, EmitDefaultValue = false)]
        public List<Base64FilePayload> Files { get; set; }

        [DataMember(Name = "links", IsRequired = false, EmitDefaultValue = false)]
        public List<string> Links { get; set; }
    }
}
