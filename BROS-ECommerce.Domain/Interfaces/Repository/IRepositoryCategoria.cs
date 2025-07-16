using BROS_ECommerce.Domain.Entities;
using BROS_ECommerce.Domain.Interfaces.Crud;

namespace BROS_ECommerce.Domain.Interfaces.Repository
{
    public interface IRepositoryCategoria : IDisposable, IRepositoryBase<Categoria>, IAdicionarAsync<Categoria>, IAtualizarAsync<Categoria>, IBuscarPorIdAsync<Categoria>, IEncontrarAsync<Categoria>
    {
        Task<IEnumerable<Categoria>> ObterTodasCategorias();
    }
}
