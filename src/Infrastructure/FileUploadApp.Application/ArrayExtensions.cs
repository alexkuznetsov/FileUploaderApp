using System;

namespace FileUploadApp.Application;

public static class ArrayExtensions
{
    public static T[] Slice<T>(this T[] source, int from, int len)
    {
        var result = new T[len];
        Array.Copy(source, from, result, 0, len);

        return result;
    }
}
