using BROS_ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BROS_ECommerce.Infra.EntityConfig
{
    public class PromocaoMap : IEntityTypeConfiguration<Promocao>
    {
        public void Configure(EntityTypeBuilder<Promocao> builder)
        {
            builder.ToTable("Promocoes");

            builder.HasKey(p => p.IdPromocao);
            builder.Property(p => p.IdPromocao)
                .HasColumnName("IdPromocao")
                .ValueGeneratedOnAdd();

            builder.Property(p => p.IdProduto)
                .HasColumnName("IdProduto")
                .IsRequired();

            builder.Property(p => p.Nome)
                .HasColumnName("Nome")
                .HasColumnType("varchar(100)")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(p => p.Descricao)
                .HasColumnName("Descricao")
                .HasColumnType("varchar(500)")
                .HasMaxLength(500);

            builder.Property(p => p.PercentualDesconto)
                .HasColumnName("PercentualDesconto")
                .HasColumnType("decimal(5,2)")
                .IsRequired();

            builder.Property(p => p.ValorDesconto)
                .HasColumnName("ValorDesconto")
                .HasColumnType("decimal(18,2)");

            builder.Property(p => p.DataInicio)
                .HasColumnName("DataInicio")
                .HasColumnType("datetime2")
                .IsRequired();

            builder.Property(p => p.DataFim)
                .HasColumnName("DataFim")
                .HasColumnType("datetime2")
                .IsRequired();

            builder.Property(p => p.Ativo)
                .HasColumnName("Ativo")
                .HasDefaultValue(true)
                .IsRequired();

            builder.Property(p => p.DataCriacao)
                .HasColumnName("DataCriacao")
                .HasColumnType("datetime2")
                .HasDefaultValueSql("GETUTCDATE()")
                .IsRequired();

            builder.Property(p => p.DataAtualizacao)
                .HasColumnName("DataAtualizacao")
                .HasColumnType("datetime2");

            builder.HasOne(p => p.Produto)
                .WithMany(pr => pr.Promocoes)
                .HasForeignKey(p => p.IdProduto)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Promocoes_Produto");

            builder.HasIndex(p => p.IdProduto)
                .HasDatabaseName("IX_Promocoes_IdProduto");

            builder.HasIndex(p => p.DataInicio)
                .HasDatabaseName("IX_Promocoes_DataInicio");

            builder.HasIndex(p => p.DataFim)
                .HasDatabaseName("IX_Promocoes_DataFim");

            builder.HasIndex(p => p.Ativo)
                .HasDatabaseName("IX_Promocoes_Ativo");

            builder.HasIndex(p => new { p.IdProduto, p.DataInicio, p.DataFim, p.Ativo })
                .HasDatabaseName("IX_Promocoes_ProdutoVigencia");
        }
    }
}