namespace FileUploadApp.Domain
{
    public class Upload : FileDescriptor
    {
        public uint Width { get; private set; }

        public uint Height { get; private set; }

        public Upload(
            uint num,
            string name,
            string contentType,
            uint width,
            uint height,
            StreamAdapter streamAdapter) : base(num, name, contentType, streamAdapter)
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
