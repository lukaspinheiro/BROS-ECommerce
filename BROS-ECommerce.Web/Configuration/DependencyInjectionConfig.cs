using BROS_ECommerce.Services.Interface.Services;
using BROS_ECommerce.Services.Services;
using BROS_ECommerce.Infra.Context;
using BROS_ECommerce.Domain.Interfaces.Repository;
using BROS_ECommerce.Infra.Repository;
using Microsoft.EntityFrameworkCore;

namespace BROS_ECommerce.Web.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            #region Database
            services.AddDbContext<BrosContext>(options =>
                options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));
            #endregion

            #region Repositórios
            services.AddScoped<IRepositoryProduto, ProdutoRepository>();
            #endregion

            #region Serviços
            services.AddScoped<IServiceProduto, ProdutoService>();
            #endregion
        }
    }
}