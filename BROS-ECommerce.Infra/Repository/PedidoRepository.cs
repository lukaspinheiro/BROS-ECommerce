using BROS_ECommerce.Domain.Entities;
using BROS_ECommerce.Domain.Interfaces.Repository;
using BROS_ECommerce.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace BROS_ECommerce.Infra.Repository
{
    public class PedidoRepository : RepositoryBase<Pedido>, IRepositoryPedido
    {
        public PedidoRepository(BrosContext context) : base(context) { }

        public async Task<List<Pedido>> ObterTodosAsync()
        {
            return await _dbSet
                .AsNoTracking()
                .OrderByDescending(p => p.DataPedido)
                .ToListAsync();
        }

        public async Task<Pedido?> ObterPorIdAsync(Guid id)
        {
            return await _dbSet
                .FirstOrDefaultAsync(p => p.IdPedido == id);
        }

        public async Task<Pedido?> ObterPorIdComItensAsync(Guid id)
        {
            return await _dbSet
                .Include(p => p.PedidoItens)
                .ThenInclude(pi => pi.Produto)
                .Include(p => p.Usuario)
                .FirstOrDefaultAsync(p => p.IdPedido == id);
        }

        public async Task<List<Pedido>> ObterTodosComItensAsync()
        {
            return await _dbSet
                .Include(p => p.PedidoItens)
                .ThenInclude(pi => pi.Produto)
                .Include(p => p.Usuario)
                .OrderByDescending(p => p.DataPedido)
                .ToListAsync();
        }

        public async Task<List<Pedido>> ObterTodosComUsuarioAsync()
        {
            return await _dbSet
                .Include(p => p.Usuario)
                .Include(p => p.PedidoItens)
                .OrderByDescending(p => p.DataPedido)
                .ToListAsync();
        }

        public async Task<List<Pedido>> ObterPedidosPorUsuarioAsync(Guid idUsuario)
        {
            return await _dbSet
                .Include(p => p.Usuario)
                .Include(p => p.PedidoItens)
                .ThenInclude(pi => pi.Produto)
                .Where(p => p.IdUsuario == idUsuario)
                .OrderByDescending(p => p.DataPedido)
                .ToListAsync();
        }

        public async Task<List<Pedido>> ObterPedidosPorStatusAsync(string status)
        {
            return await _dbSet
                .Include(p => p.Usuario)
                .Where(p => p.Status == status)
                .OrderByDescending(p => p.DataPedido)
                .ToListAsync();
        }

        public async Task<List<Pedido>> ObterPedidosPorPeriodoAsync(DateTime dataInicio, DateTime dataFim)
        {
            return await _dbSet
                .Include(p => p.Usuario)
                .Where(p => p.DataPedido >= dataInicio && p.DataPedido <= dataFim)
                .OrderByDescending(p => p.DataPedido)
                .ToListAsync();
        }

        public async Task<List<Pedido>> BuscarPorNumeroAsync(string numeroPedido)
        {
            return await _dbSet
                .Include(p => p.Usuario)
                .Where(p => p.NumeroPedido.Contains(numeroPedido))
                .OrderByDescending(p => p.DataPedido)
                .ToListAsync();
        }

        public async Task<List<Pedido>> BuscarPorNomeClienteAsync(string nomeCliente)
        {
            return await _dbSet
                .Include(p => p.Usuario)
                .Where(p => p.Usuario.Nome.Contains(nomeCliente))
                .OrderByDescending(p => p.DataPedido)
                .ToListAsync();
        }

        public async Task<int> ContarTotalAsync()
        {
            return await _dbSet.CountAsync();
        }

        public async Task<int> ContarPorStatusAsync(string status)
        {
            return await _dbSet
                .Where(p => p.Status == status)
                .CountAsync();
        }

        public async Task<string> GerarNumeroPedidoAsync()
        {
            var ultimoPedido = await _dbSet
                .OrderByDescending(p => p.DataCriacao)
                .FirstOrDefaultAsync();

            if (ultimoPedido == null)
                return "P-0001";

            var ultimoNumero = ultimoPedido.NumeroPedido.Split('-')[1];
            var proximoNumero = int.Parse(ultimoNumero) + 1;
            return $"P-{proximoNumero:D4}";
        }

        public async Task<bool> AtualizarStatusAsync(Guid idPedido, string novoStatus)
        {
            try
            {
                var pedido = await _dbSet.FirstOrDefaultAsync(p => p.IdPedido == idPedido);
                if (pedido == null)
                    return false;

                pedido.Status = novoStatus;
                pedido.DataAtualizacao = DateTime.UtcNow;

                _dbSet.Update(pedido);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task ExcluirAsync(Guid id)
        {
            var pedido = await ObterPorIdAsync(id);
            if (pedido != null)
            {
                _dbSet.Remove(pedido);
                await _context.SaveChangesAsync();
            }
        }

        public async Task AdicionarAsync(Pedido pedido)
        {
            _dbSet.Add(pedido);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarAsync(Pedido pedido)
        {
            _dbSet.Update(pedido);
            await _context.SaveChangesAsync();
        }

        public async Task<Pedido> BuscarPorIdAsync(Guid id, bool somenteLeitura = false)
        {
            if (somenteLeitura)
                return await _dbSet.AsNoTracking().FirstOrDefaultAsync(p => p.IdPedido == id) ?? new Pedido();
            else
                return await _dbSet.FirstOrDefaultAsync(p => p.IdPedido == id) ?? new Pedido();
        }

        public async Task<List<Pedido>> EncontrarAsync(System.Linq.Expressions.Expression<Func<Pedido, bool>> expression)
        {
            return await _dbSet.Where(expression).ToListAsync();
        }
    }
}