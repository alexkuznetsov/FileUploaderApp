namespace FileUploadApp.Features.Services;

public static class MimeConstants
{
    public static readonly string PngB64 = "iVBORw";
    public static readonly string PngMime = "image/png";

    public static readonly string JpgB64 = "/9j/4A";
    public static readonly string JpgMime = "image/jpeg";

    public static readonly string BitmapB64 = "Qk0=";
    public static readonly string BitmapMime = "image/bmp";

    public static readonly string TiffB64 = "SUkq";
    public static readonly string TiffMime = "image/tiff";

    public static readonly string GifB64 = "R0lG";
    public static readonly string GifMime = "image/gif";

    public static readonly string SevenZipMime = "application/x-7z-compressed";
    public static readonly string OctetStreamMime = "binary/octet-stream";
}
