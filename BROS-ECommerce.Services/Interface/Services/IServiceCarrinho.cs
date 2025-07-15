using BROS_ECommerce.Services.ViewModel.Carrinho;

namespace BROS_ECommerce.Services.Interface.Services
{
    public interface IServiceCarrinho
    {
        Task<CarrinhoViewModel?> ObterCarrinhoUsuarioAsync(Guid idUsuario);
        Task<CarrinhoViewModel?> ObterCarrinhoPorIdAsync(Guid idCarrinho);
        Task<CarrinhoViewModel> CriarCarrinhoAsync(Guid? idUsuario = null);
        Task<CarrinhoViewModel> AdicionarProdutoAsync(Guid? idUsuario, Guid idProduto, int quantidade = 1);
        Task<CarrinhoViewModel> AtualizarQuantidadeAsync(Guid idCarrinho, Guid idProduto, int quantidade);
        Task<bool> RemoverProdutoAsync(Guid idCarrinho, Guid idProduto);
        Task<bool> LimparCarrinhoAsync(Guid idCarrinho);
        Task<bool> FinalizarCarrinhoAsync(Guid idCarrinho);
        Task<int> ObterQuantidadeItensAsync(Guid? idUsuario);
        Task<decimal> ObterTotalCarrinhoAsync(Guid idCarrinho);

        Task<CarrinhoViewModel> AtualizarQuantidadeProdutoAsync(Guid idUsuario, Guid idProduto, int novaQuantidade);
        Task<CarrinhoViewModel> AdicionarOuAtualizarProdutoAsync(Guid? idUsuario, Guid idProduto, int quantidade);
        Task<int> ObterQuantidadeItensCarrinhoAsync(Guid? idUsuario);
    }
}