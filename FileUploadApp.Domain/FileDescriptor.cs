using System;

namespace FileUploadApp.Domain
{
    public class FileDescriptor : FileEntity
    {
        public StreamAdapter Stream { get; }

        public FileDescriptor(Guid id, uint number, string name, string contentType, StreamAdapter streamAdapter) : base(id, number, name, contentType)
        {
            Stream = streamAdapter;
        }
    }
}