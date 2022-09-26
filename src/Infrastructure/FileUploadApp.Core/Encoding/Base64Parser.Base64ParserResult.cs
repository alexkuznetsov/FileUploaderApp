using System;

namespace FileUploadApp.Core.Encoding;

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

        public void Deconstruct(out string outContentType, out ReadOnlyMemory<byte> outByteArr)
        {
            outContentType = new string(contentType);
            outByteArr = bytes;
        }
    }
}