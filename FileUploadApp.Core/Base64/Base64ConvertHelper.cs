using ProtoBuf;
using System;
using System.Buffers.Text;

namespace FileUploadApp.Core
{
    internal static class Base64ConvertHelper
    {
        public static byte[] ConvertToBytes(ReadOnlySpan<char> sliced)
        {
            byte[] bytea;
            var bytes = BufferPool.GetBuffer(Base64.GetMaxDecodedFromUtf8Length(sliced.Length));

            try
            {
                if (Convert.TryFromBase64Chars(sliced, bytes, out var bytesWritten))
                {
                    bytea = new byte[bytesWritten];
                    Buffer.BlockCopy(bytes, 0, bytea, 0, bytesWritten);
                }
                else
                {
                    var chars = sliced.ToArray();
                    bytea = Convert.FromBase64CharArray(chars, 0, chars.Length);
                }
            }
            finally
            {
                BufferPool.ReleaseBufferToPool(ref bytes);
            }

            return bytea;
        }
    }
}
