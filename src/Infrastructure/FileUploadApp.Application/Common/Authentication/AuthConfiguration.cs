using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace FileUploadApp.Application.Common.Authentication;


public class AuthConfigurationOptionsSetup(IConfiguration configuration)
    : IConfigureOptions<AuthConfiguration>
{
    const string SectionKey = "AuthServer";

    public void Configure(AuthConfiguration options)
    {
        var conf = new AuthConfiguration();
        configuration.GetSection(SectionKey).Bind(conf);
    }
}

public class AuthConfiguration
{
    public class CsSettings
    {
        public string ProviderName { get; set; } = null!;

        public string ConnectionString { get; set; } = null!;
    }

    public CsSettings ConnectionString { get; set; } = new CsSettings
    {
        ConnectionString = "Data Source=localhost;Initial Catalog=authcatalog;Integrated Security=True;Encrypt=false",
        ProviderName = "Microsoft.Data.SqlClient"
    };
}
