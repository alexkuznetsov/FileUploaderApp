using System;
using System.Collections.Generic;
using System.Linq;

using FileUploadApp.Domain;
using FileUploadApp.Interfaces;

namespace FileUploadApp.Application.Services;

internal sealed class ContentTypeTestUtility : IContentTypeTestUtility
{
    private readonly (byte[], string)[] _fingerprints;
    private readonly HashSet<string> _contentTypes;

    public ContentTypeTestUtility(AppConfiguration appConfiguration)
    {
        _fingerprints = appConfiguration.GetMimeFingerprints().ToArray();
        _contentTypes = new HashSet<string>(appConfiguration.AllowedContentTypes);
    }

    public bool IsAllowed(string contentType) => _contentTypes.Contains(contentType);

    public string DetectContentType(ReadOnlySpan<byte> bytes)
    {
        foreach (var (ctBytes, contentType) in _fingerprints)
        {
            if (bytes.SequenceEqual(ctBytes))
                return contentType;
        }

        return MimeConstants.OctetStreamMime;
    }
}
