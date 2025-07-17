using BROS_ECommerce.Domain.Entities;

namespace BROS_ECommerce.Domain.Interfaces.Repository
{
    public interface IRepositoryPedidoItem
    {
        Task<PedidoItem?> ObterPorIdAsync(Guid idPedidoItem);
        Task<List<PedidoItem>> ObterItensPorPedidoAsync(Guid idPedido);
        Task<PedidoItem> AdicionarItemAsync(PedidoItem item);
        Task<PedidoItem> AtualizarItemAsync(PedidoItem item);
        Task<bool> RemoverItemAsync(Guid idPedidoItem);
        Task<bool> RemoverTodosItensPedidoAsync(Guid idPedido);
        Task<int> ContarItensPedidoAsync(Guid idPedido);
        Task<decimal> CalcularTotalPedidoAsync(Guid idPedido);
    }
}