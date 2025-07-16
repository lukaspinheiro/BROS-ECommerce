namespace BROS_ECommerce.Services.ViewModel.CategoriaProduto
{
    public class CategoriaProdutoViewModel
    {
        public Guid IdCategoriaProduto { get; set; }
        public Guid IdCategoria { get; set; }
        public Guid IdProduto { get; set; }
        public DateTime DataAssociacao { get; set; }
        public string NomeCategoria { get; set; } = string.Empty;
        public string NomeProduto { get; set; } = string.Empty;
    }

}
