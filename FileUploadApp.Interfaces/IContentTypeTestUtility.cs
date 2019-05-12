using System;

namespace FileUploadApp.Interfaces
{
    public interface IContentTypeTestUtility
    {
        string DetectContentType(string base64);
        bool IsAllowed(string contentType);
    }

}
