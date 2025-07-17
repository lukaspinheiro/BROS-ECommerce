using BROS_ECommerce.Domain.Entities;
using BROS_ECommerce.Domain.Interfaces.Crud;

namespace BROS_ECommerce.Domain.Interfaces.Repository
{
    public interface IRepositoryProduto : IDisposable, IRepositoryBase<Produto>, IAdicionarAsync<Produto>, IAtualizarAsync<Produto>, IBuscarPorIdAsync<Produto>, IEncontrarAsync<Produto>
    {
        bool Any(Guid id);

        Task<List<Produto>> ObterTodosComImagensAsync();
        Task<Produto?> ObterPorSlugComImagensAsync(string slug);
        Task<Produto?> ObterPorIdComImagensAsync(Guid id);
        Task<List<Produto>> ObterTodosAsync();
        Task<Produto?> ObterPorIdAsync(Guid id);
        Task<Produto?> ObterPorSlugAsync(string slug);
        Task<bool> SlugExisteAsync(string slug, Guid? excluirId = null);
        Task ExcluirAsync(Guid id);
        Task<List<Produto>> BuscarPorNomeAsync(string nome);
        Task<List<Produto>> BuscarPorCategoriaAsync(string nomeCategoria);
        Task<List<Produto>> ObterPaginadoAsync(int pagina, int tamanhoPagina);
        Task<int> ContarTotalAsync();
    }
}