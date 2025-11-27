using BROS_ECommerce.Domain.Entities;

namespace BROS_ECommerce.Domain.Interfaces.Repository;

using BROS_ECommerce.Domain.Entities;

public interface IRepositoryBanner : IRepositoryBase<Banner>
{
    Task AdicionarAsync(Banner banner);
    Task AtualizarAsync(Banner banner);
    Task<Banner?> ObterPorIdAsync(Guid id);
    Task<List<Banner>> ObterTodosAsync();
    Task ExcluirAsync(Guid id);
    Task ExcluirLogicamenteAsync(Guid id);
}

