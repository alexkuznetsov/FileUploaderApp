using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;

namespace FileUploadApp.Domain
{
    public class AppConfiguration
    {
        public string[] AllowedContentTypes { get; set; } = new[] { "image/jpeg", "image/png", "image/bmp", "image/x-windows-bmp", "image/gif", "image/tiff" };

        public Dictionary<string, string> Mappings { get; set; } = new Dictionary<string, string>
        {
            //[new byte[] { 137, 80, 78, 71 }    ] = "image/png",
            //[new byte[] { 255, 216, 255, 224 } ] = "image/jpeg",
            //[new byte[] { 66, 77, 166, 21 }    ] = "image/bmp",
            //[new byte[] { 73, 73, 42, 0 }      ] = "image/tiff",
            //[new byte[] { 71, 73, 70, 56 }     ] = "image/gif"
            ["iVBORw=="] = "image/png",
            ["/9j/4A=="] = "image/jpeg",
            ["Qk2mFQ=="] = "image/bmp",
            ["SUkqAA=="] = "image/tiff",
            ["R0lGOA=="] = "image/gif",
        };

        public string DefaultUserAgent { get; set; } = "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/67.0.3396.87 Safari/537.36 OPR/54.0.2952.46";

        public Size PreviewSize { get; set; } = new Size(100, 100);
    }

    public static class AppConfigurationExtensions
    {
        private static readonly ConcurrentDictionary<string, byte[]>
            mimeCache = new ConcurrentDictionary<string, byte[]>();

        public static IEnumerable<(byte[], string)> GetFingerprints(this AppConfiguration appConfiguration)
        {
            foreach (var k in appConfiguration.Mappings)
            {
                var converted = Convert.FromBase64String(k.Key);

                yield return (mimeCache.GetOrAdd(k.Value, converted), k.Value);
            }

        }
    }
}

