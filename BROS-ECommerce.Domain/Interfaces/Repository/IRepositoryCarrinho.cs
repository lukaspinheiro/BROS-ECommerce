using BROS_ECommerce.Domain.Entities;

namespace BROS_ECommerce.Domain.Interfaces.Repository
{
    public interface IRepositoryCarrinho
    {
        Task<Carrinho?> ObterPorIdAsync(Guid idCarrinho);
        Task<Carrinho?> ObterCarrinhoAbertoUsuarioAsync(Guid idUsuario);
        Task<Carrinho?> ObterCarrinhoComItensAsync(Guid idCarrinho);
        Task<Carrinho> CriarCarrinhoAsync(Carrinho carrinho);
        Task<Carrinho> AtualizarCarrinhoAsync(Carrinho carrinho);
        Task<bool> RemoverCarrinhoAsync(Guid idCarrinho);
        Task<List<Carrinho>> ObterCarrinhosPorUsuarioAsync(Guid idUsuario);
        Task<bool> ExisteCarrinhoAbertoAsync(Guid idUsuario);
        Task<bool> FinalizarCarrinhoAsync(Guid idCarrinho);
    }
}