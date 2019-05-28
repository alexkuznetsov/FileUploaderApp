using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace FileUploadApp.Domain.Dirty
{
    [DataContract]
    public class Base64FilePayload
    {
        public static readonly string DataToken = "data";

        [DataMember(Name = "name", IsRequired = true, Order = 0)]
        [Required]
        [MinLength(3)]
        public string Name { get; set; }

        [DataMember(Name = "data", IsRequired = true, Order = 1)]
        [MinLength(3)]
        public string RawData { get; set; }
    }
}
