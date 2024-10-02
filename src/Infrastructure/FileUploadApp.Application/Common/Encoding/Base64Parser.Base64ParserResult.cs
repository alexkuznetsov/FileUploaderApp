using System;

namespace FileUploadApp.Application.Common.Encoding;

public static partial class Base64Parser
{
    public readonly ref struct Base64ParserResult
    {
        private readonly ReadOnlySpan<char> _contentType;
        private readonly byte[] _bytes;

        public Base64ParserResult(ReadOnlySpan<char> contentType, byte[] bytes)
        {
            this._contentType = contentType;
            this._bytes = bytes;
        }

        public void Deconstruct(out string outContentType, out byte[] outByteArr)
        {
            outContentType = new string(_contentType);
            outByteArr = _bytes;
        }
    }
}