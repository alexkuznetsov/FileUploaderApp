using System.Runtime.Serialization;

namespace FileUploadApp.Domain
{
    [DataContract]
    public class Base64FilePayload
    {
        [DataMember(Name = "name", IsRequired = true, Order = 0)]
        public string Name { get; set; }

        [DataMember(Name = "base64", IsRequired = true, Order = 1)]
        public string Base64 { get; set; }
    }
}
