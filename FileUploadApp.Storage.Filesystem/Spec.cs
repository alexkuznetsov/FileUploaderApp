using System;
using System.Runtime.Serialization;

namespace FileUploadApp.Storage.Filesystem
{
    [DataContract]
    public class Spec
    {
        [DataMember(Name = "id", IsRequired = true, EmitDefaultValue = false, Order = 3)]
        public Guid Id { get; set; }

        [DataMember(Name = "nm", IsRequired = true, EmitDefaultValue = false, Order = 0)]
        public string Name { get; set; }

        [DataMember(Name = "ct", IsRequired = true, EmitDefaultValue = false, Order = 1)]
        public string ContentType { get; set; }

        [DataMember(Name = "crat", IsRequired = true, EmitDefaultValue = false, Order = 2)]
        public DateTime CreatedDate { get; set; }

        [DataMember(Name = "wdt", IsRequired = true, EmitDefaultValue = false, Order = 4)]
        public uint Width { get; set; }

        [DataMember(Name = "hgt", IsRequired = true, EmitDefaultValue = false, Order = 4)]
        public uint Height { get; set; }

        public Spec() { }

        public Spec(Guid id, string name, string contentType, uint width, uint height, DateTime? dateTime = null)
        {
            Id = id;
            Name = name;
            ContentType = contentType;
            Width = width;
            Height = height;
            CreatedDate = dateTime ?? DateTime.UtcNow;
        }
    }
}
