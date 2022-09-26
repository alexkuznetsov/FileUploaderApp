using System;

namespace FileUploadApp.Domain;

public class FileEntity : IHaveId<Guid>
{
    private const string StrImageMime = "image/";

    public FileEntity(Guid id, uint number, string name, string contentType)
    {
        Id = id;
        Number = number;
        Name = name;
        ContentType = contentType;
    }

    public Guid Id { get; }

    public uint Number { get; }

    public string Name { get; }

    public string ContentType { get; }

    public bool IsImage() => ContentType.StartsWith(StrImageMime);
}