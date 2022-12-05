using FileUploadApp.Domain;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace FileUploadApp.StreamAdapters;

[Obsolete("Избавиться от данного класса")]
public class ByteaStreamAdapter : StreamAdapter
{
    private readonly ReadOnlyMemory<byte> _byteArray;

    public ByteaStreamAdapter(ReadOnlyMemory<byte> byteArray)
        => _byteArray = byteArray;

    public override Stream Stream => throw new NotImplementedException();

    public override async Task CopyToAsync(Stream target, CancellationToken cancellationToken = default)
        => await target.WriteAsync(_byteArray, cancellationToken).ConfigureAwait(false);
}
