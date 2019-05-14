using System;

namespace FileUploadApp.Events
{
    public class ProcessImageUriEvent : GenericEvent
    {
        public ProcessImageUriEvent(string[] links)
        {
            Links = links;
        }

        public string[] Links { get; }
    }

    public class AfterDownloadImageUriEvent : GenericEvent
    {
        public AfterDownloadImageUriEvent(Uri uri, byte[] bytea)
        {
            Uri = uri;
            Bytea = bytea;
        }

        public Uri Uri { get; }
        public byte[] Bytea { get; }
    }
}
