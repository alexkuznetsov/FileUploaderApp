using System;

namespace FileUploadApp.Core
{
    public static class ArrayExtensions
    {
        public static T[] Slice<T>(this T[] source, int from, int len)
        {
            T[] result = new T[len];
            Array.Copy(source, from, result, 0, len);

            return result;
        }
    }
}
