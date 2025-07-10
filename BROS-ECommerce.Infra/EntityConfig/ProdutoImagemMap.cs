using BROS_ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BROS_ECommerce.Infra.EntityConfig
{
    public class ProdutoImagemMap : IEntityTypeConfiguration<ProdutoImagem>
    {
        public void Configure(EntityTypeBuilder<ProdutoImagem> builder)
        {
            builder.ToTable("ProdutoImagens");

            builder.HasKey(pi => pi.IdProdutoImagem);
            builder.Property(pi => pi.IdProdutoImagem)
                .HasColumnName("IdProdutoImagem")
                .ValueGeneratedOnAdd();

            builder.Property(pi => pi.IdProduto)
                .HasColumnName("IdProduto")
                .IsRequired();

            builder.Property(pi => pi.IdImagem)
                .HasColumnName("IdImagem")
                .IsRequired();

            builder.Property(pi => pi.Ordem)
                .HasColumnName("Ordem")
                .HasColumnType("int")
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(pi => pi.Principal)
                .HasColumnName("Principal")
                .HasColumnType("bit")
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(pi => pi.DataAssociacao)
                .HasColumnName("DataAssociacao")
                .HasColumnType("datetime2")
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            
            builder.HasOne(pi => pi.Produto)
                .WithMany(p => p.ProdutoImagens)
                .HasForeignKey(pi => pi.IdProduto)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_ProdutoImagens_Produto");

            builder.HasOne(pi => pi.Imagem)
                .WithMany(i => i.ProdutoImagens)
                .HasForeignKey(pi => pi.IdImagem)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_ProdutoImagens_Imagem");

            
            builder.HasIndex(pi => pi.IdProduto)
                .HasDatabaseName("IX_ProdutoImagens_IdProduto");

            builder.HasIndex(pi => pi.IdImagem)
                .HasDatabaseName("IX_ProdutoImagens_IdImagem");

            builder.HasIndex(pi => new { pi.IdProduto, pi.Principal })
                .HasDatabaseName("IX_ProdutoImagens_IdProduto_Principal");

            builder.HasIndex(pi => new { pi.IdProduto, pi.Ordem })
                .HasDatabaseName("IX_ProdutoImagens_IdProduto_Ordem");

            
            builder.HasIndex(pi => new { pi.IdProduto, pi.Principal })
                .HasDatabaseName("IX_ProdutoImagens_UnicaPrincipal")
                .HasFilter("Principal = 1");
        }
    }
}