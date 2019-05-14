using System;

namespace FileUploadApp.Interfaces
{
    public interface IContentTypeTestUtility
    {
        [Obsolete("Требуется определять по байтам")]
        string DetectContentType(string base64);

        string DetectContentType(ReadOnlySpan<byte> bytes);

        bool IsAllowed(string contentType);
    }

}
