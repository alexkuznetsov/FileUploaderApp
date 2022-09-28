using FileUploadApp.Domain;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace FileUploadApp.Core
{
    public static class AppConfigurationExtensions
    {
        private static readonly ConcurrentDictionary<string, byte[]>
            MimeCache = new();

        public static IEnumerable<(byte[], string)> GetFingerprints(this AppConfiguration appConfiguration)
        {
            foreach (var (key, value) in appConfiguration.Mappings)
            {
                var converted = Convert.FromBase64String(key);

                yield return (MimeCache.GetOrAdd(value, converted), value);
            }
        }
    }
}

