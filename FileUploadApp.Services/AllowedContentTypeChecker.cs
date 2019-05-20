using FileUploadApp.Core;
using FileUploadApp.Domain;
using FileUploadApp.Interfaces;
using System;
using System.Collections.Generic;

namespace FileUploadApp.Services
{
    public class ContentTypeTestUtility : IContentTypeTestUtility
    {
        private readonly ICollection<string> contentTypes;
        private readonly AppConfiguration appConfiguration;

        public ContentTypeTestUtility(AppConfiguration appConfiguration)
        {
            this.appConfiguration = appConfiguration;
            contentTypes = new HashSet<string>(appConfiguration.AllowedContentTypes);
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
    }
}
