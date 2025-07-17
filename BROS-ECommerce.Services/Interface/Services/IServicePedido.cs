using BROS_ECommerce.Services.ViewModel.Pedido;
using BROS_ECommerce.Domain.Entities;

namespace BROS_ECommerce.Services.Interface.Services
{
    public interface IServicePedido
    {
        Task<List<TabelaPedidoViewModel>> ObterTabelaPedidoAsync();
        Task<List<TabelaPedidoViewModel>> ObterTabelaPedidoFiltradaAsync(FiltroPedidoViewModel filtro);
        Task<DetalhesPedidoViewModel?> ObterDetalhesPedidoAsync(Guid idPedido);
        Task<List<TabelaPedidoViewModel>> ObterPedidosPorUsuarioAsync(Guid idUsuario);
        Task<bool> AtualizarStatusPedidoAsync(Guid idPedido, string novoStatus);
        Task<Pedido> CriarPedidoAsync(Guid idUsuario, decimal valorSubtotal, decimal valorDesconto, decimal valorFrete, string? observacoes = null);
        Task<bool> CancelarPedidoAsync(Guid idPedido, string motivoCancelamento);
        Task<int> ContarPedidosPorStatusAsync(string status);
        Task<decimal> ObterValorTotalPedidosAsync();
        Task<bool> ExcluirPedidoAsync(Guid idPedido);
    }
}