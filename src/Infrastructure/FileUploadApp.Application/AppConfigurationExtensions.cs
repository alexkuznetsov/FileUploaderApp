using System;
using System.Collections.Generic;

using FileUploadApp.Domain;

namespace FileUploadApp.Application;

public static class AppConfigurationExtensions
{

    internal static IEnumerable<(byte[], string)> GetMimeFingerprints(this AppConfiguration appConfiguration)
    {
        foreach (var (key, value) in appConfiguration.Mappings)
        {
            var converted = Convert.FromBase64String(key);

            yield return (converted, value);
        }
    }
}

