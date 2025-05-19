using BROS_ECommerce.Services.Interface.Services;
using BROS_ECommerce.Services.Services;

namespace BROS_ECommerce.Web.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            #region Serviços
            services.AddScoped<IServiceProduto, ProdutoService>();

            #endregion
        }
    }
}
