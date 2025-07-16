using BROS_ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BROS_ECommerce.Infra.EntityConfig
{
    public class PedidoItemMap : IEntityTypeConfiguration<PedidoItem>
    {
        public void Configure(EntityTypeBuilder<PedidoItem> builder)
        {
            builder.ToTable("PedidoItens");

            builder.HasKey(pi => pi.IdPedidoItem);
            builder.Property(pi => pi.IdPedidoItem)
                .HasColumnName("IdPedidoItem")
                .ValueGeneratedOnAdd();

            builder.Property(pi => pi.IdPedido)
                .HasColumnName("IdPedido")
                .IsRequired();

            builder.Property(pi => pi.IdProduto)
                .HasColumnName("IdProduto")
                .IsRequired();

            builder.Property(pi => pi.Quantidade)
                .HasColumnName("Quantidade")
                .IsRequired();

            builder.Property(pi => pi.PrecoUnitario)
                .HasColumnName("PrecoUnitario")
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(pi => pi.ValorDesconto)
                .HasColumnName("ValorDesconto")
                .HasColumnType("decimal(18,2)")
                .HasDefaultValue(0)
                .IsRequired();

            builder.Property(pi => pi.ValorTotal)
                .HasColumnName("ValorTotal")
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.HasOne(pi => pi.Pedido)
                .WithMany(p => p.PedidoItens)
                .HasForeignKey(pi => pi.IdPedido)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_PedidoItens_Pedido");

            builder.HasOne(pi => pi.Produto)
                .WithMany()
                .HasForeignKey(pi => pi.IdProduto)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_PedidoItens_Produto");

            builder.HasIndex(pi => pi.IdPedido)
                .HasDatabaseName("IX_PedidoItens_IdPedido");

            builder.HasIndex(pi => pi.IdProduto)
                .HasDatabaseName("IX_PedidoItens_IdProduto");

            builder.HasIndex(pi => new { pi.IdPedido, pi.IdProduto })
                .HasDatabaseName("IX_PedidoItens_PedidoProduto");
        }
    }
}