using BROS_ECommerce.Domain.Entities;
using BROS_ECommerce.Domain.Interfaces.Repository;
using BROS_ECommerce.Infra.Context;
using BROS_ECommerce.Infra.Repository;
using BROS_ECommerce.Services.Interface.Services;
using BROS_ECommerce.Services.Services;
using BROS_ECommerce.Web.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BROS_ECommerce.Web.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            #region Database
            services.AddDbContext<BrosContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            services.AddDbContext<TenantDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            services.AddDbContext<ImagemDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            #endregion

            #region Identity/Authentication
            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
            #endregion

            #region Repositórios
            services.AddScoped<IRepositoryProduto, ProdutoRepository>();
            services.AddScoped<IRepositoryUser, UserRepository>();
            services.AddScoped<IRepositoryEstoque, EstoqueRepository>();
            services.AddScoped<IRepositoryImagem, ImagemRepository>();
            services.AddScoped<IRepositoryProdutoImagem, ProdutoImagemRepository>();
            services.AddScoped<IRepositoryCarrinho, CarrinhoRepository>();
            services.AddScoped<IRepositoryCarrinhoItem, CarrinhoItemRepository>();
            services.AddScoped<IRepositoryCategoria, CategoriaRepository>();
            services.AddScoped<IRepositoryPedido, PedidoRepository>();
            services.AddScoped<IRepositoryPedidoItem, PedidoItemRepository>();
            services.AddScoped<IRepositoryCategoriaProduto, RepositoryCategoriaProduto>();
            services.AddScoped<IRepositoryTenant, TenantRepository>();
            services.AddScoped<IRepositoryBanner, BannerRepository>();
            #endregion

            #region Serviços
            services.AddScoped<IServiceProduto, ProdutoService>();
            services.AddScoped<IServiceUser, ServiceUser>();
            services.AddScoped<IServiceEstoque, ServiceEstoque>();
            services.AddScoped<IServiceImagem, ServiceImagem>();
            services.AddScoped<JwtService>();
            services.AddScoped<IServiceCarrinho, ServiceCarrinho>();
            services.AddScoped<IServiceCategoria, ServiceCategoria>();
            services.AddScoped<IServicePedido, ServicePedido>();
            services.AddScoped<IServicePedidoIntegracao, ServicePedidoIntegracao>();
            services.AddScoped<IServiceCategoriaProduto, ServiceCategoriaProduto>();
            services.AddScoped<IServiceTenant, ServiceTenant>();
            services.AddScoped<IServiceBanner, ServiceBanner>();
            #endregion

            #region Serviços de Pagamento
            services.AddScoped<IStripeService, StripeService>();
            services.AddScoped<IMercadoPagoService, MercadoPagoService>();
            #endregion
        }
    }
}