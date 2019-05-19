using System;

namespace FileUploadApp.Domain
{
    public class Upload : FileDescriptor
    {
        public static readonly string PreviewPrefix = "preview_";

        public Guid PreviewId { get; }

        [Obsolete("Требуется избавиться от данного конструктора и перейти на полную версию")]
        public Upload(
            uint num,
            string name,
            string contentType,
            StreamAdapter streamAdapter) : this(Guid.NewGuid(), Guid.NewGuid(), num, name, contentType, streamAdapter)
        {

        }

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
