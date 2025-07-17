using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BROS_ECommerce.Services.ViewModel.CategoriaProduto
{
    public class ProdutoCategoriasModalViewModel
    {
        public Guid IdProduto { get; set; }
        public List<SelectListItem> CategoriasDisponiveis { get; set; } = new();
        public List<CategoriaProdutoViewModel> CategoriasAssociadas { get; set; } = new();
    }
}
