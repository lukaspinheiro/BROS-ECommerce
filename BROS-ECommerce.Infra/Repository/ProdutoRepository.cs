using BROS_ECommerce.Domain.Entities;
using BROS_ECommerce.Domain.Interfaces.Repository;
using BROS_ECommerce.Infra.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BROS_ECommerce.Infra.Repository
{
    public class ProdutoRepository : RepositoryBase<Produto>, IRepositoryProduto
    {
        public ProdutoRepository(BrosContext context) : base(context) { }

        public bool Any(Guid id)
        {
            return _context.Produto
                .Any(o => o.IdProduto == id);
        }

        public async Task AdicionarAsync(Produto banner)
        {
            _dbSet.Add(banner);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarAsync(Produto banner)
        {
            _dbSet.Update(banner);
            await _context.SaveChangesAsync();
        }

        public async Task<Produto> BuscarPorIdAsync(Guid id, bool somenteLeitura = false)
        {
            return await _dbSet.AsNoTracking().FirstOrDefaultAsync(b => b.IdProduto == id);
        }

        public async Task<List<Produto>> EncontrarAsync(Expression<Func<Produto, bool>> expressao)
        {
            return await _dbSet.Where(expressao).ToListAsync();
        }

        public Task<List<Produto>> EncontrarAsync(Expression<Func<Produto, bool>> expressao, Func<object, object> orderBy)
        {
            throw new NotImplementedException();
        }
    }
}
