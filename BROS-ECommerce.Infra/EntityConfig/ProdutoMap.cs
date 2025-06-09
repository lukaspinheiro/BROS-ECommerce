using BROS_ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BROS_ECommerce.Infra.EntityConfig
{
    public class ProdutoMap : IEntityTypeConfiguration<Produto>
    {
        public void Configure(EntityTypeBuilder<Produto> builder)
        {
            builder.ToTable("Produtos");

            builder.HasKey(p => p.IdProduto);
            builder.Property(p => p.IdProduto)
                .HasColumnName("IdProduto")
                .ValueGeneratedOnAdd();

            builder.Property(p => p.Nome)
                .HasColumnName("Nome")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(p => p.Slug)
                .HasColumnName("Slug")
                .HasColumnType("varchar(30)")
                .HasMaxLength(30)
                .IsRequired();

            builder.Property(p => p.TituloDescricao)
                .HasColumnName("TituloDescricao")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(p => p.Descricao)
                .HasColumnName("Descricao")
                .HasColumnType("varchar(300)")
                .HasMaxLength(300)
                .IsRequired();

            builder.Property(p => p.Preco)
                .HasColumnName("Preco")
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.HasIndex(p => p.Slug)
                .IsUnique()
                .HasDatabaseName("IX_Produtos_Slug");

            builder.HasIndex(p => p.Nome)
                .HasDatabaseName("IX_Produtos_Nome");
        }
    }
}