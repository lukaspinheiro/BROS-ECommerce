using BROS_ECommerce.Domain.Entities;
using BROS_ECommerce.Domain.Interfaces.Repository;
using BROS_ECommerce.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace BROS_ECommerce.Infra.Repository
{
    public class CarrinhoRepository : IRepositoryCarrinho
    {
        private readonly BrosContext _context;

        public CarrinhoRepository(BrosContext context)
        {
            _context = context;
        }

        public async Task<Carrinho?> ObterPorIdAsync(Guid idCarrinho)
        {
            return await _context.Carrinhos
                .FirstOrDefaultAsync(c => c.IdCarrinho == idCarrinho);
        }

        public async Task<Carrinho?> ObterCarrinhoAbertoUsuarioAsync(Guid idUsuario)
        {
            return await _context.Carrinhos
                .FirstOrDefaultAsync(c => c.IdUsuario == idUsuario && c.Status == "Aberto");
        }

        public async Task<Carrinho?> ObterCarrinhoComItensAsync(Guid idCarrinho)
        {
            return await _context.Carrinhos
                .Include(c => c.Itens)
                    .ThenInclude(i => i.Produto)
                        .ThenInclude(p => p.ProdutoImagens)
                            .ThenInclude(pi => pi.Imagem)
                .FirstOrDefaultAsync(c => c.IdCarrinho == idCarrinho);
        }

        public async Task<Carrinho> CriarCarrinhoAsync(Carrinho carrinho)
        {
            carrinho.DataCriacao = DateTime.UtcNow;
            _context.Carrinhos.Add(carrinho);
            await _context.SaveChangesAsync();
            return carrinho;
        }

        public async Task<Carrinho> AtualizarCarrinhoAsync(Carrinho carrinho)
        {
            _context.Carrinhos.Update(carrinho);
            await _context.SaveChangesAsync();
            return carrinho;
        }

        public async Task<bool> RemoverCarrinhoAsync(Guid idCarrinho)
        {
            var carrinho = await ObterPorIdAsync(idCarrinho);
            if (carrinho == null) return false;

            _context.Carrinhos.Remove(carrinho);
            var result = await _context.SaveChangesAsync();
            return result;
        }

        public async Task<List<Carrinho>> ObterCarrinhosPorUsuarioAsync(Guid idUsuario)
        {
            return await _context.Carrinhos
                .Where(c => c.IdUsuario == idUsuario)
                .OrderByDescending(c => c.DataCriacao)
                .ToListAsync();
        }

        public async Task<bool> ExisteCarrinhoAbertoAsync(Guid idUsuario)
        {
            return await _context.Carrinhos
                .AnyAsync(c => c.IdUsuario == idUsuario && c.Status == "Aberto");
        }

        public async Task<bool> FinalizarCarrinhoAsync(Guid idCarrinho)
        {
            var carrinho = await ObterPorIdAsync(idCarrinho);
            if (carrinho == null) return false;

            carrinho.Status = "Finalizado";
            _context.Carrinhos.Update(carrinho);
            var result = await _context.SaveChangesAsync();
            return result;
        }
    }
}