

using BROS_ECommerce.Domain.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BROS_ECommerce.Domain.Entities
{
    public class User : IAggregateRoot, ITemTenant
    {
        public User() { }

        [Key]
        public Guid IdUser { get; set; } = Guid.NewGuid();
        
        public string TenantId { get; set; }

        [Required]
        [StringLength(255)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(14)] 
        public string Cpf { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string Nome { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "date")]
        public DateTime Nascimento { get; set; }

        [Required]
        [StringLength(500)] 
        public string Senha { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string Genero { get; set; } = string.Empty;

        [Column(TypeName = "datetime2")]
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

        [Column(TypeName = "datetime2")]
        public DateTime? DataAtualizacao { get; set; }

        public bool Ativo { get; set; } = true;

       
        [NotMapped]
        public int Idade => DateTime.Now.Year - Nascimento.Year -
            (DateTime.Now.DayOfYear < Nascimento.DayOfYear ? 1 : 0);

        [NotMapped]
        public string CpfFormatado =>
            Cpf.Length == 11 ?
                $"{Cpf.Substring(0, 3)}.{Cpf.Substring(3, 3)}.{Cpf.Substring(6, 3)}-{Cpf.Substring(9, 2)}" :
                Cpf;
    }
}