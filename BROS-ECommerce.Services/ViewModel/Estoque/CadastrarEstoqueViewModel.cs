using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BROS_ECommerce.Services.ViewModel.Estoque
{
    public class CadastrarEstoqueViewModel
    {
        public CadastrarEstoqueViewModel()
        {
        }
        public Guid IdEstoque { get; set; }
        public Guid IdProduto { get; set; }
        public string Nome { get; set; }
        public DateTime UltimaAtualizacao { get; set; }

        [RegularExpression(@"^(?!\s*$).+", ErrorMessage = "Este campo não pode ser nulo.")]
        public int Quantidade { get; set; }

    }
}
