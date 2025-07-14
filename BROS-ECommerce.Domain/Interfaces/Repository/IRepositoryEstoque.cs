using BROS_ECommerce.Domain.Entities;
using BROS_ECommerce.Domain.Interfaces.Crud;

namespace BROS_ECommerce.Domain.Interfaces.Repository
{
    public interface IRepositoryEstoque : IDisposable, IRepositoryBase<Estoque>, IAdicionarAsync<Estoque>, IAtualizarAsync<Estoque>, IBuscarPorIdAsync<Estoque>, IEncontrarAsync<Estoque>
    {
        Task<List<Estoque>> ObterTodosAsync();
        Task<Estoque?> ObterPorIdAsync(Guid id);
        Task<Estoque?> ObterPorIdProdutoAsync(Guid idProduto);
        Task<List<Estoque>> ObterEstoquesBaixosAsync(int quantidadeMinima = 5);
        Task<bool> ExistePorIdProdutoAsync(Guid idProduto);
        Task ExcluirAsync(Guid id);
        Task ExcluirPorIdProdutoAsync(Guid idProduto);
        Task<int> ContarTotalAsync();
        Task<List<Estoque>> ObterEstoquesComProdutosAsync();
        Task AtualizarQuantidadeAsync(Guid idProduto, int novaQuantidade, DateTime UltimaAtualizacao);
        Task AdicionarQuantidadeAsync(Guid idProduto, int quantidadeAdicionar, DateTime UltimaAtualizacao);
        Task RemoverQuantidadeAsync(Guid idProduto, int quantidadeRemover);
        Task<bool> VerificarDisponibilidadeAsync(Guid idProduto, int quantidadeSolicitada);
        Task<List<Estoque>> ObterTodosComImagensAsync();
    }
}