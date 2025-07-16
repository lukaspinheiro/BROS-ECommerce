using BROS_ECommerce.Services.ViewModel.Carrinho;
using System.Threading.Tasks;

namespace BROS_ECommerce.Services.Interface.Services
{
    public interface IStripeService
    {
        Task<string> CriarSessaoCheckoutAsync(CarrinhoViewModel carrinho);
    }
}
