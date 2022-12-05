using FileUploadApp.Core.Encoding;
using FileUploadApp.Domain;
using FileUploadApp.Domain.Raw;
using FileUploadApp.Interfaces;
using FileUploadApp.StreamAdapters;
using System;
using System.Collections.Generic;
using System.IO;

namespace FileUploadApp.Features;

public static class UploadRequestEventBuilderExtensions
{
    public static IEnumerable<Upload> AsFileDescriptors(this IEnumerable<Base64FilePayload> files
        , IContentTypeTestUtility contentTypeTestUtility)
    {
        var number = 0U;

        foreach (var rawFile in files)
        {
            string contentType;

            var data = rawFile.RawData.AsSpan();

            if (data == null)
            {
                continue;
            }

            byte[] byteArr;
            if (data.StartsWith(Base64FilePayload.DataToken.AsSpan(), StringComparison.InvariantCultureIgnoreCase))
            {
                (contentType, byteArr) = Base64Parser.Parse(data, rawFile.Name);
            }
            else
            {
                byteArr = Base64ConvertHelper.ConvertToBytes(data);
                contentType = string.Empty;
            }

            if (string.IsNullOrEmpty(contentType))
            {
                contentType = contentTypeTestUtility.DetectContentType(byteArr[..4].AsSpan());
            }

            if (contentTypeTestUtility.IsAllowed(contentType))
            {
                yield return new Upload(
                    id: Guid.NewGuid(),
                    previewId: Guid.NewGuid(),
                    num: number++,
                    name: rawFile.Name,
                    contentType: contentType,
                    //streamAdapter: new ByteaStreamAdapter(byteArr)
                    streamAdapter: new CommonStreamStreamAdapter(new MemoryStream(byteArr))
                    );
            }
        }
    }
}
