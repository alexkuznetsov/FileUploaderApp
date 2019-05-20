using System;
using System.Runtime.Serialization;

namespace FileUploadApp.Domain.Dirty
{
    [DataContract]
    public class Base64FilePayload
    {
        private static readonly string DataToken = "data";

        [DataMember(Name = "name", IsRequired = true, Order = 0)]
        public string Name { get; set; }

        [DataMember(Name = "data", IsRequired = true, Order = 1)]
        public string RawData { get; set; }

        public bool IsDataURI()
        {
            return RawData.Substring(0,4).ToLowerInvariant().Equals(DataToken);
        }
    }
}
