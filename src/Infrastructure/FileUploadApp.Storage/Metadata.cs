using System;

namespace FileUploadApp.Storage;

public class Metadata
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string ContentType { get; set; }

    public DateTime CreatedDate { get; set; }

    public Metadata() { }

    public Metadata(Guid id, string name, string contentType, DateTime? dateTime = null)
    {
        Id = id;
        Name = name;
        ContentType = contentType;
        CreatedDate = dateTime ?? DateTime.UtcNow;
    }
}
