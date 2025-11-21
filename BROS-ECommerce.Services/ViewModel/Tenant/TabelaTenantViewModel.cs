namespace BROS_ECommerce.Services.ViewModel.Tenant;

public class TabelaTenantViewModel
{
    public TabelaTenantViewModel()
    {
        Tenant = new List<TenantViewModel>();
    }

    public TabelaTenantViewModel(
        string id, string nome, string email, string telefone, string corMenu, string corTextoMenu, string corMenuInferior, 
        string corTextoMenuInferior, string corFundo, string corTexto, DateTime dataCriacao, bool ativo)
    {
        Id = id;
        Nome = nome;
        Email = email;
        Telefone = telefone;

        CorMenu = corMenu;
        CorTextoMenu = corTextoMenu;    

        CorMenuInferior = corMenuInferior;
        CorTextoMenuInferior = corTextoMenuInferior;

        CorFundo = corFundo;
        CorTexto = corTexto;

        DataCriacao = dataCriacao;
        Ativo = ativo;

        Tenant = new List<TenantViewModel>();
    }

    public List<TenantViewModel> Tenant { get; set; }
    public string Id { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Telefone { get; set; }
    public string CorMenu { get; set; }
    public string CorTextoMenu { get; set; }

    public string CorMenuInferior { get; set; }
    public string CorTextoMenuInferior { get; set; }

    public string CorFundo { get; set; }
    public string CorTexto { get; set; }

    public DateTime DataCriacao { get; set; }
    public bool Ativo { get; set; }


}
