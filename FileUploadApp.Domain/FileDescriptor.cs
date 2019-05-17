using System;

namespace FileUploadApp.Domain
{
    public class FileDescriptor
    {
        public Guid Id { get; }

        public uint Number { get; }

        public string Name { get; }

        public string ContentType { get; }

        public StreamAdapter Stream { get; }

        public FileDescriptor(uint num, string name, string contentType, StreamAdapter stream)
            : this(Guid.NewGuid(), num, name, contentType, stream)
        {
        }

        public FileDescriptor(Guid id, uint number, string name, string contentType, StreamAdapter streamAdapter)
        {
            Id = id;
            Number = number;
            Name = name;
            ContentType = contentType;
            Stream = streamAdapter;
        }
    }
}