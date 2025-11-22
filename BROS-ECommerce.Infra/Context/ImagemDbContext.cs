using BROS_ECommerce.Domain.Entities;
using BROS_ECommerce.Infra.EntityConfig;
using Microsoft.EntityFrameworkCore;

namespace BROS_ECommerce.Infra.Context;

public class ImagemDbContext : DbContext
{
    public ImagemDbContext(DbContextOptions<ImagemDbContext> options)
        : base(options) { }

    public DbSet<Imagem> Imagens { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Model.GetEntityTypes()
        .Where(t => t.ClrType != typeof(Imagem))
        .ToList()
        .ForEach(t => modelBuilder.Ignore(t.ClrType));

        modelBuilder.ApplyConfiguration(new ImagemMap());
    }


}
