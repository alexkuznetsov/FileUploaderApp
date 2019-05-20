using FileUploadApp.Domain;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace FileUploadApp.Core
{
    public static class AppConfigurationExtensions
    {
        private static readonly ConcurrentDictionary<string, byte[]>
            mimeCache = new ConcurrentDictionary<string, byte[]>();

        public static IEnumerable<(byte[], string)> GetFingerprints(this AppConfiguration appConfiguration)
        {
            foreach (var k in appConfiguration.Mappings)
            {
                var converted = Convert.FromBase64String(k.Key);

                yield return (mimeCache.GetOrAdd(k.Value, converted), k.Value);
            }

        }
    }
}

