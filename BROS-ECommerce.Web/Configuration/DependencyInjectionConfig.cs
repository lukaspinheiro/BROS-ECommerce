using BROS_ECommerce.Domain.Interfaces.Crud;
using BROS_ECommerce.Domain.Interfaces.Repository;
using BROS_ECommerce.Infra.Context;
using BROS_ECommerce.Infra.Repository;
using BROS_ECommerce.Services.Interface.Services;
using BROS_ECommerce.Services.Services;
using Microsoft.EntityFrameworkCore;

namespace BROS_ECommerce.Web.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            #region Repositórios
            services.AddScoped<IRepositoryProduto, ProdutoRepository>();
            #endregion


            #region Serviços
            services.AddScoped<IServiceProduto, ProdutoService>();
            #endregion

            #region Contextos
            services.AddDbContext<BrosContext>(x => x.UseNpgsql(configuration.GetConnectionString("DbGymBros")));

            services.AddScoped<IUnitOfWork>(tc => tc.GetRequiredService<BrosContext>());
            #endregion
        }
    }
}
