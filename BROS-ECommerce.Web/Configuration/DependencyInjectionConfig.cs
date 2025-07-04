using BROS_ECommerce.Services.Interface.Services;
using BROS_ECommerce.Services.Services;
using BROS_ECommerce.Infra.Context;
using BROS_ECommerce.Domain.Interfaces.Repository;
using BROS_ECommerce.Infra.Repository;
using BROS_ECommerce.Domain.Entities;
using BROS_ECommerce.Web.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace BROS_ECommerce.Web.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            #region Database
            services.AddDbContext<BrosContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            #endregion

            #region Identity/Authentication
            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
            #endregion

            #region Repositórios
            services.AddScoped<IRepositoryProduto, ProdutoRepository>();
            services.AddScoped<IRepositoryUser, UserRepository>();
            services.AddScoped<IRepositoryEstoque, EstoqueRepository>();
            #endregion

            #region Serviços
            services.AddScoped<IServiceProduto>(provider =>
            {
                var repositoryProduto = provider.GetRequiredService<IRepositoryProduto>();
                var repositoryEstoque = provider.GetRequiredService<IRepositoryEstoque>();
                return new ProdutoService(repositoryProduto, repositoryEstoque);
            });

            services.AddScoped<IServiceUser, ServiceUser>();
            services.AddScoped<IServiceEstoque, EstoqueService>();
            services.AddScoped<JwtService>();
            #endregion
        }
    }
}