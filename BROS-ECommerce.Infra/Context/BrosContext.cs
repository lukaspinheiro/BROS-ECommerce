using BROS_ECommerce.Core.Interfaces;
using BROS_ECommerce.Domain.Entities;
using BROS_ECommerce.Domain.Interfaces;
using BROS_ECommerce.Domain.Interfaces.Crud;
using BROS_ECommerce.Infra.EntityConfig;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;


namespace BROS_ECommerce.Infra.Context
{
    public class BrosContext : DbContext, IUnitOfWork
    {
        private readonly IConfiguration _configuration; 
        private readonly ITenantAtualService _tenantAtualService;
        public string? TenantAtualId { get; set; }

        public BrosContext(DbContextOptions<BrosContext> options, IConfiguration configuration, ITenantAtualService tenantAtualService) : base(options)
        {
            _configuration = configuration;
            _tenantAtualService = tenantAtualService;
            TenantAtualId = _tenantAtualService?.TenantId;
        }

        public DbSet<Produto> Produtos { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Estoque> Estoque { get; set; }
        public DbSet<Imagem> Imagens { get; set; }
        public DbSet<ProdutoImagem> ProdutoImagens { get; set; }
        public DbSet<Carrinho> Carrinhos { get; set; }
        public DbSet<CarrinhoItem> CarrinhoItens { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<CategoriaProduto> CategoriaProdutos { get; set; }
        public DbSet<Promocao> Promocoes { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<PedidoItem> PedidoItens { get; set; }
        public DbSet<Tenant> Tenants { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ProdutoMap());
            modelBuilder.ApplyConfiguration(new UserMap());
            modelBuilder.ApplyConfiguration(new EstoqueMap());
            modelBuilder.ApplyConfiguration(new ImagemMap());
            modelBuilder.ApplyConfiguration(new ProdutoImagemMap());
            modelBuilder.ApplyConfiguration(new CategoriaMap());
            modelBuilder.ApplyConfiguration(new CategoriaProdutoMap());
            modelBuilder.ApplyConfiguration(new PromocaoMap());
            modelBuilder.ApplyConfiguration(new PedidoMap());
            modelBuilder.ApplyConfiguration(new PedidoItemMap());
            modelBuilder.ApplyConfiguration(new CarrinhoMap());
            modelBuilder.ApplyConfiguration(new CarrinhoItemMap());

            SeedUsers(modelBuilder);
            SeedEstoque(modelBuilder);

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Produto>().HasQueryFilter(a => a.TenantId == TenantAtualId);
            modelBuilder.Entity<User>().HasQueryFilter(a => a.TenantId == TenantAtualId);
            modelBuilder.Entity<Estoque>().HasQueryFilter(a => a.TenantId == TenantAtualId);
            modelBuilder.Entity<Imagem>().HasQueryFilter(a => a.TenantId == TenantAtualId);
            modelBuilder.Entity<ProdutoImagem>().HasQueryFilter(a => a.TenantId == TenantAtualId);
            modelBuilder.Entity<Carrinho>().HasQueryFilter(a => a.TenantId == TenantAtualId);
            modelBuilder.Entity<CarrinhoItem>().HasQueryFilter(a => a.TenantId == TenantAtualId);
            modelBuilder.Entity<Categoria>().HasQueryFilter(a => a.TenantId == TenantAtualId);
            modelBuilder.Entity<CategoriaProduto>().HasQueryFilter(a => a.TenantId == TenantAtualId);
            modelBuilder.Entity<Promocao>().HasQueryFilter(a => a.TenantId == TenantAtualId);
            modelBuilder.Entity<Pedido>().HasQueryFilter(a => a.TenantId == TenantAtualId);
            modelBuilder.Entity<PedidoItem>().HasQueryFilter(a => a.TenantId == TenantAtualId);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.LogTo(
                action: Console.WriteLine,
                minimumLevel: LogLevel.Information);
        }

        private void ApplyTenantFilter()
        {
            foreach (var entrada in ChangeTracker.Entries<ITemTenant>().ToList())
            {
                switch (entrada.State)
                {
                    case EntityState.Added:
                    case EntityState.Modified:
                        entrada.Entity.TenantId = TenantAtualId;
                        break;
                }
            }
        }

        public override int SaveChanges()
        {
            ApplyTenantFilter();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ApplyTenantFilter();
            return await base.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await SaveChangesAsync(default) > 0;
        }

        private void SeedUsers(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    IdUser = new Guid("11111111-1111-1111-1111-111111111111"),
                    Email = "admin@bros.com",
                    Cpf = "12345678901",
                    Nome = "Administrador Sistema",
                    Nascimento = new DateTime(1990, 1, 1),
                    Senha = "sRZL6wOaZHF0L6wOaZHF0L6wOaZHF0L6wOaZHF0L6wOaZHF=",
                    Genero = "Masculino",
                    DataCriacao = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    Ativo = true,
                    TenantId = "bros"
                },
                new User
                {
                    IdUser = new Guid("22222222-2222-2222-2222-222222222222"),
                    Email = "joao.silva@gmail.com",
                    Cpf = "98765432100",
                    Nome = "João Silva Santos",
                    Nascimento = new DateTime(1995, 5, 15),
                    Senha = "sRZL6wOaZHF0L6wOaZHF0L6wOaZHF0L6wOaZHF0L6wOaZHF=",
                    Genero = "Masculino",
                    DataCriacao = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    Ativo = true,
                    TenantId = "gamma"
                }
            );
        }

        private void SeedEstoque(ModelBuilder modelBuilder)
        {

        }
    }
}