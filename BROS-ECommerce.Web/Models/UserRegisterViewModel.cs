
using BROS_ECommerce.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace BROS_ECommerce.Web.ViewModels
{
    public class UserRegisterViewModel
    {
        [Required(ErrorMessage = "Email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "CPF é obrigatório")]
        [StringLength(14, MinimumLength = 11, ErrorMessage = "CPF deve ter entre 11 e 14 caracteres")]
        [Display(Name = "CPF")]
        public string Cpf { get; set; } = string.Empty;

        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Nome deve ter entre 2 e 200 caracteres")]
        [Display(Name = "Nome Completo")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "Data de nascimento é obrigatória")]
        [DataType(DataType.Date)]
        [Display(Name = "Data de Nascimento")]
        public DateTime Nascimento { get; set; }

        [Required(ErrorMessage = "Senha é obrigatória")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Senha deve ter pelo menos 6 caracteres")]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Senha { get; set; } = string.Empty;

        [Required(ErrorMessage = "Gênero é obrigatório")]
        [Display(Name = "Gênero")]
        public string Genero { get; set; } = string.Empty;

        [Required]
        public EPerfil Perfil { get; set; } = EPerfil.TenantUser;
        public bool IsValidAge()
        {
            var age = DateTime.Now.Year - Nascimento.Year;
            if (DateTime.Now.DayOfYear < Nascimento.DayOfYear) age--;
            return age >= 16;
        }

        public string GetCleanCpf()
        {
            return new string(Cpf.Where(char.IsDigit).ToArray());
        }
    }
}