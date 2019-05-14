using FileUploadApp.Domain;
using System.Collections.Generic;

namespace FileUploadApp.Events
{
    public class FilesRequestEvent : GenericEvent
    {
        public FilesRequestEvent(IEnumerable<FormFileDescriptor> formFileDescriptors)
        {
            FormFileDescriptors = formFileDescriptors;
        }

        public IEnumerable<FormFileDescriptor> FormFileDescriptors { get; }
    }
}
