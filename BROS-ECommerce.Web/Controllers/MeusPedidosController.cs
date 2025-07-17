using BROS_ECommerce.Services.Interface.Services;
using BROS_ECommerce.Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BROS_ECommerce.Web.Controllers
{
    [Authorize]
    [Route("MeusPedidos")]
    public class MeusPedidosController : Controller
    {
        private readonly IServicePedido _servicePedido;

        public MeusPedidosController(IServicePedido servicePedido)
        {
            _servicePedido = servicePedido;
        }

        private Guid? ObterIdUsuarioLogado()
        {
            var userIdClaim = User.FindFirst("user_id")?.Value ??
                             User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (Guid.TryParse(userIdClaim, out var userId))
            {
                return userId;
            }
            return null;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var idUsuario = ObterIdUsuarioLogado();
                if (!idUsuario.HasValue)
                {
                    return RedirectToAction("Login", "Autenticacao");
                }

                var pedidos = await _servicePedido.ObterPedidosPorUsuarioAsync(idUsuario.Value);
                return View(pedidos);
            }
            catch (Exception ex)
            {
                TempData["Erro"] = "Erro ao carregar seus pedidos: " + ex.Message;
                return View(new List<BROS_ECommerce.Services.ViewModel.Pedido.TabelaPedidoViewModel>());
            }
        }

        [HttpGet("detalhes/{idPedido}")]
        public async Task<IActionResult> Detalhes(Guid idPedido)
        {
            try
            {
                var idUsuario = ObterIdUsuarioLogado();
                if (!idUsuario.HasValue)
                {
                    return RedirectToAction("Login", "Autenticacao");
                }

                var pedido = await _servicePedido.ObterDetalhesPedidoAsync(idPedido);
                if (pedido == null)
                {
                    TempData["Erro"] = "Pedido não encontrado.";
                    return RedirectToAction(nameof(Index));
                }

                if (pedido.Cliente.IdUsuario != idUsuario.Value)
                {
                    TempData["Erro"] = "Você não tem permissão para ver este pedido.";
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

        [HttpPost("CancelarPedido")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelarPedido(Guid idPedido, string motivoCancelamento)
        {
            try
            {
                var idUsuario = ObterIdUsuarioLogado();
                if (!idUsuario.HasValue)
                {
                    return RedirectToAction("Login", "Autenticacao");
                }

                var pedido = await _servicePedido.ObterDetalhesPedidoAsync(idPedido);
                if (pedido == null || pedido.Cliente.IdUsuario != idUsuario.Value)
                {
                    TempData["Erro"] = "Pedido não encontrado ou você não tem permissão para cancelá-lo.";
                    return RedirectToAction(nameof(Index));
                }

                if (pedido.Status != "Pendente" && pedido.Status != "Confirmado")
                {
                    TempData["Erro"] = "Este pedido não pode ser cancelado.";
                    return RedirectToAction(nameof(Index));
                }

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
    }
}