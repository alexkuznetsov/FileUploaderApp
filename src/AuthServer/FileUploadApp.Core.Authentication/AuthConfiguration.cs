namespace FileUploadApp.Authentication;

public class AuthConfiguration
{
    public const string SectionKey = "AuthServer";
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
