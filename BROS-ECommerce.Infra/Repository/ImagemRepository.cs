using BROS_ECommerce.Domain.Entities;
using BROS_ECommerce.Domain.Interfaces.Repository;
using BROS_ECommerce.Infra.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BROS_ECommerce.Infra.Repository
{
    public class ImagemRepository : RepositoryBase<Imagem>, IRepositoryImagem
    {
        public ImagemRepository(BrosContext context) : base(context) { }

        
        public async Task AdicionarAsync(Imagem entity)
        {
            entity.DataCriacao = DateTime.UtcNow;
            _dbSet.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarAsync(Imagem entity)
        {
            entity.DataAtualizacao = DateTime.UtcNow;
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<Imagem> BuscarPorIdAsync(Guid id, bool somenteLeitura = false)
        {
            if (somenteLeitura)
            {
                return await _dbSet.AsNoTracking().FirstOrDefaultAsync(i => i.IdImagem == id) ?? new Imagem();
            }
            return await _dbSet.FindAsync(id) ?? new Imagem();
        }

        public async Task<List<Imagem>> EncontrarAsync(Expression<Func<Imagem, bool>> expressao)
        {
            return await _dbSet.Where(expressao).ToListAsync();
        }

        
        public async Task<List<Imagem>> ObterTodosAsync()
        {
            return await _dbSet
                .OrderByDescending(i => i.DataCriacao)
                .ToListAsync();
        }

        public async Task<List<Imagem>> ObterAtivosAsync()
        {
            return await _dbSet
                .Where(i => i.Ativo)
                .OrderByDescending(i => i.DataCriacao)
                .ToListAsync();
        }

        public async Task<Imagem?> ObterPorIdAsync(Guid id)
        {
            return await _dbSet
                .Include(i => i.ProdutoImagens)
                .ThenInclude(pi => pi.Produto)
                .FirstOrDefaultAsync(i => i.IdImagem == id);
        }

        public async Task<Imagem?> ObterLogosPorIdAsync(Guid id)
        {
            var imagem = await _dbSet
                .IgnoreQueryFilters()
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.IdImagem == id);
            return imagem;
        }


        public async Task<Imagem?> ObterPorNomeArquivoAsync(string nomeArquivo)
        {
            return await _dbSet
                .FirstOrDefaultAsync(i => i.NomeArquivo == nomeArquivo);
        }

        public async Task<Imagem?> ObterPorCaminhoAsync(string caminho)
        {
            return await _dbSet
                .FirstOrDefaultAsync(i => i.CaminhoArquivo == caminho);
        }

        public async Task<List<Imagem>> BuscarPorTipoMimeAsync(string tipoMime)
        {
            return await _dbSet
                .Where(i => i.TipoMime == tipoMime && i.Ativo)
                .OrderByDescending(i => i.DataCriacao)
                .ToListAsync();
        }

        public async Task<List<Imagem>> ObterPaginadoAsync(int pagina, int tamanhoPagina)
        {
            return await _dbSet
                .Where(i => i.Ativo)
                .OrderByDescending(i => i.DataCriacao)
                .Skip((pagina - 1) * tamanhoPagina)
                .Take(tamanhoPagina)
                .ToListAsync();
        }

        public async Task<int> ContarTotalAsync()
        {
            return await _dbSet.CountAsync();
        }

        public async Task<int> ContarAtivosAsync()
        {
            return await _dbSet.CountAsync(i => i.Ativo);
        }

        public async Task ExcluirAsync(Guid id)
        {
            var imagem = await _dbSet.FindAsync(id);
            if (imagem != null)
            {
                _dbSet.Remove(imagem);
                await _context.SaveChangesAsync();
            }
        }

        public async Task ExcluirLogicamenteAsync(Guid id)
        {
            var imagem = await _dbSet.FindAsync(id);
            if (imagem != null)
            {
                imagem.Ativo = false;
                imagem.DataAtualizacao = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Imagem>> ObterImagensNaoAssociadasAsync()
        {
            return await _dbSet
                .Where(i => i.Ativo && !i.ProdutoImagens.Any())
                .OrderByDescending(i => i.DataCriacao)
                .ToListAsync();
        }

        public async Task<List<Imagem>> ObterImagensAssociadasAoProdutoAsync(Guid idProduto)
        {
            return await _dbSet
                .Where(i => i.Ativo && i.ProdutoImagens.Any(pi => pi.IdProduto == idProduto))
                .OrderByDescending(i => i.DataCriacao)
                .ToListAsync();
        }

        public async Task<bool> NomeArquivoExisteAsync(string nomeArquivo)
        {
            return await _dbSet.AnyAsync(i => i.NomeArquivo == nomeArquivo);
        }

        public async Task<long> ObterTamanhoTotalImagensAsync()
        {
            return await _dbSet
                .Where(i => i.Ativo)
                .SumAsync(i => i.TamanhoArquivo);
        }
    }
}