using BROS_ECommerce.Services.ViewModel.Promocao;

namespace BROS_ECommerce.Services.Interface.Services
{
    public interface IServicePromocao
    {
        Task<IEnumerable<PromocaoViewModel>> ObterTodosAsync();
        Task<IEnumerable<PromocaoViewModel>> ObterAtivosParaHomeAsync();
        Task AdicionarAsync(CadastrarPromocaoViewModel viewModel);
        Task AtualizarAsync(CadastrarPromocaoViewModel viewModel);
        Task ExcluirAsync(Guid id);
        Task<CadastrarPromocaoViewModel?> ObterPorIdAsync(Guid id);
    }
}
