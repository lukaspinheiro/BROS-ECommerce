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
        [Required]
        public Guid IdProduto { get; set; }
        public DateTime UltimaAtualizacao { get; set; }

        [Required(ErrorMessage = "A quantidade é obrigatória.")]
        [Range(0, int.MaxValue, ErrorMessage = "A quantidade deve ser um número igual ou maior que 0.")]
        public int Quantidade { get; set; }
        public string Nome { get; set; }

    }
}
