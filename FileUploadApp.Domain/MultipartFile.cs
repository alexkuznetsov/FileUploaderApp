using System.IO;

namespace FileUploadApp.Domain
{
    public class MultipartFile : IUploadedFile
    {
        private readonly IStreamWrapper _dataStreamWrapper;

        public string Name { get; }
        public string ContentType { get; }

        public Stream GetStream() => _dataStreamWrapper.GetStream();

        public MultipartFile(string fileName, string contentType, IStreamWrapper dataStreamWrapper)
        {
            Name = fileName;
            ContentType = contentType;
            _dataStreamWrapper = dataStreamWrapper;
        }
    }
}
