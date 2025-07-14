namespace BROS_ECommerce.Services.ViewModel.Produto
{
    public class TabelaProdutoViewModel
    {
        public TabelaProdutoViewModel()
        {
            Produto = new List<ProdutoViewModel>();
            Imagens = new List<string>();
        }

        public TabelaProdutoViewModel(Guid idProduto, string nome, string slug, string tituloDescricao, string descricao, decimal preco)
        {
            IdProduto = idProduto;
            Nome = nome;
            Slug = slug;
            TituloDescricao = tituloDescricao;
            Descricao = descricao;
            Preco = preco;
            Produto = new List<ProdutoViewModel>();
            Imagens = new List<string>();
        }

        public List<ProdutoViewModel> Produto { get; set; }
        public Guid IdProduto { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string TituloDescricao { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public decimal Preco { get; set; }

       
        public List<string> Imagens { get; set; }
        public string? ImagemPrincipal { get; set; }
        public bool TemImagem => !string.IsNullOrEmpty(ImagemPrincipal);
    }
}