using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace FileUploadApp.Domain
{
    public class UploadResult
    {
        private UploadResultRow[] result;

        public UploadResult(UploadResultRow[] result)
        {
            this.result = result;
        }
    }

    public class UploadResultRowCore
    {
        public string Id { get; set; }

        public string Path { get; set; }

        public string ContentType { get; set; }

        public uint Width { get; set; }

        public uint Height { get; set; }
    }

    public class UploadResultRow : UploadResultRowCore
    {
        public UploadResultRowCore Preview { get; set; }
    }
}
