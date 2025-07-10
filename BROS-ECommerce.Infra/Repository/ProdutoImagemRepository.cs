using BROS_ECommerce.Domain.Entities;
using BROS_ECommerce.Domain.Interfaces.Repository;
using BROS_ECommerce.Infra.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BROS_ECommerce.Infra.Repository
{
    public class ProdutoImagemRepository : RepositoryBase<ProdutoImagem>, IRepositoryProdutoImagem
    {
        public ProdutoImagemRepository(BrosContext context) : base(context) { }

        
        public async Task AdicionarAsync(ProdutoImagem entity)
        {
            entity.DataAssociacao = DateTime.UtcNow;
            _dbSet.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarAsync(ProdutoImagem entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<ProdutoImagem> BuscarPorIdAsync(Guid id, bool somenteLeitura = false)
        {
            if (somenteLeitura)
            {
                return await _dbSet.AsNoTracking().FirstOrDefaultAsync(pi => pi.IdProdutoImagem == id) ?? new ProdutoImagem();
            }
            return await _dbSet.FindAsync(id) ?? new ProdutoImagem();
        }

        public async Task<List<ProdutoImagem>> EncontrarAsync(Expression<Func<ProdutoImagem, bool>> expressao)
        {
            return await _dbSet.Where(expressao).ToListAsync();
        }

        
        public async Task<List<ProdutoImagem>> ObterTodosAsync()
        {
            return await _dbSet
                .Include(pi => pi.Produto)
                .Include(pi => pi.Imagem)
                .OrderBy(pi => pi.IdProduto)
                .ThenBy(pi => pi.Ordem)
                .ToListAsync();
        }

        public async Task<ProdutoImagem?> ObterPorIdAsync(Guid id)
        {
            return await _dbSet
                .Include(pi => pi.Produto)
                .Include(pi => pi.Imagem)
                .FirstOrDefaultAsync(pi => pi.IdProdutoImagem == id);
        }

        public async Task<List<ProdutoImagem>> ObterPorProdutoIdAsync(Guid idProduto)
        {
            return await _dbSet
                .Include(pi => pi.Imagem)
                .Where(pi => pi.IdProduto == idProduto)
                .OrderBy(pi => pi.Ordem)
                .ToListAsync();
        }

        public async Task<List<ProdutoImagem>> ObterPorImagemIdAsync(Guid idImagem)
        {
            return await _dbSet
                .Include(pi => pi.Produto)
                .Where(pi => pi.IdImagem == idImagem)
                .OrderBy(pi => pi.Produto.Nome)
                .ToListAsync();
        }

        public async Task<ProdutoImagem?> ObterImagemPrincipalPorProdutoAsync(Guid idProduto)
        {
            return await _dbSet
                .Include(pi => pi.Imagem)
                .FirstOrDefaultAsync(pi => pi.IdProduto == idProduto && pi.Principal);
        }

        public async Task<List<ProdutoImagem>> ObterImagensOrdendasPorProdutoAsync(Guid idProduto)
        {
            return await _dbSet
                .Include(pi => pi.Imagem)
                .Where(pi => pi.IdProduto == idProduto && pi.Imagem.Ativo)
                .OrderByDescending(pi => pi.Principal)
                .ThenBy(pi => pi.Ordem)
                .ToListAsync();
        }

        public async Task<bool> ExisteAssociacaoAsync(Guid idProduto, Guid idImagem)
        {
            return await _dbSet.AnyAsync(pi => pi.IdProduto == idProduto && pi.IdImagem == idImagem);
        }

        public async Task RemoverAssociacaoAsync(Guid idProduto, Guid idImagem)
        {
            var associacao = await _dbSet
                .FirstOrDefaultAsync(pi => pi.IdProduto == idProduto && pi.IdImagem == idImagem);

            if (associacao != null)
            {
                _dbSet.Remove(associacao);
                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoverTodasAssociacoesProdutoAsync(Guid idProduto)
        {
            var associacoes = await _dbSet
                .Where(pi => pi.IdProduto == idProduto)
                .ToListAsync();

            _dbSet.RemoveRange(associacoes);
            await _context.SaveChangesAsync();
        }

        public async Task RemoverTodasAssociacoesImagemAsync(Guid idImagem)
        {
            var associacoes = await _dbSet
                .Where(pi => pi.IdImagem == idImagem)
                .ToListAsync();

            _dbSet.RemoveRange(associacoes);
            await _context.SaveChangesAsync();
        }

        public async Task DefinirImagemPrincipalAsync(Guid idProduto, Guid idImagem)
        {
           
            var imagensAtuais = await _dbSet
                .Where(pi => pi.IdProduto == idProduto)
                .ToListAsync();

            foreach (var img in imagensAtuais)
            {
                img.Principal = false;
            }

            
            var novaImagemPrincipal = imagensAtuais.FirstOrDefault(pi => pi.IdImagem == idImagem);
            if (novaImagemPrincipal != null)
            {
                novaImagemPrincipal.Principal = true;
            }

            await _context.SaveChangesAsync();
        }

        public async Task RemoverImagemPrincipalAsync(Guid idProduto)
        {
            var imagemPrincipal = await _dbSet
                .FirstOrDefaultAsync(pi => pi.IdProduto == idProduto && pi.Principal);

            if (imagemPrincipal != null)
            {
                imagemPrincipal.Principal = false;
                await _context.SaveChangesAsync();
            }
        }

        public async Task AtualizarOrdemImagensAsync(Guid idProduto, Dictionary<Guid, int> imagensOrdem)
        {
            var imagensProduto = await _dbSet
                .Where(pi => pi.IdProduto == idProduto)
                .ToListAsync();

            foreach (var imagem in imagensProduto)
            {
                if (imagensOrdem.ContainsKey(imagem.IdImagem))
                {
                    imagem.Ordem = imagensOrdem[imagem.IdImagem];
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task<int> ContarImagensPorProdutoAsync(Guid idProduto)
        {
            return await _dbSet.CountAsync(pi => pi.IdProduto == idProduto);
        }

        public async Task<int> ContarProdutosPorImagemAsync(Guid idImagem)
        {
            return await _dbSet.CountAsync(pi => pi.IdImagem == idImagem);
        }
    }
}