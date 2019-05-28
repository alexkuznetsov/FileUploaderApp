using System;

namespace FileUploadApp.Core
{
    public static partial class Base64Parser
    {
        public ref struct Base64ParserResult
        {
            private readonly ReadOnlySpan<char> contentType;
            private readonly byte[] bytes;

            public Base64ParserResult(ReadOnlySpan<char> contentType, byte[] bytes)
            {
                this.contentType = contentType;
                this.bytes = bytes;
            }

            internal void Deconstruct(out string contentType, out ReadOnlyMemory<byte> bytea)
            {
                contentType = new string(this.contentType);
                bytea = bytes;
            }
        }
    }
}
