using BROS_ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BROS_ECommerce.Infra.EntityConfig
{
    public class PedidoMap : IEntityTypeConfiguration<Pedido>
    {
        public void Configure(EntityTypeBuilder<Pedido> builder)
        {
            builder.ToTable("Pedidos");

            builder.HasKey(p => p.IdPedido);
            builder.Property(p => p.IdPedido)
                .HasColumnName("IdPedido")
                .ValueGeneratedOnAdd();

            builder.Property(p => p.IdUsuario)
                .HasColumnName("IdUsuario")
                .IsRequired();

            builder.Property(p => p.NumeroPedido)
                .HasColumnName("NumeroPedido")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(p => p.DataPedido)
                .HasColumnName("DataPedido")
                .HasColumnType("datetime2")
                .IsRequired();

            builder.Property(p => p.Status)
                .HasColumnName("Status")
                .HasColumnType("varchar(30)")
                .HasMaxLength(30)
                .HasDefaultValue("Pendente")
                .IsRequired();

            builder.Property(p => p.ValorSubtotal)
                .HasColumnName("ValorSubtotal")
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(p => p.ValorDesconto)
                .HasColumnName("ValorDesconto")
                .HasColumnType("decimal(18,2)")
                .HasDefaultValue(0)
                .IsRequired();

            builder.Property(p => p.ValorFrete)
                .HasColumnName("ValorFrete")
                .HasColumnType("decimal(18,2)")
                .HasDefaultValue(0)
                .IsRequired();

            builder.Property(p => p.ValorTotal)
                .HasColumnName("ValorTotal")
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(p => p.ObservacoesPedido)
                .HasColumnName("ObservacoesPedido")
                .HasColumnType("varchar(1000)")
                .HasMaxLength(1000);

            builder.Property(p => p.ObservacoesInternas)
                .HasColumnName("ObservacoesInternas")
                .HasColumnType("varchar(1000)")
                .HasMaxLength(1000);

            builder.Property(p => p.DataCancelamento)
                .HasColumnName("DataCancelamento")
                .HasColumnType("datetime2");

            builder.Property(p => p.MotivoCancelamento)
                .HasColumnName("MotivoCancelamento")
                .HasColumnType("varchar(500)")
                .HasMaxLength(500);

            builder.Property(p => p.DataCriacao)
                .HasColumnName("DataCriacao")
                .HasColumnType("datetime2")
                .HasDefaultValueSql("GETUTCDATE()")
                .IsRequired();

            builder.Property(p => p.DataAtualizacao)
                .HasColumnName("DataAtualizacao")
                .HasColumnType("datetime2");

            builder.HasOne(p => p.Usuario)
                .WithMany()
                .HasForeignKey(p => p.IdUsuario)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Pedidos_Usuario");

            builder.HasMany(p => p.PedidoItens)
                .WithOne(pi => pi.Pedido)
                .HasForeignKey(pi => pi.IdPedido)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(p => p.IdUsuario)
                .HasDatabaseName("IX_Pedidos_IdUsuario");

            builder.HasIndex(p => p.NumeroPedido)
                .IsUnique()
                .HasDatabaseName("IX_Pedidos_NumeroPedido");

            builder.HasIndex(p => p.DataPedido)
                .HasDatabaseName("IX_Pedidos_DataPedido");

            builder.HasIndex(p => p.Status)
                .HasDatabaseName("IX_Pedidos_Status");

            builder.HasIndex(p => p.DataCriacao)
                .HasDatabaseName("IX_Pedidos_DataCriacao");

            builder.HasIndex(p => new { p.IdUsuario, p.DataPedido })
                .HasDatabaseName("IX_Pedidos_UsuarioData");
        }
    }
}