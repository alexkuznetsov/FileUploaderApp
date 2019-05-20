using System.Collections.Generic;
using System.Drawing;

namespace FileUploadApp.Domain
{
    public class AppConfiguration
    {
        public string[] AllowedContentTypes { get; set; } = new[] { "image/jpeg", "image/png", "image/bmp", "image/x-windows-bmp", "image/gif", "image/tiff" };

        public Dictionary<string, string> Mappings { get; set; } = new Dictionary<string, string>
        {
            ["iVBORw=="] = "image/png",
            ["/9j/4A=="] = "image/jpeg",
            ["Qk2mFQ=="] = "image/bmp",
            ["SUkqAA=="] = "image/tiff",
            ["R0lGOA=="] = "image/gif",
        };

        public string DefaultUserAgent { get; set; } = "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/67.0.3396.87 Safari/537.36 OPR/54.0.2952.46";

        public Size PreviewSize { get; set; } = new Size(100, 100);
    }
}

