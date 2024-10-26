using System;

namespace FileUploadApp.Storage;

public class Metadata
{
    public Guid Id { get; set; } = Guid.Empty;
    public string Name { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; } = DateTime.MinValue;

    public Metadata()
    {

    }

    public Metadata(Guid id, string name, string contentType, DateTime? dateTime = null) 
        : this(id, name, contentType, dateTime ?? DateTime.UtcNow)
    {
    }

    public Metadata(Guid id, string name, string contentType, DateTime dateTime)
    {
        Id = id;
        Name = name;
        ContentType = contentType;
        CreatedDate = dateTime;
    }
}
