using BROS_ECommerce.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace BROS_ECommerce.Services.ViewModel.Usuario
{
    public class CadastrarUsuarioViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MaxLength(14)]
        public string Cpf { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nome { get; set; }

        [Required]
        public DateTime Nascimento { get; set; }

        [Required]
        [MinLength(6)]
        public string Senha { get; set; }

        [Required]
        public string Genero { get; set; }

        [Required]
        public EPerfil Perfil { get; set; } = EPerfil.TenantUser;
    }
}
