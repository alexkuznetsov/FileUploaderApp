namespace FileUploadApp.Domain
{
    public class FormFileDescriptor
    {
        public string Name { get; }

        public string ContentType { get; }

        public StreamWrapper Stream { get; }

        public FormFileDescriptor(string name, string contentType, StreamWrapper stream)
        {
            Name = name;
            ContentType = contentType;
            Stream = stream;
        }
    }
}