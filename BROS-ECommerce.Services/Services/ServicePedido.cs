using BROS_ECommerce.Domain.Entities;
using BROS_ECommerce.Domain.Interfaces.Repository;
using BROS_ECommerce.Services.Interface.Services;
using BROS_ECommerce.Services.ViewModel.Pedido;

namespace BROS_ECommerce.Services.Services
{
    public class ServicePedido : IServicePedido
    {
        private readonly IRepositoryPedido _repositoryPedido;
        private readonly IRepositoryPedidoItem _repositoryPedidoItem;

        public ServicePedido(IRepositoryPedido repositoryPedido, IRepositoryPedidoItem repositoryPedidoItem)
        {
            _repositoryPedido = repositoryPedido;
            _repositoryPedidoItem = repositoryPedidoItem;
        }

        public async Task<List<TabelaPedidoViewModel>> ObterTabelaPedidoAsync()
        {
            var pedidos = await _repositoryPedido.ObterTodosComUsuarioAsync();
            return MapearParaTabelaPedido(pedidos);
        }

        public async Task<List<TabelaPedidoViewModel>> ObterTabelaPedidoFiltradaAsync(FiltroPedidoViewModel filtro)
        {
            var pedidos = await _repositoryPedido.ObterTodosComUsuarioAsync();

            if (!string.IsNullOrEmpty(filtro.NumeroPedido))
                pedidos = pedidos.Where(p => p.NumeroPedido.Contains(filtro.NumeroPedido)).ToList();

            if (!string.IsNullOrEmpty(filtro.Status))
                pedidos = pedidos.Where(p => p.Status == filtro.Status).ToList();

            if (!string.IsNullOrEmpty(filtro.NomeCliente))
                pedidos = pedidos.Where(p => p.Usuario.Nome.Contains(filtro.NomeCliente)).ToList();

            if (filtro.DataInicio.HasValue)
                pedidos = pedidos.Where(p => p.DataPedido >= filtro.DataInicio.Value).ToList();

            if (filtro.DataFim.HasValue)
                pedidos = pedidos.Where(p => p.DataPedido <= filtro.DataFim.Value).ToList();

            return MapearParaTabelaPedido(pedidos);
        }

        public async Task<DetalhesPedidoViewModel?> ObterDetalhesPedidoAsync(Guid idPedido)
        {
            var pedido = await _repositoryPedido.ObterPorIdComItensAsync(idPedido);
            if (pedido == null) return null;

            return new DetalhesPedidoViewModel
            {
                IdPedido = pedido.IdPedido,
                NumeroPedido = pedido.NumeroPedido,
                DataPedido = pedido.DataPedido,
                Status = pedido.Status,
                ValorSubtotal = pedido.ValorSubtotal,
                ValorDesconto = pedido.ValorDesconto,
                ValorFrete = pedido.ValorFrete,
                ValorTotal = pedido.ValorTotal,
                ObservacoesPedido = pedido.ObservacoesPedido,
                ObservacoesInternas = pedido.ObservacoesInternas,
                DataCancelamento = pedido.DataCancelamento,
                MotivoCancelamento = pedido.MotivoCancelamento,
                DataCriacao = pedido.DataCriacao,
                DataAtualizacao = pedido.DataAtualizacao,
                Cliente = new ClientePedidoViewModel
                {
                    IdUsuario = pedido.Usuario.IdUser,
                    Nome = pedido.Usuario.Nome,
                    Email = pedido.Usuario.Email,
                    Cpf = pedido.Usuario.Cpf
                },
                Itens = pedido.PedidoItens.Select(item => new ItemPedidoViewModel
                {
                    IdPedidoItem = item.IdPedidoItem,
                    IdProduto = item.IdProduto,
                    NomeProduto = item.Produto.Nome,
                    Quantidade = item.Quantidade,
                    PrecoUnitario = item.PrecoUnitario,
                    ValorDesconto = item.ValorDesconto,
                    ValorTotal = item.ValorTotal
                }).ToList()
            };
        }

        public async Task<List<TabelaPedidoViewModel>> ObterPedidosPorUsuarioAsync(Guid idUsuario)
        {
            var pedidos = await _repositoryPedido.ObterPedidosPorUsuarioAsync(idUsuario);
            return MapearParaTabelaPedido(pedidos);
        }

        public async Task<bool> AtualizarStatusPedidoAsync(Guid idPedido, string novoStatus)
        {
            return await _repositoryPedido.AtualizarStatusAsync(idPedido, novoStatus);
        }

        public async Task<Pedido> CriarPedidoAsync(Guid idUsuario, decimal valorSubtotal, decimal valorDesconto, decimal valorFrete, string? observacoes = null)
        {
            var numeroPedido = await _repositoryPedido.GerarNumeroPedidoAsync();

            var pedido = new Pedido
            {
                IdPedido = Guid.NewGuid(),
                IdUsuario = idUsuario,
                NumeroPedido = numeroPedido,
                DataPedido = DateTime.UtcNow,
                Status = "Pendente",
                ValorSubtotal = valorSubtotal,
                ValorDesconto = valorDesconto,
                ValorFrete = valorFrete,
                ValorTotal = valorSubtotal - valorDesconto + valorFrete,
                ObservacoesPedido = observacoes,
                DataCriacao = DateTime.UtcNow
            };

            await _repositoryPedido.AdicionarAsync(pedido);
            return pedido;
        }

        public async Task<bool> CancelarPedidoAsync(Guid idPedido, string motivoCancelamento)
        {
            var pedido = await _repositoryPedido.ObterPorIdAsync(idPedido);
            if (pedido == null) return false;

            pedido.Status = "Cancelado";
            pedido.DataCancelamento = DateTime.UtcNow;
            pedido.MotivoCancelamento = motivoCancelamento;
            pedido.DataAtualizacao = DateTime.UtcNow;

            await _repositoryPedido.AtualizarAsync(pedido);
            return true;
        }

        public async Task<int> ContarPedidosPorStatusAsync(string status)
        {
            return await _repositoryPedido.ContarPorStatusAsync(status);
        }

        public async Task<decimal> ObterValorTotalPedidosAsync()
        {
            var pedidos = await _repositoryPedido.ObterTodosAsync();
            return pedidos.Sum(p => p.ValorTotal);
        }

        public async Task<bool> ExcluirPedidoAsync(Guid idPedido)
        {
            try
            {
                await _repositoryPedido.ExcluirAsync(idPedido);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private List<TabelaPedidoViewModel> MapearParaTabelaPedido(List<Pedido> pedidos)
        {
            return pedidos.Select(p => new TabelaPedidoViewModel
            {
                IdPedido = p.IdPedido,
                NumeroPedido = p.NumeroPedido,
                DataPedido = p.DataPedido,
                NomeCliente = p.Usuario.Nome,
                Status = p.Status,
                QuantidadeItens = p.PedidoItens?.Count ?? 0,
                ValorTotal = p.ValorTotal,
                ObservacoesPedido = p.ObservacoesPedido ?? "",
                DataCriacao = p.DataCriacao
            }).ToList();
        }
    }
}