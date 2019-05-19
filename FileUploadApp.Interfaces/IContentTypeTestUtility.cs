using System;

namespace FileUploadApp.Interfaces
{
    public interface IContentTypeTestUtility
    {
        string DetectContentType(ReadOnlySpan<byte> bytes);

        bool IsAllowed(string contentType);
    }

}
