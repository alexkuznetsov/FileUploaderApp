using System;
using System.Data.Common;

using Microsoft.Data.SqlClient;

namespace FileUploadApp.Authentication;

public static class DbProviderFactoriesFake
{
    public static DbProviderFactory GetFactory(string providerName)
    {
        var p = providerName.ToLowerInvariant();

        return p switch
        {
            "microsoft.data.sqlclient" => SqlClientFactory.Instance,
            _ => throw new ArgumentOutOfRangeException(nameof(providerName))
        };
    }
}
