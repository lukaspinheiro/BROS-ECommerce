using BROS_ECommerce.Services.Interface.Services;
using BROS_ECommerce.Services.ViewModel.Home;
using BROS_ECommerce.Services.ViewModel.Promocao;
using Microsoft.AspNetCore.Mvc;

namespace BROS_ECommerce.Web.Areas.Administrativo.Controllers
{
    [Area("Administrativo")]
    [Route("Administrativo/Promocao")]
    public class PromocaoController : Controller
    {
        private readonly IServicePromocao _servicePromocao;
        private readonly IServiceProduto _serviceProduto;

        public PromocaoController(IServicePromocao servicePromocao, IServiceProduto serviceProduto)
        {
            _servicePromocao = servicePromocao;
            _serviceProduto = serviceProduto;
        }

        [HttpGet("Index")]
        public async Task<IActionResult> Index()
        {
            var promocoes = await _servicePromocao.ObterAtivosParaHomeAsync();
            var viewModel = new HomeViewModel
            {
                Promocoes = promocoes
            };
            return View(viewModel);
        }


        [HttpPost("Cadastrar")]
        public async Task<IActionResult> Cadastrar(CadastrarPromocaoViewModel vm)
        {
            await _servicePromocao.AdicionarAsync(vm);
            TempData["Sucesso"] = "Promoção cadastrada com sucesso!";
            return RedirectToAction("Index");
        }

        [HttpPost("Atualizar")]
        public async Task<IActionResult> Atualizar(CadastrarPromocaoViewModel vm)
        {
            await _servicePromocao.AtualizarAsync(vm);
            TempData["Sucesso"] = "Promoção atualizada com sucesso!";
            return RedirectToAction("Index");
        }

        [HttpPost("Excluir/{id}")]
        public async Task<IActionResult> Excluir(Guid id)
        {
            await _servicePromocao.ExcluirAsync(id);
            TempData["Sucesso"] = "Promoção excluída!";
            return RedirectToAction("Index");
        }
    }
}
