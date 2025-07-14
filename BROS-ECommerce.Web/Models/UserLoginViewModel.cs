
using System.ComponentModel.DataAnnotations;

namespace BROS_ECommerce.Web.ViewModels
{
    public class UserLoginViewModel
    {
        [Required(ErrorMessage = "Email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        [Display(Name = "Email")]
        public string Usuario { get; set; } = string.Empty; 

        [Required(ErrorMessage = "Senha é obrigatória")]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Senha { get; set; } = string.Empty;

        [Display(Name = "Lembrar-me")]
        public bool LembrarSenha { get; set; }
    }
}