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
        public AfterDownloadImageUriEvent(uint number, Uri uri, byte[] bytea)
        {
            Number = number;
            Uri = uri;
            Bytea = bytea;
        }

        public uint Number { get; }
        public Uri Uri { get; }
        public byte[] Bytea { get; }
    }
}
