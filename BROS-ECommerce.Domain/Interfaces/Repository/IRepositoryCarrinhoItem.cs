using BROS_ECommerce.Domain.Entities;

namespace BROS_ECommerce.Domain.Interfaces.Repository
{
    public interface IRepositoryCarrinhoItem
    {
        Task<CarrinhoItem?> ObterPorIdAsync(Guid idCarrinhoItem);
        Task<CarrinhoItem?> ObterPorCarrinhoEProdutoAsync(Guid idCarrinho, Guid idProduto);
        Task<List<CarrinhoItem>> ObterItensPorCarrinhoAsync(Guid idCarrinho);
        Task<CarrinhoItem> AdicionarItemAsync(CarrinhoItem item);
        Task<CarrinhoItem> AtualizarItemAsync(CarrinhoItem item);
        Task<bool> RemoverItemAsync(Guid idCarrinhoItem);
        Task<bool> RemoverTodosItensCarrinhoAsync(Guid idCarrinho);
        Task<int> ContarItensCarrinhoAsync(Guid idCarrinho);
        Task<decimal> CalcularTotalCarrinhoAsync(Guid idCarrinho);
    }
}