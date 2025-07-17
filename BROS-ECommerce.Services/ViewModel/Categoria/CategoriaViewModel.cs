using BROS_ECommerce.Services.ViewModel.CategoriaProduto;

namespace BROS_ECommerce.Services.ViewModel.Categoria
{
    public class CategoriaViewModel
    {
        public Guid IdCategoria { get; set; }
        public string NomeCategoria { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public bool Ativo { get; set; } = true;
        public DateTime DataCriacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }

        public ICollection<CategoriaProdutoViewModel> CategoriaProdutos { get; set; } = new List<CategoriaProdutoViewModel>();
    }
}
