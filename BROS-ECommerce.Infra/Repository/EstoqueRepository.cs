using BROS_ECommerce.Domain.Entities;
using BROS_ECommerce.Domain.Interfaces.Repository;
using BROS_ECommerce.Infra.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BROS_ECommerce.Infra.Repository
{
    public class EstoqueRepository : RepositoryBase<Estoque>, IRepositoryEstoque
    {
        public EstoqueRepository(BrosContext context) : base(context) { }

        public async Task AdicionarAsync(Estoque estoque)
        {
            _dbSet.Add(estoque);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarAsync(Estoque estoque)
        {
            _dbSet.Update(estoque);
            await _context.SaveChangesAsync();
        }

        public async Task<Estoque?> BuscarPorIdAsync(Guid id, bool somenteLeitura = false)
        {
            if (somenteLeitura)
                return await _dbSet.AsNoTracking().FirstOrDefaultAsync(e => e.IdEstoque == id);
            else
                return await _dbSet.FirstOrDefaultAsync(e => e.IdEstoque == id);
        }

        public async Task<List<Estoque>> EncontrarAsync(Expression<Func<Estoque, bool>> expressao)
        {
            return await _dbSet.Where(expressao).ToListAsync();
        }

        public async Task<List<Estoque>> ObterTodosAsync()
        {
            return await _dbSet
                .Include(e => e.Produto)
                .OrderBy(e => e.Produto.Nome)
                .ToListAsync();
        }

        public async Task<Estoque?> ObterPorIdAsync(Guid id)
        {
            return await _dbSet
                .Include(e => e.Produto)
                .FirstOrDefaultAsync(e => e.IdEstoque == id);
        }

        public async Task<Estoque?> ObterPorIdProdutoAsync(Guid idProduto)
        {
            return await _dbSet
                .Include(e => e.Produto)
                .FirstOrDefaultAsync(e => e.IdProduto == idProduto);
        }

        public async Task<List<Estoque>> ObterEstoquesBaixosAsync(int quantidadeMinima = 5)
        {
            return await _dbSet
                .Include(e => e.Produto)
                .Where(e => e.Quantidade <= quantidadeMinima)
                .OrderBy(e => e.Quantidade)
                .ToListAsync();
        }

        public async Task<bool> ExistePorIdProdutoAsync(Guid idProduto)
        {
            return await _dbSet.AnyAsync(e => e.IdProduto == idProduto);
        }

        public async Task ExcluirAsync(Guid id)
        {
            var estoque = await _dbSet.FindAsync(id);
            if (estoque != null)
            {
                _dbSet.Remove(estoque);
                await _context.SaveChangesAsync();
            }
        }

        public async Task ExcluirPorIdProdutoAsync(Guid idProduto)
        {
            var estoque = await _dbSet.FirstOrDefaultAsync(e => e.IdProduto == idProduto);
            if (estoque != null)
            {
                _dbSet.Remove(estoque);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> ContarTotalAsync()
        {
            return await _dbSet.CountAsync();
        }

        public async Task<List<Estoque>> ObterEstoquesComProdutosAsync()
        {
            return await _dbSet
                .Include(e => e.Produto)
                .OrderBy(e => e.Produto.Nome)
                .ToListAsync();
        }

        public async Task AtualizarQuantidadeAsync(Guid idProduto, int novaQuantidade, DateTime UltimaAtualizacao)
        {
            var estoque = await ObterPorIdProdutoAsync(idProduto);
            if (estoque != null)
            {
                estoque.AtualizarQuantidade(novaQuantidade);
                estoque.UltimaAtualizacao = UltimaAtualizacao;
                await AtualizarAsync(estoque);
            }
        }

        public async Task AdicionarQuantidadeAsync(Guid idProduto, int quantidadeAdicionar)
        {
            var estoque = await ObterPorIdProdutoAsync(idProduto);
            if (estoque != null)
            {
                estoque.AdicionarQuantidade(quantidadeAdicionar);
                await AtualizarAsync(estoque);
            }
        }

        public async Task RemoverQuantidadeAsync(Guid idProduto, int quantidadeRemover)
        {
            var estoque = await ObterPorIdProdutoAsync(idProduto);
            if (estoque != null)
            {
                estoque.RemoverQuantidade(quantidadeRemover);
                await AtualizarAsync(estoque);
            }
        }

        public async Task<bool> VerificarDisponibilidadeAsync(Guid idProduto, int quantidadeSolicitada)
        {
            var estoque = await ObterPorIdProdutoAsync(idProduto);
            return estoque?.TemEstoqueSuficiente(quantidadeSolicitada) ?? false;
        }
    }
}