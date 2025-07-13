using BROS_ECommerce.Services.ViewModel.Imagem;

namespace BROS_ECommerce.Services.ViewModel.Produto
{
    public class ProdutoViewModel
    {
        public Guid IdProduto { get; set; }
        public string Nome { get; set; }
        public string Slug { get; set; }
        public string TituloDescricao { get; set; }
        public string Descricao { get; set; }
        public decimal Preco { get; set; }
        public List<string> Imagens { get; set; } = new();
        public List<ImagemViewModel> ImagensDetalhadas { get; set; } = new();
    }

}
