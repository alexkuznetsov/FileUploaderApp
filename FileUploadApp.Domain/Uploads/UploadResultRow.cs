using System;

namespace FileUploadApp.Domain
{
    public class UploadResultRow : FileEntity
    {
        public UploadResultRow(Guid id, uint number, string name, string contentType) :
            base(id, number, name, contentType)
        {
        }

        public FileEntity Preview { get; set; }
    }
}
