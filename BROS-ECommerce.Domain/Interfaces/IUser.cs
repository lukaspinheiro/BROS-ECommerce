using BROS_ECommerce.Domain.Enums;
using System.Security.Claims;

namespace BROS_ECommerce.Domain.Interfaces;

public interface IUser
{
    bool Administrador { get; }
    bool PossuiPerfil(EPerfil perfil);
    string Nome { get; }
    bool EstaAutenticado();
    IEnumerable<Claim> ObterClaims();
    Guid? Id { get; }
    ClaimsPrincipal UserContext();
    public string cpf { get; }
    public string Email { get; set; }
}
