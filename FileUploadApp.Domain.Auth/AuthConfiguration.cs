namespace FileUploadApp.Domain.Authentication
{
    public class AuthConfiguration
    {
        public class CsSettings
        {
            public string ProviderName { get; set; }

            public string ConnectionString { get; set; }
        }

        public CsSettings ConnectionString { get; set; } = new CsSettings
        {
            ConnectionString = "Data Source=localhost;Initial Catalog=authcatalog;Integrated Security=True;",
            ProviderName = "System.Data.SqlClient"
        };
    }
}
