using FileUploadApp.Core.Configuration;
using FileUploadApp.Interfaces;
using System;
using System.Collections.Generic;

namespace FileUploadApp.Services
{
    public class ContentTypeTestUtility : IContentTypeTestUtility
    {
        private readonly ICollection<string> contentTypes;
        private readonly IReadOnlyDictionary<string, string> mappings;

        public ContentTypeTestUtility(AppConfiguration appConfiguration)
        {
            contentTypes = new HashSet<string>(appConfiguration.AllowedContentTypes);
            mappings = appConfiguration.Mappings;
        }

        public bool IsAllowed(string contentType) => contentTypes.Contains(contentType);

        public string DetectContentType(ReadOnlySpan<byte> bytes)
        {
            var base64 = Convert.ToBase64String(bytes, Base64FormattingOptions.None);

            return DetectContentType(base64);
        }


        public string DetectContentType(string base64)
        {
            foreach (var p in mappings)
            {
                if (base64.StartsWith(p.Key))
                    return p.Value;
            }

            //if (base64.StartsWith(MimeConstants.PngB64))
            //    return MimeConstants.PngMime;

            //if (base64.StartsWith(MimeConstants.JpgB64))
            //    return MimeConstants.JpgMime;

            //if (base64.StartsWith(MimeConstants.BitmapB64))
            //    return MimeConstants.BitmapMime;

            //if (base64.StartsWith(MimeConstants.TiffB64))
            //    return MimeConstants.TiffMime;

            //if (base64.StartsWith(MimeConstants.GifB64))
            //    return MimeConstants.GifMime;

            return MimeConstants.OctetStreamMime;
        }
    }
}
