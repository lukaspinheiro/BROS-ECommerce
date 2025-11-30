using BROS_ECommerce.Services.Interface.Services;
using BROS_ECommerce.Services.ViewModel.Pedido;
using BROS_ECommerce.Web.Configuration;
using Microsoft.AspNetCore.Mvc;

namespace BROS_ECommerce.Web.Areas.Administrativo.Controllers
{
    [Area("Administrativo")]
    [Route("Administrativo/Pedido")]
    public class PedidoController : BaseAdminController
    {
        private readonly IServicePedido _servicePedido;

        public PedidoController(IServicePedido servicePedido)
        {
            _servicePedido = servicePedido;
        }

        [HttpGet("index")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var pedidosTabela = await _servicePedido.ObterTabelaPedidoAsync();
                var filtro = new FiltroPedidoViewModel();
                var viewModel = new IndexPedidoViewModel(filtro, pedidosTabela);

                return View(viewModel);
            }
            catch (Exception ex)
            {
                ViewBag.Erro = "Erro ao carregar Pedidos: " + ex.Message;
                var filtro = new FiltroPedidoViewModel();
                var tabelaVazia = new List<TabelaPedidoViewModel>();
                var viewModel = new IndexPedidoViewModel(filtro, tabelaVazia);
                return View(viewModel);
            }
        }

        [HttpPost("Filtrar")]
        public async Task<IActionResult> Filtrar(IndexPedidoViewModel indexPedidoViewModel)
        {
            try
            {
                var filtro = indexPedidoViewModel.Filtro;
                var pedidosTabela = await _servicePedido.ObterTabelaPedidoFiltradaAsync(filtro);
                var viewModel = new IndexPedidoViewModel(filtro, pedidosTabela);

                return View("Index", viewModel);
            }
            catch (Exception ex)
            {
                ViewBag.Erro = "Erro ao filtrar Pedidos: " + ex.Message;
                var filtro = new FiltroPedidoViewModel();
                var tabelaVazia = new List<TabelaPedidoViewModel>();
                var viewModel = new IndexPedidoViewModel(filtro, tabelaVazia);
                return View("Index", viewModel);
            }
        }

        [HttpGet("detalhes/{idPedido}")]
        public async Task<IActionResult> Detalhes(Guid idPedido)
        {

            try
            {
                var pedido = await _servicePedido.ObterDetalhesPedidoAsync(idPedido);
                if (pedido == null)
                {
                    TempData["Erro"] = "Pedido não encontrado.";
                    return RedirectToAction(nameof(Index));
                }

                return View(pedido);
            }
            catch (Exception ex)
            {
                TempData["Erro"] = "Erro ao carregar detalhes do pedido: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost("AtualizarStatus")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AtualizarStatus(Guid idPedido, string novoStatus)
        {

            try
            {
                var resultado = await _servicePedido.AtualizarStatusPedidoAsync(idPedido, novoStatus);
                if (resultado)
                {
                    TempData["Sucesso"] = "Status do pedido atualizado com sucesso!";
                }
                else
                {
                    TempData["Erro"] = "Erro ao atualizar status do pedido.";
                }
            }
            catch (Exception ex)
            {
                TempData["Erro"] = $"Erro ao atualizar status: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost("CancelarPedido")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelarPedido(Guid idPedido, string motivoCancelamento)
        {
            try
            {
                var resultado = await _servicePedido.CancelarPedidoAsync(idPedido, motivoCancelamento);
                if (resultado)
                {
                    TempData["Sucesso"] = "Pedido cancelado com sucesso!";
                }
                else
                {
                    TempData["Erro"] = "Erro ao cancelar pedido.";
                }
            }
            catch (Exception ex)
            {
                TempData["Erro"] = $"Erro ao cancelar pedido: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost("ExcluirPedido")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExcluirPedido(Guid idPedido)
        {
            try
            {
                var resultado = await _servicePedido.ExcluirPedidoAsync(idPedido);
                if (resultado)
                {
                    TempData["Sucesso"] = "Pedido excluído com sucesso!";
                }
                else
                {
                    TempData["Erro"] = "Erro ao excluir pedido.";
                }
            }
            catch (Exception ex)
            {
                TempData["Erro"] = $"Erro ao excluir pedido: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}