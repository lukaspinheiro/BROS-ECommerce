namespace BROS_ECommerce.Services.ViewModel.Categoria;

public class IndexCategoriaViewModel
{
    public IndexCategoriaViewModel()    
    {
        Filtro = new FiltroCategoriaViewModel();
        Tabela = new List<TabelaCategoriaViewModel>();
    }
    public IndexCategoriaViewModel(FiltroCategoriaViewModel filtro, List<TabelaCategoriaViewModel> tabela)
    {
        Filtro = filtro;
        Tabela = tabela;
    }
    public FiltroCategoriaViewModel Filtro { get; set; } = new FiltroCategoriaViewModel();
    public ICollection<TabelaCategoriaViewModel> Tabela { get; set; }
    public CadastrarCategoriaViewModel cadastrarCategoriaViewModel { get; set; } = new CadastrarCategoriaViewModel();

}
