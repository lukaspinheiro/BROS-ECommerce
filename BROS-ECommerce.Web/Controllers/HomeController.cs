using BROS_ECommerce.Services.Interface.Services;
using BROS_ECommerce.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BROS_ECommerce.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
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

        private readonly IServicePromocao _servicePromocao;

        public HomeController(IServicePromocao servicePromocao)
        {
            _servicePromocao = servicePromocao;
        }

        public async Task<IActionResult> Index()
        {
            var promocoes = await _servicePromocao.ObterAtivosParaHomeAsync();
            return View(promocoes); // você pode criar uma ViewModel mais completa se quiser
        }
    }
}
