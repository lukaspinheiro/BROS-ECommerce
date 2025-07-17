using BROS_ECommerce.Domain.Entities;
using BROS_ECommerce.Domain.Interfaces.Repository;
using BROS_ECommerce.Infra.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BROS_ECommerce.Infra.Repository
{
    public class RepositoryCategoriaProduto : RepositoryBase<CategoriaProduto>, IRepositoryCategoriaProduto
    {
        public RepositoryCategoriaProduto(BrosContext context) : base(context) { }

        public async Task AdicionarAsync(CategoriaProduto categoriaProduto)
        {
            _dbSet.Add(categoriaProduto);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarAsync(CategoriaProduto categoriaProduto)
        {
            _dbSet.Update(categoriaProduto);
            await _context.SaveChangesAsync();
        }
        public async Task ExcluirAsync(Guid id)
        {
            var categoriaProduto = await _dbSet.FindAsync(id);
            if (categoriaProduto != null)
            {
                _dbSet.Remove(categoriaProduto);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<CategoriaProduto> BuscarPorIdAsync(Guid id, bool somenteLeitura = false)
        {
            if (somenteLeitura)
                return await _dbSet.AsNoTracking().FirstOrDefaultAsync(b => b.IdCategoriaProduto == id) ?? new CategoriaProduto();
            else
                return await _dbSet.FirstOrDefaultAsync(b => b.IdCategoriaProduto == id) ?? new CategoriaProduto();
        }

        public async Task<List<CategoriaProduto>> EncontrarAsync(Expression<Func<CategoriaProduto, bool>> expressao)
        {
            return await _dbSet.Where(expressao).ToListAsync();
        }

        public async Task<List<CategoriaProduto>> ListarPorProdutoAsync(Guid idProduto)
        {
            return await _context.CategoriaProdutos
                .Include(cp => cp.Categoria)
                .Include(cp => cp.Produto)
                .Where(cp => cp.IdProduto == idProduto)
                .ToListAsync();
        }

        public async Task<List<CategoriaProduto>> ListarPorCategoriaAsync(Guid idCategoria)
        {
            return await _context.CategoriaProdutos
                .Where(cp => cp.IdCategoria == idCategoria)
                .ToListAsync();
        }

        public void RemoverTodos(List<CategoriaProduto> lista)
        {
            _dbSet.RemoveRange(lista);
        }

        public async Task RemoverVinculosPorProdutoAsync(Guid idProduto)
        {
            var vinculos = await _dbSet.Where(cp => cp.IdProduto == idProduto).ToListAsync();

            if (vinculos.Any())
            {
                _dbSet.RemoveRange(vinculos);
                await _context.SaveChangesAsync();
            }
        }


        public async Task SalvarAsync()
        {
            await _context.SaveChangesAsync();
        }

    }
}
