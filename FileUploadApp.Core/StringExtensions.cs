using System;
using System.Collections.Generic;

namespace FileUploadApp
{
    public static class StringExtensions
    {
        public static IEnumerable<Uri> AsUriEnumerable(this string[] source, Action<string> onError = null)
        {
            for (uint i = 0; i < source.Length; i++)
            {
                var status = Uri.TryCreate(source[i], UriKind.Absolute, out var result);

                if (status)
                {
                    yield return result;
                }
                else
                {
                    onError?.Invoke(source[i]);
                }
            }
        }
    }
}
