using System;
using System.Runtime.Serialization;

namespace FileUploadApp.Storage.Filesystem
{
    [DataContract]
    public class Spec
    {
        [DataMember(Name = "nm", IsRequired = true, EmitDefaultValue = false, Order = 0)]
        public string Name { get; set; }

        [DataMember(Name = "ct", IsRequired = true, EmitDefaultValue = false, Order = 1)]
        public string ContentType { get; set; }

        [DataMember(Name = "crat", IsRequired = true, EmitDefaultValue = false, Order = 2)]
        public DateTime CreatedDate { get; set; }

        [DataMember(Name = "pth", IsRequired = true, EmitDefaultValue = false, Order = 3)]
        public string Path { get; set; }

        [DataMember(Name = "wdt", IsRequired = true, EmitDefaultValue = false, Order = 4)]
        public uint Width { get; set; }

        [DataMember(Name = "hgt", IsRequired = true, EmitDefaultValue = false, Order = 4)]
        public uint Height { get; set; }

        public Spec() { }

        public Spec(string name, string cntentType, string path, uint width, uint height, DateTime? dateTime = null)
        {
            Name = name;
            ContentType = cntentType;
            CreatedDate = dateTime ?? DateTime.UtcNow;
            Path = path;
            Height = height;
            Width = width;
        }
    }
}
