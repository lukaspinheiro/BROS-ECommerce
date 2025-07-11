using BROS_ECommerce.Domain.Entities;
using BROS_ECommerce.Domain.Interfaces.Repository;
using BROS_ECommerce.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace BROS_ECommerce.Infra.Repository
{
    public class CarrinhoItemRepository : IRepositoryCarrinhoItem
    {
        private readonly BrosContext _context;

        public CarrinhoItemRepository(BrosContext context)
        {
            _context = context;
        }

        public async Task<CarrinhoItem?> ObterPorIdAsync(Guid idCarrinhoItem)
        {
            return await _context.CarrinhoItens
                .Include(ci => ci.Produto)
                .FirstOrDefaultAsync(ci => ci.IdCarrinhoItem == idCarrinhoItem);
        }

        public async Task<CarrinhoItem?> ObterPorCarrinhoEProdutoAsync(Guid idCarrinho, Guid idProduto)
        {
            return await _context.CarrinhoItens
                .FirstOrDefaultAsync(ci => ci.IdCarrinho == idCarrinho && ci.IdProduto == idProduto);
        }

        public async Task<List<CarrinhoItem>> ObterItensPorCarrinhoAsync(Guid idCarrinho)
        {
            return await _context.CarrinhoItens
                .Include(ci => ci.Produto)
                    .ThenInclude(p => p.ProdutoImagens)
                        .ThenInclude(pi => pi.Imagem)
                .Where(ci => ci.IdCarrinho == idCarrinho)
                .ToListAsync();
        }

        public async Task<CarrinhoItem> AdicionarItemAsync(CarrinhoItem item)
        {
            _context.CarrinhoItens.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<CarrinhoItem> AtualizarItemAsync(CarrinhoItem item)
        {
            _context.CarrinhoItens.Update(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<bool> RemoverItemAsync(Guid idCarrinhoItem)
        {
            var item = await ObterPorIdAsync(idCarrinhoItem);
            if (item == null) return false;

            _context.CarrinhoItens.Remove(item);
            var result = await _context.SaveChangesAsync();
            return result;
        }

        public async Task<bool> RemoverTodosItensCarrinhoAsync(Guid idCarrinho)
        {
            var itens = await _context.CarrinhoItens
                .Where(ci => ci.IdCarrinho == idCarrinho)
                .ToListAsync();

            if (!itens.Any()) return true;

            _context.CarrinhoItens.RemoveRange(itens);
            var result = await _context.SaveChangesAsync();
            return result;
        }

        public async Task<int> ContarItensCarrinhoAsync(Guid idCarrinho)
        {
            return await _context.CarrinhoItens
                .Where(ci => ci.IdCarrinho == idCarrinho)
                .SumAsync(ci => ci.Quantidade);
        }

        public async Task<decimal> CalcularTotalCarrinhoAsync(Guid idCarrinho)
        {
            return await _context.CarrinhoItens
                .Where(ci => ci.IdCarrinho == idCarrinho)
                .SumAsync(ci => ci.Quantidade * ci.PrecoUnitario);
        }
    }
}