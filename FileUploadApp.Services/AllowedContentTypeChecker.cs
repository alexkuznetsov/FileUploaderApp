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
            foreach (var (ctBytes, contentType) in appConfiguration.GetFingerprints())
            {
                if (bytes.SequenceEqual(ctBytes))
                    return contentType;
            }

            return MimeConstants.OctetStreamMime;
        }
    }
}
