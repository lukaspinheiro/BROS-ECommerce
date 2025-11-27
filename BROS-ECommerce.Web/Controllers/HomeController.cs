using BROS_ECommerce.Services.Interface.Services;
using BROS_ECommerce.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BROS_ECommerce.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IServiceProduto _serviceProduto;

        public HomeController(ILogger<HomeController> logger, IServiceProduto serviceProduto)
        {
            _logger = logger;
            _serviceProduto = serviceProduto;
        }

        public async Task<IActionResult> Index()
        {
            var produtos = await _serviceProduto.ObterTabelaProdutosAsync();
            return View(produtos);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
