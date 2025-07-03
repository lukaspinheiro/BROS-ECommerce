namespace BROS_ECommerce.Services.ViewModel.Estoque;

public class IndexEstoqueViewModel
{
    public IndexEstoqueViewModel()
    {
        Filtro = new FiltroEstoqueViewModel();
        Tabela = new List<TabelaEstoqueViewModel>();
        cadastrarEstoqueViewModel = new CadastrarEstoqueViewModel();
    }
    public IndexEstoqueViewModel(FiltroEstoqueViewModel filtro, List<TabelaEstoqueViewModel> tabela)
    {
        Filtro = filtro;
        Tabela = tabela;
        cadastrarEstoqueViewModel = new CadastrarEstoqueViewModel();
    }

    public FiltroEstoqueViewModel Filtro { get; set; } = new FiltroEstoqueViewModel();
    public ICollection<TabelaEstoqueViewModel> Tabela { get; set; } = new List<TabelaEstoqueViewModel>();
    public CadastrarEstoqueViewModel cadastrarEstoqueViewModel { get; set; } = new CadastrarEstoqueViewModel();
}
