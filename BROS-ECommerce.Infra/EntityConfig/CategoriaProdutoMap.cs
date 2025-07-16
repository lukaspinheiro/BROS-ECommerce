using BROS_ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BROS_ECommerce.Infra.EntityConfig
{
    public class CategoriaProdutoMap : IEntityTypeConfiguration<CategoriaProduto>
    {
        public void Configure(EntityTypeBuilder<CategoriaProduto> builder)
        {
            builder.ToTable("CategoriaProdutos");

            builder.HasKey(cp => cp.IdCategoriaProduto);
            builder.Property(cp => cp.IdCategoriaProduto)
                .HasColumnName("IdCategoriaProduto")
                .ValueGeneratedOnAdd();

            builder.Property(cp => cp.IdCategoria)
                .HasColumnName("IdCategoria")
                .IsRequired();

            builder.Property(cp => cp.IdProduto)
                .HasColumnName("IdProduto")
                .IsRequired();

            builder.Property(cp => cp.DataAssociacao)
                .HasColumnName("DataAssociacao")
                .HasColumnType("datetime2")
                .HasDefaultValueSql("GETUTCDATE()")
                .IsRequired();

            builder.HasOne(cp => cp.Categoria)
                .WithMany(c => c.CategoriaProdutos)
                .HasForeignKey(cp => cp.IdCategoria)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_CategoriaProdutos_Categoria");

            builder.HasOne(cp => cp.Produto)
                .WithMany(p => p.CategoriaProdutos)
                .HasForeignKey(cp => cp.IdProduto)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_CategoriaProdutos_Produto");

            builder.HasIndex(cp => cp.IdCategoria)
                .HasDatabaseName("IX_CategoriaProdutos_IdCategoria");

            builder.HasIndex(cp => cp.IdProduto)
                .HasDatabaseName("IX_CategoriaProdutos_IdProduto");

            builder.HasIndex(cp => new { cp.IdCategoria, cp.IdProduto })
                .IsUnique()
                .HasDatabaseName("IX_CategoriaProdutos_CategoriasProduto");

            builder.HasIndex(cp => cp.DataAssociacao)
                .HasDatabaseName("IX_CategoriaProdutos_DataAssociacao");
        }
    }
}