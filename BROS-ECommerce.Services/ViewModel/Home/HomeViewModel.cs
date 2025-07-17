using BROS_ECommerce.Services.ViewModel.Promocao;

namespace BROS_ECommerce.Services.ViewModel.Home
{
    public class HomeViewModel
    {
        public IEnumerable<PromocaoViewModel> Promocoes { get; set; } = new List<PromocaoViewModel>();
    }
}
