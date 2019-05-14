using System;

namespace FileUploadApp.Events
{
    public class PlainTextRequestEvent : GenericEvent
    {
        public PlainTextRequestEvent(ReadOnlyMemory<char> text)
        {
            Text = text;
        }

        public ReadOnlyMemory<char> Text { get; }
    }
}
