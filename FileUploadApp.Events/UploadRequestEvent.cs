using FileUploadApp.Domain;

namespace FileUploadApp.Events
{
    public class UploadRequestEvent : GenericEvent
    {
        public UploadRequestEvent(UploadRequest uploadRequest)
        {
            UploadRequest = uploadRequest;
        }

        public UploadRequest UploadRequest { get; }
    }
}
