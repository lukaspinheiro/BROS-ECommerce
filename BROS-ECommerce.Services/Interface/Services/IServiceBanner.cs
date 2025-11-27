using BROS_ECommerce.Services.ViewModel.Banner;
using Microsoft.AspNetCore.Http;

namespace BROS_ECommerce.Services.Interface.Services;

public interface IServiceBanner
{
    Task<List<BannerViewModel>> ObterTodosAsync();
    Task<BannerViewModel?> ObterPorIdAsync(Guid id);
    Task<Guid> AdicionarAsync(CriarBannerViewModel bannerVM);

    Task AtualizarAsync(BannerViewModel vm, IFormFile? arquivo = null);
    Task ExcluirAsync(Guid id);
    Task ExcluirLogicamenteAsync(Guid id);
}

