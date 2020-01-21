using FileUploadApp.Domain;
using System.Collections.Generic;

namespace FileUploadApp.Events
{
    public class UploadFilesEvent : GenericEvent
    {
        public UploadFilesEvent(IEnumerable<Upload> uploadedFiles)
        {
            UploadedFiles = uploadedFiles;
        }

        public IEnumerable<Upload> UploadedFiles { get; }
    }
}
