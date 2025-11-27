using BROS_ECommerce.Domain.Entities;
using BROS_ECommerce.Domain.Interfaces.Repository;
using BROS_ECommerce.Services.Interface.Services;
using BROS_ECommerce.Services.ViewModel.Banner;

namespace BROS_ECommerce.Services.Services;

public class ServiceBanner : IServiceBanner
{
    private readonly IRepositoryBanner _repositoryBanner;

    public ServiceBanner(IRepositoryBanner repositoryBanner)
    {
        _repositoryBanner = repositoryBanner;
    }

    public async Task<List<BannerViewModel>> ObterTodosAsync()
    {
        var banners = await _repositoryBanner.ObterTodosAsync();

        return banners.Select(b => new BannerViewModel
        {
            IdBanner = b.IdBanner,
            Titulo = b.Titulo,
            CaminhoImagem = b.CaminhoImagem,
            Link = b.Link,
            Ordem = b.Ordem,
            Ativo = b.Ativo
        }).ToList();
    }

    public async Task<BannerViewModel?> ObterPorIdAsync(Guid id)
    {
        var b = await _repositoryBanner.ObterPorIdAsync(id);
        if (b == null) return null;

        return new BannerViewModel
        {
            IdBanner = b.IdBanner,
            Titulo = b.Titulo,
            CaminhoImagem = b.CaminhoImagem,
            Link = b.Link,
            Ordem = b.Ordem,
            Ativo = b.Ativo
        };
    }

    public async Task<Guid> AdicionarAsync(CriarBannerViewModel bannerVM)
    {
        string? caminhoImagem = null;

        if (bannerVM.Arquivo != null)
        {
            var folderPath = Path.Combine("wwwroot", "uploads", "banners");
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var nomeArquivo = Guid.NewGuid() + Path.GetExtension(bannerVM.Arquivo.FileName);
            var filePath = Path.Combine(folderPath, nomeArquivo);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await bannerVM.Arquivo.CopyToAsync(stream);
            }

            caminhoImagem = "/uploads/banners/" + nomeArquivo;
        }

        var banner = new Banner
        {
            IdBanner = Guid.NewGuid(),
            Titulo = bannerVM.Titulo,
            Link = bannerVM.Link,
            Ordem = bannerVM.Ordem,
            CaminhoImagem = caminhoImagem,
            Ativo = true,
            DataCriacao = DateTime.UtcNow
        };

        await _repositoryBanner.AdicionarAsync(banner);
        return banner.IdBanner;
    }


    public async Task AtualizarAsync(BannerViewModel vm)
    {
        var banner = await _repositoryBanner.ObterPorIdAsync(vm.IdBanner);

        banner.Titulo = vm.Titulo;
        banner.CaminhoImagem = vm.CaminhoImagem;
        banner.Link = vm.Link;
        banner.Ordem = vm.Ordem;
        banner.Ativo = vm.Ativo;

        await _repositoryBanner.AtualizarAsync(banner);
    }

    public async Task ExcluirAsync(Guid id)
    {
        await _repositoryBanner.ExcluirAsync(id);
    }

    public async Task ExcluirLogicamenteAsync(Guid id)
    {
        await _repositoryBanner.ExcluirLogicamenteAsync(id);
    }
}
