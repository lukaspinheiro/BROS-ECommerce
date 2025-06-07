using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BROS_ECommerce.Services.ViewModel.Produto
{
    public class TabelaProdutoViewModel
    {
        public TabelaProdutoViewModel() 
        {
            Produto = new List<ProdutoViewModel>();

        }
        public TabelaProdutoViewModel(Guid idProduto, string nome, string slug, string tituloDescricao, string descricao, decimal preco)
        {
            IdProduto = idProduto;
            Nome = nome;
            Slug = slug;
            TituloDescricao = tituloDescricao;
            Descricao = descricao;
            Preco = preco;
        }
        public List<ProdutoViewModel> Produto { get; set; }
        public Guid IdProduto { get; set; }
        public string Nome { get; set; }
        public string Slug { get; set; }
        public string TituloDescricao { get; set; }
        public string Descricao { get; set; }
        public decimal Preco { get; set; }

    }
}
