using BROS_ECommerce.Domain.Entities;
using BROS_ECommerce.Domain.Interfaces.Repository;
using BROS_ECommerce.Infra.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BROS_ECommerce.Infra.Repository
{
    public class CategoriaRepository : RepositoryBase<Categoria>, IRepositoryCategoria
    {
        public CategoriaRepository(BrosContext context) : base(context) { }

        public async Task AdicionarAsync(Categoria categoria)
        {
            _dbSet.Add(categoria);
            await _context.SaveChangesAsync();
        }
        
        public async Task ExcluirAsync(Guid id)
        {
            var produto = await _dbSet.FindAsync(id);
            if (produto != null)
            {
                _dbSet.Remove(produto);
                await _context.SaveChangesAsync();
            }
        }

        public async Task AtualizarAsync(Categoria categoria)
        {
            _dbSet.Update(categoria);
            await _context.SaveChangesAsync();
        }

        public async Task<Categoria> BuscarPorIdAsync(Guid id, bool somenteLeitura = false)
        {
            if (somenteLeitura)
                return await _dbSet.AsNoTracking().FirstOrDefaultAsync(c => c.IdCategoria == id) ?? new Categoria();
            else
                return await _dbSet.FirstOrDefaultAsync(c => c.IdCategoria == id) ?? new Categoria();
        }

        public async Task<List<Categoria>> EncontrarAsync(Expression<Func<Categoria, bool>> expressao)
        {
            return await _dbSet.Where(expressao).ToListAsync();
        }

        public async Task<bool> CategoriaExisteAsync(string nomeCategoria)
        {
            return await _dbSet.AnyAsync(c => c.NomeCategoria.ToLower() == nomeCategoria.ToLower());
        }

        public async Task<IEnumerable<Categoria>> ObterTodasCategorias()
        {
            return await _dbSet
                .AsNoTracking()
                .OrderBy(c => c.NomeCategoria)
                .ToListAsync();
        }
    }
}
