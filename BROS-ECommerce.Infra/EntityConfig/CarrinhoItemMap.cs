
using BROS_ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BROS_ECommerce.Infra.EntityConfig
{
    public class CarrinhoItemMap : IEntityTypeConfiguration<CarrinhoItem>
    {
        public void Configure(EntityTypeBuilder<CarrinhoItem> builder)
        {
            builder.ToTable("CarrinhoItens");

            builder.HasKey(ci => ci.IdCarrinhoItem);
            builder.Property(ci => ci.IdCarrinhoItem)
                .ValueGeneratedOnAdd()
                .HasColumnName("IdCarrinhoItem");

            builder.Property(ci => ci.IdCarrinho)
                .IsRequired()
                .HasColumnName("IdCarrinho");

            builder.Property(ci => ci.IdProduto)
                .IsRequired()
                .HasColumnName("IdProduto");

            builder.Property(ci => ci.Quantidade)
                .IsRequired()
                .HasColumnName("Quantidade")
                .HasDefaultValue(1);

            builder.Property(ci => ci.PrecoUnitario)
                .IsRequired()
                .HasColumnType("decimal(18,2)")
                .HasColumnName("PrecoUnitario");

            
            builder.HasOne(ci => ci.Carrinho)
                .WithMany(c => c.Itens)
                .HasForeignKey(ci => ci.IdCarrinho)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_CarrinhoItens_Carrinho");

            builder.HasOne(ci => ci.Produto)
                .WithMany()
                .HasForeignKey(ci => ci.IdProduto)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_CarrinhoItens_Produto");

            
            builder.HasIndex(ci => ci.IdCarrinho)
                .HasDatabaseName("IX_CarrinhoItens_IdCarrinho");

            builder.HasIndex(ci => ci.IdProduto)
                .HasDatabaseName("IX_CarrinhoItens_IdProduto");

            builder.HasIndex(ci => new { ci.IdCarrinho, ci.IdProduto })
                .IsUnique()
                .HasDatabaseName("IX_CarrinhoItens_CarrinhoProduto");
        }
    }
}