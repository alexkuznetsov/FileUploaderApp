using FileUploadApp.Domain;

namespace FileUploadApp.Events
{
    public class ProcessFileDescriptorEvent : GenericEvent
    {
        public ProcessFileDescriptorEvent(FileDescriptor[] files)
        {
            Files = files;
        }

        public FileDescriptor[] Files { get; }
    }
}
