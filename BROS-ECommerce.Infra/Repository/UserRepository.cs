
using BROS_ECommerce.Domain.Entities;
using BROS_ECommerce.Domain.Interfaces.Repository;
using BROS_ECommerce.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace BROS_ECommerce.Infra.Repository
{
    public class UserRepository : IRepositoryUser
    {
        private readonly BrosContext _context;

        public UserRepository(BrosContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.IdUser == id && u.Ativo);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower() && u.Ativo);
        }

        public async Task<User?> GetByCpfAsync(string cpf)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Cpf == cpf && u.Ativo);
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users
                .Where(u => u.Ativo)
                .OrderBy(u => u.Nome)
                .ToListAsync();
        }

        public async Task<IEnumerable<User>> GetActiveUsersAsync()
        {
            return await _context.Users
                .Where(u => u.Ativo)
                .OrderByDescending(u => u.DataCriacao)
                .ToListAsync();
        }

        public async Task<User> CreateAsync(User user)
        {
            user.DataCriacao = DateTime.UtcNow;
            _context.Users.Add(user);

            
            var result = await _context.SaveChangesAsync();
            if (!result)
            {
                throw new InvalidOperationException("Falha ao salvar usuário no banco de dados");
            }

            return user;
        }

        public async Task<User> UpdateAsync(User user)
        {
            user.DataAtualizacao = DateTime.UtcNow;
            _context.Users.Update(user);

            
            var result = await _context.SaveChangesAsync();
            if (!result)
            {
                throw new InvalidOperationException("Falha ao atualizar usuário no banco de dados");
            }

            return user;
        }

        public async Task DeleteAsync(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                
                user.Ativo = false;
                user.DataAtualizacao = DateTime.UtcNow;

                
                var result = await _context.SaveChangesAsync();
                if (!result)
                {
                    throw new InvalidOperationException("Falha ao desativar usuário no banco de dados");
                }
            }
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Users
                .AnyAsync(u => u.Email.ToLower() == email.ToLower() && u.Ativo);
        }

        public async Task<bool> CpfExistsAsync(string cpf)
        {
            return await _context.Users
                .AnyAsync(u => u.Cpf == cpf && u.Ativo);
        }

        public async Task<User?> ValidateUserAsync(string email, string senha)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u =>
                    u.Email.ToLower() == email.ToLower() &&
                    u.Senha == senha &&
                    u.Ativo);
        }

        public async Task<IEnumerable<User>> GetUsersByTenantAsync(string tenantId)
        {
            return await _context.Users
                .Where(u => u.TenantId == tenantId)
                .OrderBy(u => u.Nome)
                .ToListAsync();
        }

    }
}