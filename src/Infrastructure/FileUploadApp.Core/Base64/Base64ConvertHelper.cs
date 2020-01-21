using ProtoBuf;
using System;
using System.Buffers.Text;

namespace FileUploadApp.Core
{
    internal static class Base64ConvertHelper
    {
        public static byte[] ConvertToBytes(ReadOnlySpan<char> sliced)
        {
            byte[] byteArr;
            var bytes = BufferPool.GetBuffer(Base64.GetMaxDecodedFromUtf8Length(sliced.Length));

            try
            {
                if (Convert.TryFromBase64Chars(sliced, bytes, out var bytesWritten))
                {
                    byteArr = new byte[bytesWritten];
                    Buffer.BlockCopy(bytes, 0, byteArr, 0, bytesWritten);
                }
                else
                {
                    var chars = sliced.ToArray();
                    byteArr = Convert.FromBase64CharArray(chars, 0, chars.Length);
                }
            }
            finally
            {
                BufferPool.ReleaseBufferToPool(ref bytes);
            }

            return byteArr;
        }
    }
}
