using System;
using System.IO;

namespace FileUploadApp.Domain;

public class FileDescriptor : FileEntity
{
    public Stream Stream { get; }

    protected FileDescriptor(Guid id, uint number, string name, string contentType, Stream accociatedStream) :
        base(id, number, name, contentType)
    {
        Stream = accociatedStream;
    }
}