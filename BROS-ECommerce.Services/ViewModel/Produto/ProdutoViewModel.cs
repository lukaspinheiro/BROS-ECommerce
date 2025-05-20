using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BROS_ECommerce.Services.ViewModel.Produto
{
    public class ProdutoViewModel
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Slug { get; set; }
        public string TituloDescricao { get; set; }
        public string Descricao { get; set; }
        public decimal Preco { get; set; }
        public List<string> Imagens { get; set; } = new();
    }

}
