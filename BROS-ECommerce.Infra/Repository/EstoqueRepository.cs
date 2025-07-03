using BROS_ECommerce.Domain.Entities;
using BROS_ECommerce.Domain.Interfaces.Repository;
using BROS_ECommerce.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace BROS_ECommerce.Infra.Repository
{
    public class EstoqueRepository : RepositoryBase<Estoque>, IRepositoryEstoque
    {
        public EstoqueRepository(BrosContext context) : base(context) { }
        public async Task<List<Estoque>> ObterTodosAsync()
        {
            return await _dbSet.ToListAsync();
        }
    }
}
