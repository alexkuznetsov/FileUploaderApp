using FileUploadApp.Domain;
using System.Collections.Generic;

namespace FileUploadApp.Events
{
    public class ProcessImageBase64Event : GenericEvent
    {
        public ProcessImageBase64Event(Base64FilePayload[] files)
        {
            Files = files;
        }

        public Base64FilePayload[] Files { get; }
    }
}
