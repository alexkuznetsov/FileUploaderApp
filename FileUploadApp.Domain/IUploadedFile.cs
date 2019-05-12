using System.IO;

namespace FileUploadApp.Domain
{
    public interface IUploadedFile
    {
        string Name { get; }

        string ContentType { get; }

        Stream GetStream();
    }
}
