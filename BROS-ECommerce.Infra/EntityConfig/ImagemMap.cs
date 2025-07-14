using BROS_ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BROS_ECommerce.Infra.EntityConfig
{
    public class ImagemMap : IEntityTypeConfiguration<Imagem>
    {
        public void Configure(EntityTypeBuilder<Imagem> builder)
        {
            builder.ToTable("Imagens");

            builder.HasKey(i => i.IdImagem);
            builder.Property(i => i.IdImagem)
                .HasColumnName("IdImagem")
                .ValueGeneratedOnAdd();

            builder.Property(i => i.NomeArquivo)
                .HasColumnName("NomeArquivo")
                .HasColumnType("varchar(255)")
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(i => i.CaminhoArquivo)
                .HasColumnName("CaminhoArquivo")
                .HasColumnType("varchar(500)")
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(i => i.TamanhoArquivo)
                .HasColumnName("TamanhoArquivo")
                .HasColumnType("bigint")
                .IsRequired();

            builder.Property(i => i.TipoMime)
                .HasColumnName("TipoMime")
                .HasColumnType("varchar(100)")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(i => i.AltText)
                .HasColumnName("AltText")
                .HasColumnType("varchar(255)")
                .HasMaxLength(255);

            builder.Property(i => i.DataCriacao)
                .HasColumnName("DataCriacao")
                .HasColumnType("datetime2")
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(i => i.DataAtualizacao)
                .HasColumnName("DataAtualizacao")
                .HasColumnType("datetime2");

            builder.Property(i => i.Ativo)
                .HasColumnName("Ativo")
                .HasColumnType("bit")
                .IsRequired()
                .HasDefaultValue(true);

            
            builder.HasIndex(i => i.NomeArquivo)
                .HasDatabaseName("IX_Imagens_NomeArquivo");

            builder.HasIndex(i => i.DataCriacao)
                .HasDatabaseName("IX_Imagens_DataCriacao");

            builder.HasIndex(i => i.Ativo)
                .HasDatabaseName("IX_Imagens_Ativo");

            builder.HasIndex(i => i.TipoMime)
                .HasDatabaseName("IX_Imagens_TipoMime");
        }
    }
}