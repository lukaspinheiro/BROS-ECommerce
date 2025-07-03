using BROS_ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BROS_ECommerce.Infra.EntityConfig
{
    public class EstoqueMap : IEntityTypeConfiguration<Estoque>
    {
        public void Configure(EntityTypeBuilder<Estoque> builder)
        {
            builder.ToTable("Estoque");

            builder.HasKey(e => e.IdEstoque);
            builder.Property(e => e.IdEstoque)
                .HasColumnName("IdEstoque")
                .ValueGeneratedOnAdd();

            builder.Property(e => e.IdProduto)
                .HasColumnName("IdProduto")
                .IsRequired();

            builder.Property(e => e.Quantidade)
                .HasColumnName("Quantidade")
                .HasColumnType("int")
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(e => e.UltimaAtualizacao)
                .HasColumnName("UltimaAtualizacao")
                .HasColumnType("datetime2")
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            builder.HasOne(e => e.Produto)
                .WithMany()
                .HasForeignKey(e => e.IdProduto)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Estoque_Produto");

            builder.HasIndex(e => e.IdProduto)
                .IsUnique()
                .HasDatabaseName("IX_Estoque_IdProduto");

            builder.HasIndex(e => e.UltimaAtualizacao)
                .HasDatabaseName("IX_Estoque_UltimaAtualizacao");

            builder.HasIndex(e => e.Quantidade)
                .HasDatabaseName("IX_Estoque_Quantidade");
        }
    }
}