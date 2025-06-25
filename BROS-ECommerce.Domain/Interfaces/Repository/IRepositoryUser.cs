using BROS_ECommerce.Domain.Entities;

namespace BROS_ECommerce.Domain.Interfaces.Repository
{
    public interface IRepositoryUser
    {
        Task<User?> GetByIdAsync(Guid id);
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByCpfAsync(string cpf);
        Task<IEnumerable<User>> GetAllAsync();
        Task<IEnumerable<User>> GetActiveUsersAsync();
        Task<User> CreateAsync(User user);
        Task<User> UpdateAsync(User user);
        Task DeleteAsync(Guid id);
        Task<bool> EmailExistsAsync(string email);
        Task<bool> CpfExistsAsync(string cpf);
        Task<User?> ValidateUserAsync(string email, string senha);
    }
}