
using BROS_ECommerce.Domain.Interfaces;

namespace BROS_ECommerce.Domain.Entities
{
    public class Carrinho : IAggregateRoot, ITemTenant
    {
        public Carrinho() { }

        public Guid IdCarrinho { get; set; }
        public string TenantId { get; set; }
        public Guid? IdUsuario { get; set; }
        public DateTime DataCriacao { get; set; }
        public string Status { get; set; } = "Aberto";

        
        public virtual User? Usuario { get; set; }
        public virtual ICollection<CarrinhoItem> Itens { get; set; } = new List<CarrinhoItem>();
    }
}