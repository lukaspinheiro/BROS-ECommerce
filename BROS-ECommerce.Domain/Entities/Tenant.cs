using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BROS_ECommerce.Domain.Entities;

public class Tenant
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public string Id { get; set; }
    public string Nome { get; set; }

    #region Identidade Visual
    public string CorMenu { get; set; }
    public string CorTextoMenu { get; set; }

    public string CorMenuInferior { get; set; }
    public string CorTextoMenuInferior { get; set; }
    
    public string CorFundo { get; set; }
    public string CorTexto { get; set; }

    public Guid? IdLogo { get; set; }
    public Guid? IdFavicon { get; set; }
    #endregion

    #region Controle Interno
    public DateTime DataCriacao { get; set; }
    public bool Ativo { get; set; }
    #endregion

}
