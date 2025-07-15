using BROS_ECommerce.Services.ViewModel.Carrinho;
using System.Threading.Tasks;

namespace BROS_ECommerce.Services.Interface.Services
{
    public interface IMercadoPagoService
    {
        Task<string> CriarPreferenciaAsync(CarrinhoViewModel carrinho);
    }
}
