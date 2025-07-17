using BROS_ECommerce.Domain.Interfaces;

namespace BROS_ECommerce.Domain.Entities
{
    public class Categoria : IAggregateRoot
    {
        public Categoria() { }

        public Categoria(string nomeCategoria, string descricao, bool ativo, DateTime dataCriacao, ICollection<CategoriaProduto> categoriaProdutos)
        {
            IdCategoria = Guid.NewGuid();
            NomeCategoria = nomeCategoria;
            Descricao = descricao;
            Ativo = ativo;
            DataCriacao = dataCriacao;
            DataAtualizacao = DateTime.UtcNow;
            CategoriaProdutos = categoriaProdutos;
        }

        public Guid IdCategoria { get; set; }
        public string NomeCategoria { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public bool Ativo { get; set; } = true;
        public DateTime DataCriacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }
        public virtual ICollection<CategoriaProduto> CategoriaProdutos { get; set; } = new List<CategoriaProduto>();
    }
}