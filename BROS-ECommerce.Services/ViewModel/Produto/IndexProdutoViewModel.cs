
using System.Collections.Generic;

namespace BROS_ECommerce.Services.ViewModel.Produto
{
    public class IndexProdutoViewModel
    {
        public IndexProdutoViewModel()
        {
        }
        public IndexProdutoViewModel(FiltroProdutoViewModel filtro, List<TabelaProdutoViewModel> tabela)
        {
            Filtro = filtro;
            Tabela = tabela;
            cadastrarProdutoViewModel = new CadastrarProdutoViewModel();
        }
        public FiltroProdutoViewModel Filtro { get; set; } = new FiltroProdutoViewModel();

        public ICollection<TabelaProdutoViewModel> Tabela {  get; set; }

        public CadastrarProdutoViewModel cadastrarProdutoViewModel { get; set; } = new CadastrarProdutoViewModel();
             
    }
}
