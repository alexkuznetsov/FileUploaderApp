using FileUploadApp.Domain;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace FileUploadApp.Core.Streams
{
    public class ByteaStreamWrapper : StreamWrapper
    {
        private ReadOnlyMemory<byte> _bytea;

        public ByteaStreamWrapper(ReadOnlyMemory<byte> bytea)
        {
            _bytea = bytea;
        }

        public override Task<byte[]> AsRawBytesAsync()
        {
            return Task.FromResult(_bytea.ToArray());
        }

        public override Task CopyToAsync(Stream target, CancellationToken cancellationToken = default)
        {
            return target.WriteAsync(_bytea, cancellationToken)
                .AsTask();
        }

        //public override Stream GetStream()
        //{
        //    return new MemoryStream()
        //}
    }
}
