using BROS_ECommerce.Services.ViewModel.CategoriaProduto;
using System.ComponentModel.DataAnnotations.Schema;

namespace BROS_ECommerce.Services.ViewModel.Categoria;

public class TabelaCategoriaViewModel
{
    public TabelaCategoriaViewModel(){ }

    public TabelaCategoriaViewModel(
        Guid idCategoria,
        string nomeCategoria,
        string descricao,
        bool ativo,
        DateTime dataCriacao,
        DateTime dataAtualizacao)
    {
        IdCategoria = idCategoria;
        NomeCategoria = nomeCategoria;
        Descricao = descricao;
        Ativo = ativo;
        DataCriacao = dataCriacao;
        DataAtualizacao = dataAtualizacao;
    }

    public Guid IdCategoria { get; set; }
    public string NomeCategoria { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public bool Ativo { get; set; } = true;
    public DateTime DataCriacao { get; set; }
    public DateTime? DataAtualizacao { get; set; }
}
