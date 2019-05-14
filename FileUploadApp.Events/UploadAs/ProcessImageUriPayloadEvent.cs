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
}
