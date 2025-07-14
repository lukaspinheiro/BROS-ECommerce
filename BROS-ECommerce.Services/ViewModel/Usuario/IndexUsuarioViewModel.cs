using System.Collections.Generic;

namespace BROS_ECommerce.Services.ViewModel.Usuario
{
    public class IndexUsuarioViewModel
    {
        public IndexUsuarioViewModel()
        {
            Tabela = new List<UsuarioTabelaViewModel>();
            CadastrarUsuario = new CadastrarUsuarioViewModel();
            EditarUsuario = new EditarUsuarioViewModel();
        }

        public List<UsuarioTabelaViewModel> Tabela { get; set; }
        public CadastrarUsuarioViewModel CadastrarUsuario { get; set; }
        public EditarUsuarioViewModel EditarUsuario { get; set; }
    }
}
