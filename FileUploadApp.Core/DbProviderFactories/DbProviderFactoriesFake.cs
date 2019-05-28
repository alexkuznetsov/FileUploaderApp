using System;
using System.Data.Common;

namespace FileUploadApp.Core.DbProviderFactories
{
    public static class DbProviderFactoriesFake
    {
        public static DbProviderFactory GetFactory(string providerName)
        {
            if (providerName.ToLowerInvariant().Equals("system.data.sqlclient"))
            {
                return System.Data.SqlClient.SqlClientFactory.Instance;
            }

            throw new ArgumentOutOfRangeException(nameof(providerName));
        }
    }
}
