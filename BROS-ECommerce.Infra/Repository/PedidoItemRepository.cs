using BROS_ECommerce.Domain.Entities;
using BROS_ECommerce.Domain.Interfaces.Repository;
using BROS_ECommerce.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace BROS_ECommerce.Infra.Repository
{
    public class PedidoItemRepository : IRepositoryPedidoItem
    {
        private readonly BrosContext _context;

        public PedidoItemRepository(BrosContext context)
        {
            _context = context;
        }

        public async Task<PedidoItem?> ObterPorIdAsync(Guid idPedidoItem)
        {
            return await _context.PedidoItens
                .Include(pi => pi.Produto)
                .Include(pi => pi.Pedido)
                .FirstOrDefaultAsync(pi => pi.IdPedidoItem == idPedidoItem);
        }

        public async Task<List<PedidoItem>> ObterItensPorPedidoAsync(Guid idPedido)
        {
            return await _context.PedidoItens
                .Include(pi => pi.Produto)
                .Where(pi => pi.IdPedido == idPedido)
                .ToListAsync();
        }

        public async Task<PedidoItem> AdicionarItemAsync(PedidoItem item)
        {
            _context.PedidoItens.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<PedidoItem> AtualizarItemAsync(PedidoItem item)
        {
            _context.PedidoItens.Update(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<bool> RemoverItemAsync(Guid idPedidoItem)
        {
            var item = await ObterPorIdAsync(idPedidoItem);
            if (item == null) return false;

            _context.PedidoItens.Remove(item);
            var resultado = await _context.SaveChangesAsync();
            return resultado;
        }

        public async Task<bool> RemoverTodosItensPedidoAsync(Guid idPedido)
        {
            var itens = await ObterItensPorPedidoAsync(idPedido);
            if (!itens.Any()) return true;

            _context.PedidoItens.RemoveRange(itens);
            var resultado = await _context.SaveChangesAsync();
            return resultado;
        }

        public async Task<int> ContarItensPedidoAsync(Guid idPedido)
        {
            return await _context.PedidoItens
                .Where(pi => pi.IdPedido == idPedido)
                .CountAsync();
        }

        public async Task<decimal> CalcularTotalPedidoAsync(Guid idPedido)
        {
            return await _context.PedidoItens
                .Where(pi => pi.IdPedido == idPedido)
                .SumAsync(pi => pi.ValorTotal);
        }
    }
}