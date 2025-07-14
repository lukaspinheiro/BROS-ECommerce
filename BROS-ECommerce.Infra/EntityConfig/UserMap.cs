

using BROS_ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BROS_ECommerce.Infra.EntityConfig
{
    public class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            
            builder.ToTable("Users");

            
            builder.HasKey(u => u.IdUser);
            builder.Property(u => u.IdUser)
                .ValueGeneratedOnAdd();

            
            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(255);
            builder.HasIndex(u => u.Email)
                .IsUnique()
                .HasDatabaseName("IX_Users_Email");

            
            builder.Property(u => u.Cpf)
                .IsRequired()
                .HasMaxLength(14);
            builder.HasIndex(u => u.Cpf)
                .IsUnique()
                .HasDatabaseName("IX_Users_Cpf");

            
            builder.Property(u => u.Nome)
                .IsRequired()
                .HasMaxLength(200);

            
            builder.Property(u => u.Nascimento)
                .IsRequired()
                .HasColumnType("date");

            
            builder.Property(u => u.Senha)
                .IsRequired()
                .HasMaxLength(500);

            
            builder.Property(u => u.Genero)
                .IsRequired()
                .HasMaxLength(20);

            
            builder.Property(u => u.DataCriacao)
                .IsRequired()
                .HasColumnType("datetime2")
                .HasDefaultValueSql("GETUTCDATE()");

            
            builder.Property(u => u.DataAtualizacao)
                .HasColumnType("datetime2");

            
            builder.Property(u => u.Ativo)
                .IsRequired()
                .HasDefaultValue(true);

            
            builder.HasIndex(u => u.DataCriacao)
                .HasDatabaseName("IX_Users_DataCriacao");

            builder.HasIndex(u => u.Ativo)
                .HasDatabaseName("IX_Users_Ativo");
        }
    }
}