namespace FileUploadApp.Domain
{
    public class UploadedFile
    {
        public string Name { get; }

        public string ContentType { get; }

        public uint Width { get; private set; }

        public uint Height { get; private set; }

        public StreamWrapper StreamWrapper { get; }

        public UploadedFile(
            string name,
            string contentType,
            uint width,
            uint height,
            StreamWrapper streamWrapper)
        {
            Name = name;
            ContentType = contentType;
            Width = width;
            Height = height;
            StreamWrapper = streamWrapper;
        }

        public void SetSize(uint height, uint width)
        {
            Height = height;
            Width = width;
        }
    }
}
