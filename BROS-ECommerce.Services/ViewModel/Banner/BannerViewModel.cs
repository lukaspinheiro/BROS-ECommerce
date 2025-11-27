namespace BROS_ECommerce.Services.ViewModel.Banner;

public class BannerViewModel
{
    public Guid IdBanner { get; set; }
    public string Titulo { get; set; }
    public string? Link { get; set; }
    public string CaminhoImagem { get; set; }
    public int Ordem { get; set; }
    public bool Ativo { get; set; }
}
