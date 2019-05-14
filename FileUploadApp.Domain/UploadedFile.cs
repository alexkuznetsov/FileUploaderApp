namespace FileUploadApp.Domain
{
    public class UploadedFile : FormFileDescriptor
    {
        public uint Width { get; private set; }

        public uint Height { get; private set; }

        public UploadedFile(
            string name,
            string contentType,
            uint width,
            uint height,
            StreamWrapper streamWrapper) : base(name, contentType, streamWrapper)
        {
            Width = width;
            Height = height;
        }

        public void SetSize(uint height, uint width)
        {
            Height = height;
            Width = width;
        }
    }
}
