using BROS_ECommerce.Domain.Entities;
using BROS_ECommerce.Domain.Interfaces.Crud;

namespace BROS_ECommerce.Domain.Interfaces.Repository
{
    public interface IRepositoryProdutoImagem : IDisposable, IRepositoryBase<ProdutoImagem>, IAdicionarAsync<ProdutoImagem>, IAtualizarAsync<ProdutoImagem>, IBuscarPorIdAsync<ProdutoImagem>, IEncontrarAsync<ProdutoImagem>
    {
        Task<List<ProdutoImagem>> ObterTodosAsync();
        Task<ProdutoImagem?> ObterPorIdAsync(Guid id);
        Task<List<ProdutoImagem>> ObterPorProdutoIdAsync(Guid idProduto);
        Task<List<ProdutoImagem>> ObterPorImagemIdAsync(Guid idImagem);
        Task<ProdutoImagem?> ObterImagemPrincipalPorProdutoAsync(Guid idProduto);
        Task<List<ProdutoImagem>> ObterImagensOrdendasPorProdutoAsync(Guid idProduto);
        Task<bool> ExisteAssociacaoAsync(Guid idProduto, Guid idImagem);
        Task RemoverAssociacaoAsync(Guid idProduto, Guid idImagem);
        Task RemoverTodasAssociacoesProdutoAsync(Guid idProduto);
        Task RemoverTodasAssociacoesImagemAsync(Guid idImagem);
        Task DefinirImagemPrincipalAsync(Guid idProduto, Guid idImagem);
        Task RemoverImagemPrincipalAsync(Guid idProduto);
        Task AtualizarOrdemImagensAsync(Guid idProduto, Dictionary<Guid, int> imagensOrdem);
        Task<int> ContarImagensPorProdutoAsync(Guid idProduto);
        Task<int> ContarProdutosPorImagemAsync(Guid idImagem);
    }
}