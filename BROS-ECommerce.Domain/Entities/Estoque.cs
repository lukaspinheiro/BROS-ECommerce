using BROS_ECommerce.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BROS_ECommerce.Domain.Entities
{
    public class Estoque : IAggregateRoot
    {
        public Estoque() { }

        public Guid IdEstoque { get; set; }
        public Guid IdProduto { get; set; }
        public string Nome { get; set; }
        public DateTime UltimaAtualizacao { get; set; }
        public int Quantidade { get; set; }
    }
}
