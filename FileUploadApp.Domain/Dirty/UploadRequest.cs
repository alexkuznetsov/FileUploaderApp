using System.Runtime.Serialization;

namespace FileUploadApp.Domain.Dirty
{
    public class UploadRequest
    {
        [DataMember(Name = "files", IsRequired = false, EmitDefaultValue = false)]
        public Base64FilePayload[] Files { get; set; }

        [DataMember(Name = "links", IsRequired = false, EmitDefaultValue = false)]
        public string[] Links { get; set; }
    }
}