using BROS_ECommerce.Domain.Entities;

namespace BROS_ECommerce.Domain.Interfaces.Repository
{
    public interface IRepositoryPromocao
    {
        Task<IEnumerable<Promocao>> ObterTodasComProdutoAsync();
        Task<IEnumerable<Promocao>> ObterAtivasAsync();
        Task<Promocao?> ObterPorIdAsync(Guid id);
        Task AdicionarAsync(Promocao promocao);
        Task AtualizarAsync(Promocao promocao);
        Task ExcluirAsync(Guid id);
    }
}
