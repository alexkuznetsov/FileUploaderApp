using System;

namespace FileUploadApp.Storage;

public record Metadata(Guid Id, string Name, string ContentType, DateTime CreatedDate)
{
    public Metadata(Guid id, string name, string contentType, DateTime? dateTime = null) : this(id, name, contentType, dateTime ?? DateTime.UtcNow)
    {
    }
}
