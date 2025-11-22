using System.ComponentModel.DataAnnotations;

namespace BROS_ECommerce.Domain.Enums;

public enum EPerfil
{
    [Display(Name = "Super Administrador")]
    SuperAdmin = 1,

    [Display(Name = "Administrador")]
    TenantAdmin = 2,

    [Display(Name = "Usuário")]
    TenantUser = 3
}
