namespace FileUploadApp.Domain
{
    public class FormFileDescriptor
    {
        public uint Number { get; }
        public string Name { get; }

        public string ContentType { get; }

        public StreamWrapper Stream { get; }

        public FormFileDescriptor(uint num, string name, string contentType, StreamWrapper stream)
        {
            Number = num;
            Name = name;
            ContentType = contentType;
            Stream = stream;
        }
    }
}