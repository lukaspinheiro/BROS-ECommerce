using BROS_ECommerce.Domain.Entities;
using BROS_ECommerce.Domain.Interfaces.Crud;

namespace BROS_ECommerce.Domain.Interfaces.Repository
{
    public interface IRepositoryCategoriaProduto : IDisposable, IRepositoryBase<CategoriaProduto>, IAdicionarAsync<CategoriaProduto>, IAtualizarAsync<CategoriaProduto>, IBuscarPorIdAsync<CategoriaProduto>, IEncontrarAsync<CategoriaProduto>
    {
        Task<List<CategoriaProduto>> ListarPorProdutoAsync(Guid idProduto);
        Task ExcluirAsync(Guid id);
        Task<List<CategoriaProduto>> ListarPorCategoriaAsync(Guid idCategoria);
        void RemoverTodos(List<CategoriaProduto> lista);
        Task RemoverVinculosPorProdutoAsync(Guid idProduto);
        Task SalvarAsync();

    }
}
