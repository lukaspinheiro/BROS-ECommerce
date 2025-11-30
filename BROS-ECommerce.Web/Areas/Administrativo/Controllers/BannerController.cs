using BROS_ECommerce.Services.Interface.Services;
using BROS_ECommerce.Services.ViewModel.Banner;
using Microsoft.AspNetCore.Mvc;

namespace BROS_ECommerce.Web.Areas.Administrativo.Controllers;

[Area("Administrativo")]
[Route("Administrativo/Banner")]

public class BannerController : BaseAdminController
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
        return PartialView("Partials/_CriarBanner", new CriarBannerViewModel());
    }


    [HttpPost("Create")]
    public async Task<IActionResult> Create(CriarBannerViewModel vm)
    {
        if (!ModelState.IsValid)
            return View(vm);

        await _serviceBanner.AdicionarAsync(vm);
        return RedirectToAction("Index");
    }

    [HttpGet("Edit/{id:guid}")]
    public async Task<IActionResult> Edit(Guid id)
    {
        var banner = await _serviceBanner.ObterPorIdAsync(id);
        if (banner == null) return NotFound();

        return PartialView("Partials/_EditarBanner", banner);
    }

    [HttpPost("Edit/{id:guid}")]
    public async Task<IActionResult> Edit(Guid id, BannerViewModel vm)
    {
        await _serviceBanner.AtualizarAsync(vm, Request.Form.Files["Arquivo"]);
        return RedirectToAction("Index");
    }


    [HttpGet("Delete/{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var banner = await _serviceBanner.ObterPorIdAsync(id);
        if (banner == null) return NotFound();

        return PartialView("Partials/_ExcluirBanner", banner);
    }

    [HttpPost("DeleteConfirmed/{id:guid}")]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        await _serviceBanner.ExcluirAsync(id);
        return RedirectToAction("Index");
    }
}

