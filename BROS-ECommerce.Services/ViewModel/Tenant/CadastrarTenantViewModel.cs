using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace BROS_ECommerce.Services.ViewModel.Tenant;

public class CadastrarTenantViewModel
{
    public CadastrarTenantViewModel()
    {
        
    }
    [Required, Display(Name = "Domínio")]
    public string Id { get; set; }

    [Required, Display(Name = "Nome"), MaxLength(50, ErrorMessage = "O Nome ultrapassa 50 caracteres.")]
    [RegularExpression(@"^(?!\s*$).+", ErrorMessage = "Este campo não pode ser nulo.")]
    public string Nome { get; set; }
    [Required, Display(Name = "Email")]
    public string Email { get; set; }
    [Required, Display(Name = "Telefone")]
    public string Telefone { get; set; }


    #region Identidade Visual

    [Display(Name = "Cor do Menu")]
    public string CorMenu { get; set; }

    [Display(Name = "Cor do Texto do Menu")]
    public string CorTextoMenu { get; set; }

    [Display(Name = "Cor do Menu Inferior")]
    public string CorMenuInferior { get; set; }

    [Display(Name = "Cor do Texto do Menu Inferior")]
    public string CorTextoMenuInferior { get; set; }

    [Display(Name = "Cor de Fundo do Site")]
    public string CorFundo { get; set; }

    [Display(Name = "Cor do Texto do Site")]
    public string CorTexto { get; set; }

    public Guid? IdLogo { get; set; }
    public IFormFile LogoArquivo { get; set; }

    public Guid? IdFavicon { get; set; }
    #endregion

    #region Controle Interno
    [Display(Name = "Data de Criação")]
    public DateTime DataCriacao { get; set; }

    [Display(Name = "Status")]
    public bool Ativo { get; set; }
    #endregion

}
