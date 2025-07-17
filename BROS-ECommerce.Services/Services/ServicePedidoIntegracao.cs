using BROS_ECommerce.Domain.Entities;
using BROS_ECommerce.Domain.Interfaces.Repository;
using BROS_ECommerce.Services.Interface.Services;
using BROS_ECommerce.Services.ViewModel.Carrinho;

namespace BROS_ECommerce.Services.Interface.Services
{
    public interface IServicePedidoIntegracao
    {
        Task<Pedido> ConverterCarrinhoParaPedidoAsync(Guid idUsuario, string? observacoes = null);
        Task<Pedido> FinalizarPedidoComPagamentoAsync(Guid idUsuario, string metodoPagamento, string? observacoes = null);
        IServicePedido ServicePedido { get; }
    }
}

namespace BROS_ECommerce.Services.Services
{
    public class ServicePedidoIntegracao : IServicePedidoIntegracao
    {
        private readonly IServicePedido _servicePedido;
        private readonly IServiceCarrinho _serviceCarrinho;
        private readonly IRepositoryPedidoItem _repositoryPedidoItem;

        public IServicePedido ServicePedido => _servicePedido;

        public ServicePedidoIntegracao(
            IServicePedido servicePedido,
            IServiceCarrinho serviceCarrinho,
            IRepositoryPedidoItem repositoryPedidoItem)
        {
            _servicePedido = servicePedido;
            _serviceCarrinho = serviceCarrinho;
            _repositoryPedidoItem = repositoryPedidoItem;
        }

        public async Task<Pedido> ConverterCarrinhoParaPedidoAsync(Guid idUsuario, string? observacoes = null)
        {
            var carrinho = await _serviceCarrinho.ObterCarrinhoUsuarioAsync(idUsuario);
            if (carrinho == null || !carrinho.TemItens)
            {
                throw new InvalidOperationException("Carrinho vazio ou não encontrado");
            }

            var valorDesconto = 0m; // CarrinhoViewModel não tem ValorDesconto
            var valorFrete = 0m;    // CarrinhoViewModel não tem ValorFrete

            var pedido = await _servicePedido.CriarPedidoAsync(
                idUsuario,
                carrinho.ValorTotal,  // Usando ValorTotal como subtotal
                valorDesconto,
                valorFrete,
                observacoes
            );

            foreach (var item in carrinho.Itens)
            {
                var pedidoItem = new PedidoItem
                {
                    IdPedidoItem = Guid.NewGuid(),
                    IdPedido = pedido.IdPedido,
                    IdProduto = item.IdProduto,
                    Quantidade = item.Quantidade,
                    PrecoUnitario = item.PrecoUnitario,
                    ValorDesconto = 0m, // ItemCarrinhoViewModel não tem ValorDesconto
                    ValorTotal = item.Subtotal
                };

                await _repositoryPedidoItem.AdicionarItemAsync(pedidoItem);
            }

            // Limpar carrinho usando o IdCarrinho
            await _serviceCarrinho.LimparCarrinhoAsync(carrinho.IdCarrinho);

            return pedido;
        }

        public async Task<Pedido> FinalizarPedidoComPagamentoAsync(Guid idUsuario, string metodoPagamento, string? observacoes = null)
        {
            var pedido = await ConverterCarrinhoParaPedidoAsync(idUsuario, observacoes);

            var status = metodoPagamento switch
            {
                "stripe" => "Confirmado",
                "mercadopago" => "Confirmado",
                _ => "Pendente"
            };

            await _servicePedido.AtualizarStatusPedidoAsync(pedido.IdPedido, status);

            return pedido;
        }
    }
}