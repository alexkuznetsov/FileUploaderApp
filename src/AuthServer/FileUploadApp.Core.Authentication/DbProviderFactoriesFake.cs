using System;
using System.Data.Common;
using System.Data.SqlClient;

namespace FileUploadApp.Authentication;

public static class DbProviderFactoriesFake
{
    public static DbProviderFactory GetFactory(string providerName)
    {
        if (providerName.ToLowerInvariant().Equals("system.data.sqlclient"))
        {
            return SqlClientFactory.Instance;
        }

        throw new ArgumentOutOfRangeException(nameof(providerName));
    }
}
