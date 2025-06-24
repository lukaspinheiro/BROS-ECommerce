

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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.ApplyConfiguration(new ProdutoMap());
            modelBuilder.ApplyConfiguration(new UserMap());

            
            SeedUsers(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.LogTo(
                action: Console.WriteLine,
                minimumLevel: LogLevel.Information);
        }

        public async Task<bool> SaveChangesAsync()
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
                    Senha = "AQAAAAEAACcQAAAAEJ8D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D", 
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
                    Senha = "AQAAAAEAACcQAAAAEJ8D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D", 
                    Genero = "Masculino",
                    DataCriacao = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc), 
                    Ativo = true
                },
                new User
                {
                    IdUser = new Guid("33333333-3333-3333-3333-333333333333"),
                    Email = "maria.oliveira@hotmail.com",
                    Cpf = "45678912300",
                    Nome = "Maria Oliveira Costa",
                    Nascimento = new DateTime(1998, 8, 22),
                    Senha = "AQAAAAEAACcQAAAAEJ8D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D",
                    Genero = "Feminino",
                    DataCriacao = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc), 
                    Ativo = true
                },
                new User
                {
                    IdUser = new Guid("44444444-4444-4444-4444-444444444444"),
                    Email = "alex.santos@outlook.com",
                    Cpf = "78912345600",
                    Nome = "Alex Santos Lima",
                    Nascimento = new DateTime(2000, 12, 10),
                    Senha = "AQAAAAEAACcQAAAAEJ8D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D", 
                    Genero = "Outro",
                    DataCriacao = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc), 
                    Ativo = true
                }
            );
        }
    }
}