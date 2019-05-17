using System;

namespace FileUploadApp.Domain
{
    public class Upload : FileDescriptor
    {
        public static readonly string PreviewPrefix = "p_";

        public uint Width { get; private set; }

        public uint Height { get; private set; }

        public Guid PreviewId { get; }

        public Upload(
            uint num,
            string name,
            string contentType,
            uint width,
            uint height,
            StreamAdapter streamAdapter) : this(Guid.NewGuid(), Guid.NewGuid(), num, name, contentType, width, height, streamAdapter)
        {

        }

        public Upload(
           Guid id,
           Guid previewId,
           uint num,
           string name,
           string contentType,
           uint width,
           uint height,
           StreamAdapter streamAdapter) : base(id, num, name, contentType, streamAdapter)
        {
            PreviewId = previewId;
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
