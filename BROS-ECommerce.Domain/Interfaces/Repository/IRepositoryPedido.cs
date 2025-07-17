using BROS_ECommerce.Domain.Entities;
using BROS_ECommerce.Domain.Interfaces.Crud;

namespace BROS_ECommerce.Domain.Interfaces.Repository
{
    public interface IRepositoryPedido : IDisposable, IRepositoryBase<Pedido>, IAdicionarAsync<Pedido>, IAtualizarAsync<Pedido>, IBuscarPorIdAsync<Pedido>, IEncontrarAsync<Pedido>
    {
        Task<List<Pedido>> ObterTodosAsync();
        Task<Pedido?> ObterPorIdAsync(Guid id);
        Task<Pedido?> ObterPorIdComItensAsync(Guid id);
        Task<List<Pedido>> ObterTodosComItensAsync();
        Task<List<Pedido>> ObterPedidosPorUsuarioAsync(Guid idUsuario);
        Task<List<Pedido>> ObterPedidosPorStatusAsync(string status);
        Task<List<Pedido>> ObterPedidosPorPeriodoAsync(DateTime dataInicio, DateTime dataFim);
        Task<int> ContarTotalAsync();
        Task<int> ContarPorStatusAsync(string status);
        Task<string> GerarNumeroPedidoAsync();
        Task<bool> AtualizarStatusAsync(Guid idPedido, string novoStatus);
        Task ExcluirAsync(Guid id);
        Task<List<Pedido>> ObterTodosComUsuarioAsync();
        Task<List<Pedido>> BuscarPorNumeroAsync(string numeroPedido);
        Task<List<Pedido>> BuscarPorNomeClienteAsync(string nomeCliente);
    }
}