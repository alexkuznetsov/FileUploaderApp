using FileUploadApp.Core;
using FileUploadApp.Domain;
using FileUploadApp.Interfaces;
using System;
using System.Collections.Generic;

namespace FileUploadApp.Features.Services;

public class ContentTypeTestUtility : IContentTypeTestUtility
{
    private readonly ICollection<string> _contentTypes;
    private readonly AppConfiguration _appConfiguration;

    public ContentTypeTestUtility(AppConfiguration appConfiguration)
    {
        this._appConfiguration = appConfiguration;
        _contentTypes = new HashSet<string>(appConfiguration.AllowedContentTypes);
    }

    public bool IsAllowed(string contentType) => _contentTypes.Contains(contentType);

    public string DetectContentType(ReadOnlySpan<byte> bytes)
    {
        foreach (var (ctBytes, contentType) in _appConfiguration.GetFingerprints())
        {
            if (bytes.SequenceEqual(ctBytes))
                return contentType;
        }

        return MimeConstants.OctetStreamMime;
    }
}
