using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileUploadApp.Core.Configuration
{
    public class AppConfiguration
    {
        public string[] AllowedContentTypes { get; set; }

        public Dictionary<string, string> Mappings { get; set; }
    }
}
