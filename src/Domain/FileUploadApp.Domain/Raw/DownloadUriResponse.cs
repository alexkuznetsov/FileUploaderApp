using System;
using System.IO;

namespace FileUploadApp.Domain.Raw;

public record DownloadUriResponse(Uri Uri, string ContentType, Stream Stream);
