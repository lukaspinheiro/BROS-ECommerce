using BROS_ECommerce.Services.Interface.Services;
using BROS_ECommerce.Services.Services;
using BROS_ECommerce.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BROS_ECommerce.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IServiceProduto _serviceProduto;
        private readonly IServiceBanner _serviceBanner;

        public HomeController(ILogger<HomeController> logger, IServiceProduto serviceProduto, IServiceBanner serviceBanner)
        {
            _logger = logger;
            _serviceProduto = serviceProduto;
            _serviceBanner = serviceBanner;
        }

        public async Task<IActionResult> Index()
        {
            var produtos = await _serviceProduto.ObterTabelaProdutosAsync();
            var banners = await _serviceBanner.ObterTodosAsync();

            ViewBag.Banners = banners;

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
