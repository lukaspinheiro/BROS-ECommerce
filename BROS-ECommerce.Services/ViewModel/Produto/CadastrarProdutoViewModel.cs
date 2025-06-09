using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BROS_ECommerce.Services.ViewModel.Produto;

public class CadastrarProdutoViewModel
{
    public CadastrarProdutoViewModel()
    {

    }
    public Guid IdProduto { get; set; }


    [Required, Display(Name = "Nome"), MaxLength(50, ErrorMessage = "O Nome ultrapassa 50 caracteres.")]
    [RegularExpression(@"^(?!\s*$).+", ErrorMessage = "Este campo não pode ser nulo.")]
    public string Nome { get; set; }

    [Required, Display(Name = "Caminho URL"), MaxLength(30, ErrorMessage = "O Caminho da URL ultrapassa 30 caracteres.")]
    [RegularExpression(@"^(?!\s*$).+", ErrorMessage = "Este campo não pode ser nulo.")]
    public string Slug { get; set; }


    [Required, Display(Name = "Título"), MaxLength(50, ErrorMessage = "O Título da descrição ultrapassa 50 caracteres.")]
    [RegularExpression(@"^(?!\s*$).+", ErrorMessage = "Este campo não pode ser nulo.")]
    public string TituloDescricao { get; set; }


    [Required, Display(Name = "Descricao"), MaxLength(300, ErrorMessage = "A Descrição ultrapassa 300 caracteres.")]
    [RegularExpression(@"^(?!\s*$).+", ErrorMessage = "Este campo não pode ser nulo.")]
    public string Descricao { get; set; }


    [Required, Display(Name = "Preço")]
    public decimal Preco { get; set; }
}
