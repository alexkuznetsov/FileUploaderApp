using Microsoft.Extensions.Configuration;

namespace FileUploadApp
{
    public static class ConfigurationExtensions
    {
        public static TModel BindTo<TModel>(this IConfiguration configuration, string sectionName)
            where TModel : class, new()
        {
            var model = new TModel();
            configuration.GetSection(sectionName).Bind(model);

            return model;
        }
    }
}
