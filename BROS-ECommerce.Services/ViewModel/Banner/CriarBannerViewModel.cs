using Microsoft.AspNetCore.Http;

namespace BROS_ECommerce.Services.ViewModel.Banner;

public class CriarBannerViewModel
{
    public string Titulo { get; set; }
    public string? Link { get; set; }

    public int Ordem { get; set; }
    public string? CaminhoImagem { get; set; }  

    public IFormFile? Arquivo { get; set; }
}
