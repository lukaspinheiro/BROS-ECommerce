
using BROS_ECommerce.Domain.Entities;
using BROS_ECommerce.Domain.Interfaces.Crud;
using BROS_ECommerce.Infra.EntityConfig;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace BROS_ECommerce.Infra.Context
{
    public class BrosContext : DbContext, IUnitOfWork
    {
        public IConfiguration _configuration { get; }

        public BrosContext(DbContextOptions<BrosContext> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }

        public DbSet<Produto> Produtos { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Estoque> Estoque { get; set; }
        public DbSet<Imagem> Imagens { get; set; }
        public DbSet<ProdutoImagem> ProdutoImagens { get; set; }

        
        public DbSet<Carrinho> Carrinhos { get; set; }
        public DbSet<CarrinhoItem> CarrinhoItens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.ApplyConfiguration(new ProdutoMap());
            modelBuilder.ApplyConfiguration(new UserMap());
            modelBuilder.ApplyConfiguration(new EstoqueMap());
            modelBuilder.ApplyConfiguration(new ImagemMap());
            modelBuilder.ApplyConfiguration(new ProdutoImagemMap());

            
            modelBuilder.ApplyConfiguration(new CarrinhoMap());
            modelBuilder.ApplyConfiguration(new CarrinhoItemMap());

            SeedUsers(modelBuilder);
            SeedEstoque(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.LogTo(
                action: Console.WriteLine,
                minimumLevel: LogLevel.Information);
        }

        public new async Task<bool> SaveChangesAsync()
        {
            return await base.SaveChangesAsync() > 0;
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
                    Ativo = true
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
                    Ativo = true
                }
            );
        }
        
        private void SeedEstoque(ModelBuilder modelBuilder)
        {
           
        }
    }
}