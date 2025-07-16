using BROS_ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BROS_ECommerce.Infra.EntityConfig
{
    public class CategoriaMap : IEntityTypeConfiguration<Categoria>
    {
        public void Configure(EntityTypeBuilder<Categoria> builder)
        {
            builder.ToTable("Categorias");

            builder.HasKey(c => c.IdCategoria);
            builder.Property(c => c.IdCategoria)
                .HasColumnName("IdCategoria")
                .ValueGeneratedOnAdd();

            builder.Property(c => c.NomeCategoria)
                .HasColumnName("NomeCategoria")
                .HasColumnType("varchar(100)")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(c => c.Descricao)
                .HasColumnName("Descricao")
                .HasColumnType("varchar(500)")
                .HasMaxLength(500);

            builder.Property(c => c.Ativo)
                .HasColumnName("Ativo")
                .HasDefaultValue(true)
                .IsRequired();

            builder.Property(c => c.DataCriacao)
                .HasColumnName("DataCriacao")
                .HasColumnType("datetime2")
                .HasDefaultValueSql("GETUTCDATE()")
                .IsRequired();

            builder.Property(c => c.DataAtualizacao)
                .HasColumnName("DataAtualizacao")
                .HasColumnType("datetime2");

            builder.HasIndex(c => c.NomeCategoria)
                .HasDatabaseName("IX_Categorias_NomeCategoria");

            builder.HasIndex(c => c.Ativo)
                .HasDatabaseName("IX_Categorias_Ativo");

            builder.HasIndex(c => c.DataCriacao)
                .HasDatabaseName("IX_Categorias_DataCriacao");

            builder.HasMany(c => c.CategoriaProdutos)
                .WithOne(cp => cp.Categoria)
                .HasForeignKey(cp => cp.IdCategoria)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}