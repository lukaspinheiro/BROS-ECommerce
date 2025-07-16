using System.ComponentModel.DataAnnotations;

namespace BROS_ECommerce.Services.ViewModel.Categoria
{
    public class CadastrarCategoriaViewModel
    {
        [Required(ErrorMessage = "O nome da categoria é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome deve ter até 100 caracteres.")]
        public string NomeCategoria { get; set; } = string.Empty;

        [StringLength(255, ErrorMessage = "A descrição deve ter até 255 caracteres.")]
        public string? Descricao { get; set; }

        public bool Ativo { get; set; } = true;
    }
}
