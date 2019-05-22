using System;

namespace FileUploadApp.Domain
{
    public class Upload : FileDescriptor
    {
        public static readonly string PreviewPrefix = "preview_";

        public static readonly Upload Empty = new Upload(Guid.Empty, Guid.Empty, 0U, string.Empty, string.Empty, null);

        public Guid PreviewId { get; }

        public Upload(
           Guid id,
           Guid previewId,
           uint num,
           string name,
           string contentType,
           StreamAdapter streamAdapter) : base(id, num, name, contentType, streamAdapter)
        {
            PreviewId = previewId;
        }
    }
}
