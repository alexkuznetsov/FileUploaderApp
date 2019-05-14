using FileUploadApp.Domain;

namespace FileUploadApp.Events
{
    public class PlainTextRequestEvent : GenericEvent
    {
        public PlainTextRequestEvent(string text)
        {
            Text = text;
        }

        public string Text { get; }
    }
}
