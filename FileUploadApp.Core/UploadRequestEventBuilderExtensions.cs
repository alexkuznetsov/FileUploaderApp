using FileUploadApp.Domain;
using FileUploadApp.Domain.Dirty;
using FileUploadApp.Interfaces;
using FileUploadApp.StreamAdapters;
using System;
using System.Collections.Generic;

namespace FileUploadApp.Core
{
    public static class UploadRequestEventBuilderExtensions
    {
        private static readonly string CharsetToken = "charset";
        private static readonly string Base64Token = "base64";

        public static IEnumerable<Upload> AsFileDesciptors(this IEnumerable<Base64FilePayload> files, IContentTypeTestUtility contentTypeTestUtility)
        {
            var number = 0U;

            foreach (var f in files)
            {
                string contentType;
                byte[] bytea;
                string charset;
                bool isBase64 = false;

                if (f.IsDataURI())
                {
                    var colonPos = f.RawData.IndexOf(',');

                    if (colonPos == -1)
                        throw new ArgumentException("Invalid data paylod for " + f.Name);

                    var arrHeader = f.RawData.Substring(0, colonPos).ToLowerInvariant()
                        .Split(';');

                    var ctPart = arrHeader[0].Split(':');
                    contentType = ctPart.Length > 1 ? ctPart[1] : string.Empty;

                    if (arrHeader.Length > 1)
                    {
                        if (arrHeader[1].StartsWith(CharsetToken))
                        {
                            var charsetPart = arrHeader[1].Split('=');
                            charset = charsetPart[1];
                        }
                        else if (arrHeader[1].Equals(Base64Token))
                        {
                            isBase64 = true;
                        }

                        if (!isBase64 && arrHeader.Length > 2)
                        {
                            isBase64 = arrHeader[2].Equals(Base64Token);
                        }
                    }

                    if (isBase64)
                        bytea = Convert.FromBase64String(f.RawData.Substring(colonPos + 1));
                    else
                        throw new ArgumentException("Invalid encoding type for " + f.Name);
                }
                else
                {
                    bytea = Convert.FromBase64String(f.RawData);
                    contentType = string.Empty;
                }

                if (string.IsNullOrEmpty(contentType))
                {
                    contentType = contentTypeTestUtility.DetectContentType(bytea.Slice(0, 4).AsSpan());
                }

                if (contentTypeTestUtility.IsAllowed(contentType))
                {
                    yield return new Upload(
                        id: Guid.NewGuid(),
                        previewId: Guid.NewGuid(),
                        num: number++,
                        name: f.Name,
                        contentType: contentType,
                        streamAdapter: new ByteaStreamAdapter(bytea));
                }
            }
        }

    }
}
