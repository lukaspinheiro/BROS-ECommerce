using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BROS_ECommerce.Domain.Entities
{
    public class Produto
    {
        public Produto() { }

        public Guid IdProduto { get; set; }
        public string Nome { get; set; }
        public string Slug { get; set; }
        public string TituloDescricao { get; set; }
        public string Descricao { get; set; }
        public decimal Preco { get; set; }
    }
}
