using FileUploadApp.Domain;
using FileUploadApp.Interfaces;
using System;
using System.Collections.Generic;

namespace FileUploadApp.Services
{
    public class ContentTypeTestUtility : IContentTypeTestUtility
    {
        private readonly ICollection<string> contentTypes;
        private readonly IReadOnlyDictionary<string, string> mappings;
        private readonly AppConfiguration appConfiguration;

        public ContentTypeTestUtility(AppConfiguration appConfiguration)
        {
            contentTypes = new HashSet<string>(appConfiguration.AllowedContentTypes);
            mappings = appConfiguration.Mappings;
            this.appConfiguration = appConfiguration;
        }

        public bool IsAllowed(string contentType) => contentTypes.Contains(contentType);

        public string DetectContentType(ReadOnlySpan<byte> bytes)
        {
            foreach (var c in appConfiguration.GetFingerprints())
            {
                if (bytes.SequenceEqual(c.Item1))
                    return c.Item2;
            }

            return MimeConstants.OctetStreamMime;

        }

        [Obsolete("Данный метод не рекомендуем")]
        public string DetectContentType(string base64)
        {
            foreach (var p in mappings)
            {
                if (base64.StartsWith(p.Key))
                    return p.Value;
            }

            return MimeConstants.OctetStreamMime;
        }
    }
}
