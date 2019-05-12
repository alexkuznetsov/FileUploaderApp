using System;
using System.IO;

namespace FileUploadApp.Domain
{
    public class ByteArrayFile : IUploadedFile
    {
        public string Name { get; }

        public string ContentType { get; }

        public byte[] Bytea { get; }

        public Stream GetStream() => throw new NotImplementedException();

        public ByteArrayFile(string name, string contentType, byte[] bytea)
        {
            Name = name;
            this.Bytea = bytea;
            ContentType = contentType;
        }
    }
}
