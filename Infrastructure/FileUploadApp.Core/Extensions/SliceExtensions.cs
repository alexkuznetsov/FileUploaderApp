using System;

namespace FileUploadApp.Core
{
    internal static class SliceExtensions
    {
        public static ReadOnlySpan<char> ToLower(this ReadOnlySpan<char> span)
        {
            var destination = new Span<char>();
            var copied = span.ToLowerInvariant(destination);

            return copied > 0 ? destination : span;
        }
    }
}
