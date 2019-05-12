using System.Collections.Generic;
using System.Runtime.Serialization;

namespace FileUploadApp.Domain
{
    [DataContract]
    public class Base64Payload
    {
        [DataMember(Name = "files", IsRequired = false, EmitDefaultValue = false)]
        public List<FileAsBase64Payload> Files { get; set; }

        [DataMember(Name = "links", IsRequired = false, EmitDefaultValue = false)]
        public List<string> Links { get; set; }
    }
}
