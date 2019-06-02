using System.Runtime.Serialization;

namespace FileUploadApp.Storage.Filesystem
{
    [DataContract]
    public class StorageConfiguration
    {
        [DataMember] public string BasePath { get; set; } = "./uploads";
    }
}