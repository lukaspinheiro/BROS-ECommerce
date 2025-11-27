using BROS_ECommerce.Services.Interface.Services;
using BROS_ECommerce.Services.ViewModel.Banner;
using Microsoft.AspNetCore.Mvc;

namespace BROS_ECommerce.Web.Areas.Administrativo.Controllers;

[Area("Administrativo")]
[Route("Administrativo/Banner")]

public class BannerController : Controller
{
    private readonly IServiceBanner _serviceBanner;

    public BannerController(IServiceBanner serviceBanner)
    {
        _serviceBanner = serviceBanner;
    }

    [HttpGet("Index")]
    public async Task<IActionResult> Index()
    {
        var banners = await _serviceBanner.ObterTodosAsync();
        return View(banners);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create(CriarBannerViewModel vm)
    {
        if (!ModelState.IsValid)
            return View(vm);

        await _serviceBanner.AdicionarAsync(vm);
        return RedirectToAction("Index");
    }
}

