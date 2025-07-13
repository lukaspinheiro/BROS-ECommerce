namespace BROS_ECommerce.Services.ViewModel.Produto
{
    public class IndexProdutoViewModel
    {
        public IndexProdutoViewModel()
        {
            Filtro = new FiltroProdutoViewModel();
            Tabela = new List<TabelaProdutoViewModel>();
            cadastrarProdutoViewModel = new CadastrarProdutoViewModel();
        }

        public IndexProdutoViewModel(FiltroProdutoViewModel filtro, List<TabelaProdutoViewModel> tabela)
        {
            Filtro = filtro;
            Tabela = tabela;
            cadastrarProdutoViewModel = new CadastrarProdutoViewModel();
        }

        public FiltroProdutoViewModel Filtro { get; set; } = new FiltroProdutoViewModel();
        public ICollection<TabelaProdutoViewModel> Tabela { get; set; } = new List<TabelaProdutoViewModel>();
        public CadastrarProdutoViewModel cadastrarProdutoViewModel { get; set; } = new CadastrarProdutoViewModel();
        public int IndiceImagemPrincipal { get; set; }
    }
}