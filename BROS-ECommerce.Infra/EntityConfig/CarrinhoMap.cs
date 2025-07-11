

using BROS_ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BROS_ECommerce.Infra.EntityConfig
{
    public class CarrinhoMap : IEntityTypeConfiguration<Carrinho>
    {
        public void Configure(EntityTypeBuilder<Carrinho> builder)
        {
            builder.ToTable("Carrinhos");

            builder.HasKey(c => c.IdCarrinho);
            builder.Property(c => c.IdCarrinho)
                .ValueGeneratedOnAdd()
                .HasColumnName("IdCarrinho");

            builder.Property(c => c.IdUsuario)
                .HasColumnName("IdUsuario")
                .IsRequired(false);

            builder.Property(c => c.DataCriacao)
                .IsRequired()
                .HasColumnType("datetime2")
                .HasColumnName("DataCriacao")
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(c => c.Status)
                .IsRequired()
                .HasMaxLength(20)
                .HasColumnType("varchar(20)")
                .HasColumnName("Status")
                .HasDefaultValue("Aberto");

            
            builder.HasOne(c => c.Usuario)
                .WithMany()
                .HasForeignKey(c => c.IdUsuario)
                .IsRequired(false)
                .HasConstraintName("FK_Carrinhos_Usuario");

            builder.HasMany(c => c.Itens)
                .WithOne(i => i.Carrinho)
                .HasForeignKey(i => i.IdCarrinho)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_CarrinhoItens_Carrinho");

            
            builder.HasIndex(c => c.IdUsuario)
                .HasDatabaseName("IX_Carrinhos_IdUsuario");

            builder.HasIndex(c => c.DataCriacao)
                .HasDatabaseName("IX_Carrinhos_DataCriacao");

            builder.HasIndex(c => c.Status)
                .HasDatabaseName("IX_Carrinhos_Status");
        }
    }
}