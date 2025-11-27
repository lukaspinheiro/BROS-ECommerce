using BROS_ECommerce.Services.ViewModel.Banner;

namespace BROS_ECommerce.Services.Interface.Services;

public interface IServiceBanner
{
    Task<List<BannerViewModel>> ObterTodosAsync();
    Task<BannerViewModel?> ObterPorIdAsync(Guid id);
    Task<Guid> AdicionarAsync(CriarBannerViewModel bannerVM);

    Task AtualizarAsync(BannerViewModel bannerVM);
    Task ExcluirAsync(Guid id);
    Task ExcluirLogicamenteAsync(Guid id);
}

