using BROS_ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BROS_ECommerce.Infra.EntityConfig;

public class BannerMap : IEntityTypeConfiguration<Banner>
{
    public void Configure(EntityTypeBuilder<Banner> builder)
    {
        builder.HasKey(b => b.IdBanner);

        builder.Property(b => b.Titulo)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(b => b.Link)
            .HasMaxLength(400);

        builder.HasOne(b => b.Imagem)
            .WithMany()
            .HasForeignKey(b => b.IdImagem)
            .OnDelete(DeleteBehavior.SetNull);

        builder.ToTable("Banners");
    }
}
