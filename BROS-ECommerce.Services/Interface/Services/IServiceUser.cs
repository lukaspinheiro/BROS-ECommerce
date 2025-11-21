using BROS_ECommerce.Domain.Entities;
using BROS_ECommerce.Services.ViewModel.Usuario;
using System.Security.Claims;

namespace BROS_ECommerce.Services.Interface.Services
{
    public interface IServiceUser
    {
        Task<User?> GetUserByIdAsync(Guid id);
        Task<User?> GetUserByEmailAsync(string email);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> CreateUserAsync(string email, string cpf, string nome, DateTime nascimento, string senha, string genero);
        Task<User> UpdateUserAsync(User user);
        Task DeleteUserAsync(Guid id);
        Task<bool> ValidateUserCredentialsAsync(string email, string senha);
        Task<User?> AuthenticateUserAsync(string email, string senha);
        Task<bool> IsEmailAvailableAsync(string email);
        Task<bool> IsCpfAvailableAsync(string cpf);
        string HashPassword(string password);
        bool VerifyPassword(string password, string hash);
        Task<User?> GetByEmailAsync(string email);
        Task<bool> ResetarSenhaAsync(Guid id, string novaSenha);
        Task<PerfilUsuarioViewModel?> ObterUsuarioLogadoAsync(ClaimsPrincipal user);
        Task<IEnumerable<User>> GetUsersByTenantAsync(string tenantId);


    }
}