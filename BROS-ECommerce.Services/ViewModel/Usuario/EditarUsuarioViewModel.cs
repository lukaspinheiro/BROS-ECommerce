using System;
using System.ComponentModel.DataAnnotations;

namespace BROS_ECommerce.Services.ViewModel.Usuario
{
    public class EditarUsuarioViewModel
    {
        public Guid IdUser { get; set; }

        [Required]
        [Display(Name = "Nome")]
        public string Nome { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Display(Name = "CPF")]
        public string Cpf { get; set; }

        [Required]
        public DateTime Nascimento { get; set; }

        [Required]
        public string Genero { get; set; }

        public bool Ativo { get; set; }
    }
}
