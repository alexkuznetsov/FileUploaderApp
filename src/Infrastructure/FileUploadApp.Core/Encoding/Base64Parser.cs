using System;

namespace FileUploadApp.Core.Encoding;

public static partial class Base64Parser
{
    private const string CharsetToken = "charset";
    private const string Base64Token = "base64";

    public static Base64ParserResult Parse(ReadOnlySpan<char> data, string fileName)
    {
        var commaPos = data.IndexOf(',');

        if (commaPos == -1)
            throw new ArgumentException($"Invalid data payload for {fileName}");

        var headerEnumerator = data[..commaPos].ToLower().Split(';');
        headerEnumerator.MoveNext();

        var ctPartEnumerator = headerEnumerator.Current.Split(':');
        var ctPartLength = ctPartEnumerator.Last(out var ctPart);
        var contentType = ctPartLength > 1 ? ctPart : string.Empty;
        var isBase64 = false;

        var charsetToken = "charset".AsSpan();
        var base64Token = "base64".AsSpan();

        if (headerEnumerator.MoveNext())
        {
            if (headerEnumerator.Current.StartsWith(charsetToken))
            {
                var charsetPart = headerEnumerator.Current.Split('=');
                var charsetPartLen = charsetPart.Last(out _); //TODO Work with charset
                if (charsetPartLen == 2)
                {
                    //TODO Charset now not using
                }
                else
                    throw new ArgumentException($"Invalid charset description for {fileName}");
            }
            else if (headerEnumerator.Current.SequenceEqual(base64Token))
            {
                isBase64 = true;
            }

            if (!isBase64 && headerEnumerator.MoveNext())
            {
                isBase64 = headerEnumerator.Current.SequenceEqual(base64Token);
            }
        }

        if (!isBase64)
            throw new ArgumentException($"Invalid encoding type for {fileName}");

        return new Base64ParserResult(contentType
            , bytes: Base64ConvertHelper.ConvertToBytes(data[(commaPos + 1)..]));
    }

    [Obsolete("Previous logic")]
    // ReSharper disable once UnusedMethodReturnValue.Global
    public static Base64ParserResult Parse(string data, string fileName)
    {
        var colonPos = data.IndexOf(',');

        if (colonPos == -1)
            throw new ArgumentException($"Invalid data payload for {fileName}");

        var arrHeader = data[..colonPos].ToLowerInvariant().Split(';');

        var ctPart = arrHeader[0].Split(':');
        var contentType = ctPart.Length > 1 ? ctPart[1] : string.Empty;
        var isBase64 = false;

        if (arrHeader.Length > 1)
        {
            if (arrHeader[1].StartsWith(CharsetToken))
            {
                var charsetPart = arrHeader[1].Split('=');
                _ = charsetPart[1]; //TODO Charset now not using
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

        byte[] byteArr;

        if (isBase64)
            byteArr = Convert.FromBase64String(data[(colonPos + 1)..]);
        else
            throw new ArgumentException($"Invalid encoding type for {fileName}");

        return new Base64ParserResult(contentType, byteArr);
    }
}