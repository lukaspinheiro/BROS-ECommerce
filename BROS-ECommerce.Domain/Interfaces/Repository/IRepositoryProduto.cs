using BROS_ECommerce.Domain.Entities;
using BROS_ECommerce.Domain.Interfaces.Crud;

namespace BROS_ECommerce.Domain.Interfaces.Repository
{
    public interface IRepositoryProduto : IDisposable, IRepositoryBase<Produto>, IAdicionarAsync<Produto>, IAtualizarAsync<Produto>, IBuscarPorIdAsync<Produto>, IEncontrarAsync<Produto>
    {
        bool Any(Guid id);
    }
}
