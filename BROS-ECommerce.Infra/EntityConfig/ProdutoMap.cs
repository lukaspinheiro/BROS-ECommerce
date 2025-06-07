using BROS_ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace BROS_ECommerce.Infra.EntityConfig
{
    public class ProdutoMap : IEntityTypeConfiguration<Produto>
    {
        public void Configure(EntityTypeBuilder<Produto> builder)
        {
            builder.ToTable("Produto");

            builder.HasKey(b => b.IdProduto);
            builder.Property(b => b.IdProduto).ValueGeneratedOnAdd();

            //builder.Property(b => b.)
            //    .IsRequired();

            builder.Property(b => b.Nome)
                .HasColumnType("varchar")
                .HasMaxLength(250)
                .IsRequired();

            builder.Property(b => b.Descricao)
                .HasColumnType("varchar")
                .HasMaxLength(250)
                .IsRequired();

        }
    }
}
