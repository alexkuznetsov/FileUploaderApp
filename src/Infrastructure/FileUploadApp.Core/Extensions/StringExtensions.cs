﻿using System;
using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace FileUploadApp
{
    public static class StringExtensions
    {
        public static IEnumerable<(uint, Uri)> AsOrderedUriEnumerable(
              this IEnumerable<string> source
            , Action<string> onError = null)
        {
            var i = 0U;

            foreach (var link in source)
            {
                var status = Uri.TryCreate(link, UriKind.Absolute, out var result);

                if (status)
                {
                    yield return (i, result);
                }
                else
                {
                    onError?.Invoke(link);
                }
                i++;
            }
        }
    }
}
