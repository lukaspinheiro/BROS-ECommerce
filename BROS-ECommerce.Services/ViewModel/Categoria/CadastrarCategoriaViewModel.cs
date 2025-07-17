using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BROS_ECommerce.Services.ViewModel.Categoria
{
    public class CadastrarCategoriaViewModel
    {

        [Required(ErrorMessage = "O nome da categoria é obrigatório."), Display(Name = "Nome da Categoria")]
        [StringLength(100, ErrorMessage = "O nome deve ter até 100 caracteres.")]
        public string NomeCategoria { get; set; } = string.Empty;

        [Required(ErrorMessage = "O nome da categoria é obrigatório."), StringLength(255, ErrorMessage = "A descrição deve ter até 255 caracteres."), Display(Name = "Descrição")]
        public string? Descricao { get; set; }

        public bool Ativo { get; set; } = true;

        [NotMapped]
        public Guid IdCategoria { get; set; }
    }
}
