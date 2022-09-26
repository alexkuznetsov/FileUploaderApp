using System;

namespace FileUploadApp.Domain;

public class Upload : FileDescriptor
{
    public const string PreviewPrefix = "preview_";
    
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
