using BROS_ECommerce.Domain.Entities;
using BROS_ECommerce.Domain.Interfaces.Repository;
using BROS_ECommerce.Infra.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BROS_ECommerce.Infra.Repository
{
    public class ProdutoRepository : RepositoryBase<Produto>, IRepositoryProduto
    {
        public ProdutoRepository(BrosContext context) : base(context) { }
     
        public async Task<List<Produto>> ObterTodosComImagensAsync()
        {
            var produto = await _dbSet
                .Include(p => p.ProdutoImagens)
                .ThenInclude(pi => pi.Imagem)
                .ToListAsync();
            return produto;
        }

        
        public async Task<Produto?> ObterPorSlugComImagensAsync(string slug)
        {
            return await _dbSet
                .Include(p => p.ProdutoImagens)
                .ThenInclude(pi => pi.Imagem)
                .FirstOrDefaultAsync(p => p.Slug == slug);
        }

        
        public async Task<Produto?> ObterPorIdComImagensAsync(Guid id)
        {
            return await _dbSet
                .Include(p => p.ProdutoImagens)
                .ThenInclude(pi => pi.Imagem)
                .FirstOrDefaultAsync(p => p.IdProduto == id);
        }

        public bool Any(Guid id)
        {
            return _context.Produtos.Any(o => o.IdProduto == id);
        }

        public async Task AdicionarAsync(Produto produto)
        {
            _dbSet.Add(produto);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarAsync(Produto produto)
        {
            _dbSet.Update(produto);
            await _context.SaveChangesAsync();
        }

        public async Task<Produto> BuscarPorIdAsync(Guid id, bool somenteLeitura = false)
        {
            if (somenteLeitura)
                return await _dbSet.AsNoTracking().FirstOrDefaultAsync(b => b.IdProduto == id) ?? new Produto();
            else
                return await _dbSet.FirstOrDefaultAsync(b => b.IdProduto == id) ?? new Produto();
        }

        public async Task<List<Produto>> EncontrarAsync(Expression<Func<Produto, bool>> expressao)
        {
            return await _dbSet.Where(expressao).ToListAsync();
        }

        public Task<List<Produto>> EncontrarAsync(Expression<Func<Produto, bool>> expressao, Func<object, object> orderBy)
        {
            throw new NotImplementedException();
        }

        
        public async Task<List<Produto>> ObterTodosAsync()
        {
            return await _dbSet.ToListAsync();
        }

        
        public async Task<Produto?> ObterPorIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        
        public async Task<Produto?> ObterPorSlugAsync(string slug)
        {
            return await _dbSet.FirstOrDefaultAsync(p => p.Slug == slug);
        }

       
        public async Task<bool> SlugExisteAsync(string slug, Guid? excluirId = null)
        {
            var query = _dbSet.Where(p => p.Slug == slug);

            if (excluirId.HasValue)
                query = query.Where(p => p.IdProduto != excluirId.Value);

            return await query.AnyAsync();
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


        public async Task<List<Produto>> BuscarPorNomeAsync(string nome)
        {
            var produtos = await _dbSet
                .Include(p => p.ProdutoImagens.Where(pi => pi.Principal))
                    .ThenInclude(pi => pi.Imagem)
                .Where(p => p.Nome.Contains(nome))
                .ToListAsync();

            return produtos;
        }

        public async Task<List<Produto>> BuscarPorCategoriaAsync(string nomeCategoria)
        {
            var produtos = await _dbSet
                .Include(p => p.ProdutoImagens.Where(pi => pi.Imagem.Ativo))
                    .ThenInclude(pi => pi.Imagem)
                .Include(p => p.CategoriaProdutos)
                    .ThenInclude(cp => cp.Categoria)
                .Where(p => p.CategoriaProdutos.Any(cp => cp.Categoria.NomeCategoria == nomeCategoria && cp.Categoria.Ativo))
                .ToListAsync();

            return produtos;
        }


        public async Task<List<Produto>> ObterPaginadoAsync(int pagina, int tamanhoPagina)
        {
            return await _dbSet
                .Skip((pagina - 1) * tamanhoPagina)
                .Take(tamanhoPagina)
                .ToListAsync();
        }

        
        public async Task<int> ContarTotalAsync()
        {
            return await _dbSet.CountAsync();
        }
    }
}
