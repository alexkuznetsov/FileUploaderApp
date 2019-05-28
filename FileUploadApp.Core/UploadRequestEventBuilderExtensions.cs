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
        public static IEnumerable<Upload> AsFileDesciptors(this IEnumerable<Base64FilePayload> files, IContentTypeTestUtility contentTypeTestUtility)
        {
            var number = 0U;
            ReadOnlyMemory<byte> bytea;

            foreach (var rawFile in files)
            {
                string contentType;

                var data = rawFile.RawData.AsSpan();

                if (data.StartsWith(Base64FilePayload.DataToken.AsSpan(), StringComparison.InvariantCultureIgnoreCase))
                {
                    (contentType, bytea) = Base64Parser.Parse(data, rawFile.Name);
                }
                else
                {
                    bytea = Base64ConvertHelper.ConvertToBytes(data);
                    contentType = string.Empty;
                }

                if (string.IsNullOrEmpty(contentType))
                {
                    contentType = contentTypeTestUtility.DetectContentType(bytea.Slice(0, 4).Span);
                }

                if (contentTypeTestUtility.IsAllowed(contentType))
                {
                    yield return new Upload(
                        id: Guid.NewGuid(),
                        previewId: Guid.NewGuid(),
                        num: number++,
                        name: rawFile.Name,
                        contentType: contentType,
                        streamAdapter: new ByteaStreamAdapter(bytea));
                }
            }
        }
    }
}
