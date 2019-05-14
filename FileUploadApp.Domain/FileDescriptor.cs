namespace FileUploadApp.Domain
{
    public class FileDescriptor
    {
        public uint Number { get; }

        public string Name { get; }

        public string ContentType { get; }

        public StreamAdapter Stream { get; }

        public FileDescriptor(uint num, string name, string contentType, StreamAdapter stream)
        {
            Number = num;
            Name = name;
            ContentType = contentType;
            Stream = stream;
        }
    }
}