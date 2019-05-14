using FileUploadApp.Domain;
using System.Collections.Generic;

namespace FileUploadApp.Events
{
    public class FileUploadEvent : GenericEvent
    {
        public FileUploadEvent(IEnumerable<FileDescriptor> formFileDescriptors)
        {
            FormFileDescriptors = formFileDescriptors;
        }

        public IEnumerable<FileDescriptor> FormFileDescriptors { get; }
    }
}
