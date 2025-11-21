namespace BROS_ECommerce.Services.ViewModel.Tenant;

public class IndexTenantViewModel
{
	public IndexTenantViewModel()
	{
		Filtro = new FiltroTenantViewModel();
		Tabela = new List<TabelaTenantViewModel>();
		CadastrarTenant = new CadastrarTenantViewModel();
	}

	public IndexTenantViewModel(FiltroTenantViewModel filtro, List<TabelaTenantViewModel> tabela)
	{
        Filtro = filtro;
        Tabela = tabela;
		CadastrarTenant = new CadastrarTenantViewModel();
	}

    public FiltroTenantViewModel Filtro { get; set; } = new FiltroTenantViewModel();
    public ICollection<TabelaTenantViewModel> Tabela { get; set; } = new List<TabelaTenantViewModel>();
	public CadastrarTenantViewModel CadastrarTenant { get; set; } = new CadastrarTenantViewModel();

}
