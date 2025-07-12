using BROS_ECommerce.Services.ViewModel.Produto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BROS_ECommerce.Services.Interface.Services
{
    public interface IServiceProduto
    {
        
        List<ProdutoViewModel> ObterTodos();
        ProdutoViewModel? ObterPorSlug(string slug);

        
        Task<List<ProdutoViewModel>> ObterTodosAsync();
        Task<List<TabelaProdutoViewModel>> ObterTabelaProdutosAsync();
        Task<ProdutoViewModel?> ObterPorIdAsync(Guid id);
        Task<ProdutoViewModel?> ObterPorSlugAsync(string slug);
        Task Adicionar(CadastrarProdutoViewModel produtoVM);
        Task AtualizarAsync(ProdutoViewModel produtoVM);
        Task ExcluirAsync(Guid id);
        Task<IEnumerable<ProdutoViewModel>> BuscarPorTermoAsync(string termo);
        Task AdicionarComImagensAsync(IndexProdutoViewModel indexProdutoViewModel);
        Task PopularDadosIniciais();
    }
}