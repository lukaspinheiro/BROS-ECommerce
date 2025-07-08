using System;

namespace BROS_ECommerce.Services.ViewModel.Usuario
{
    public class UsuarioTabelaViewModel
    {
        public Guid IdUser { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Cpf { get; set; }
        public string Genero { get; set; }
        public DateTime Nascimento { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataCriacao { get; set; }

        public DateTime? DataAtualizacao { get; set; }
    }
}
