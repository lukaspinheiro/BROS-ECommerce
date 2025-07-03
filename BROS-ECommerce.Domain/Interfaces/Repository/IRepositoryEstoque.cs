using BROS_ECommerce.Domain.Entities;

namespace BROS_ECommerce.Domain.Interfaces.Repository
{
    public interface IRepositoryEstoque : IDisposable, IRepositoryBase<Estoque>
    {
        Task<List<Estoque>> ObterTodosAsync();

    }
}
