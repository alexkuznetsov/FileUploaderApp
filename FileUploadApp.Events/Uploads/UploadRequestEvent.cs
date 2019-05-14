using FileUploadApp.Domain;

namespace FileUploadApp.Events
{
    public class UploadRequestEvent : GenericEvent
    {
        public UploadRequestEvent(FileDescriptor[] files, string[] links)
        {
            Files = files;
            Links = links;
        }

        public FileDescriptor[] Files { get; }
        public string[] Links { get; }
    }
}
