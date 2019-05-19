using System;

namespace FileUploadApp.Domain
{
    public class FileDescriptor : FileEntity
    {
        public StreamAdapter Stream { get; }

        [Obsolete("Требуется избавиться от данного конструктора")]
        public FileDescriptor(uint num, string name, string contentType, StreamAdapter stream)
            : this(Guid.NewGuid(), num, name, contentType, stream)
        {
        }

        public FileDescriptor(Guid id, uint number, string name, string contentType, StreamAdapter streamAdapter) : base(id, number, name, contentType)
        {
            Stream = streamAdapter;
        }
    }
}