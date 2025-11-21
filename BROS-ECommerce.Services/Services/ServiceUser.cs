
using BROS_ECommerce.Domain.Entities;
using BROS_ECommerce.Domain.Interfaces.Repository;
using BROS_ECommerce.Services.Interface.Services;
using BROS_ECommerce.Services.ViewModel.Usuario;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BROS_ECommerce.Services.Services
{
    public class ServiceUser : IServiceUser

    {
        private readonly IRepositoryUser _userRepository;

        public ServiceUser(IRepositoryUser userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User?> GetUserByIdAsync(Guid id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _userRepository.GetByEmailAsync(email);
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        public async Task<User> CreateUserAsync(string email, string cpf, string nome, DateTime nascimento, string senha, string genero)
        {
            
            if (await _userRepository.EmailExistsAsync(email))
                throw new InvalidOperationException("Email já cadastrado no sistema.");

            
            cpf = new string(cpf.Where(char.IsDigit).ToArray());

            if (await _userRepository.CpfExistsAsync(cpf))
                throw new InvalidOperationException("CPF já cadastrado no sistema.");

            
            Console.WriteLine($"[DEBUG] CPF recebido: {cpf}");
            Console.WriteLine($"[DEBUG] CPF length: {cpf.Length}");

            
            if (!IsValidCpf(cpf))
            {
                Console.WriteLine($"[DEBUG] CPF inválido: {cpf}");
                throw new ArgumentException($"CPF inválido: {cpf}");
            }

            Console.WriteLine($"[DEBUG] CPF válido: {cpf}");

            
            var idade = DateTime.Now.Year - nascimento.Year;
            if (DateTime.Now.DayOfYear < nascimento.DayOfYear) idade--;

            if (idade < 16)
                throw new ArgumentException("Usuário deve ter pelo menos 16 anos.");

            var user = new User
            {
                IdUser = Guid.NewGuid(),
                Email = email.ToLower().Trim(),
                Cpf = cpf,
                Nome = nome.Trim(),
                Nascimento = nascimento,
                Genero = genero,
                DataCriacao = DateTime.UtcNow,
                Ativo = true
            };

            
            user.Senha = HashPassword(senha);

            return await _userRepository.CreateAsync(user);
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            return await _userRepository.UpdateAsync(user);
        }

        public async Task DeleteUserAsync(Guid id)
        {
            await _userRepository.DeleteAsync(id);
        }

        public async Task<bool> ValidateUserCredentialsAsync(string email, string senha)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null) return false;

            return VerifyPassword(senha, user.Senha);
        }

        public async Task<User?> AuthenticateUserAsync(string email, string senha)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user != null && VerifyPassword(senha, user.Senha))
            {
                return user;
            }
            return null;
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _userRepository.GetByEmailAsync(email);
        }

        public async Task<PerfilUsuarioViewModel?> ObterUsuarioLogadoAsync(ClaimsPrincipal user)
        {
            var email = user?.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email))
                return null;

            var usuario = await _userRepository.GetByEmailAsync(email);

            if (usuario == null || !usuario.Ativo)
                return null;

            return new PerfilUsuarioViewModel
            {
                Nome = usuario.Nome,
                Email = usuario.Email,
                Cpf = usuario.Cpf,
                Nascimento = usuario.Nascimento,
                Genero = usuario.Genero
            };
        }

        public async Task<bool> ResetarSenhaAsync(Guid id, string novaSenha)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return false;

            user.Senha = HashPassword(novaSenha);
            await _userRepository.UpdateAsync(user);
            return true;
        }



        public async Task<bool> IsEmailAvailableAsync(string email)
        {
            return !await _userRepository.EmailExistsAsync(email);
        }

        public async Task<bool> IsCpfAvailableAsync(string cpf)
        {
            
            cpf = new string(cpf.Where(char.IsDigit).ToArray());
            return !await _userRepository.CpfExistsAsync(cpf);
        }

        public string HashPassword(string password)
        {
            
            using (var sha256 = SHA256.Create())
            {
                
                var saltedPassword = password + "BROS_SALT_2025";
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(saltedPassword));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        public bool VerifyPassword(string password, string hash)
        {
            var passwordHash = HashPassword(password);
            return passwordHash == hash;
        }

        private static bool IsValidCpf(string cpf)
        {
            Console.WriteLine($"[DEBUG] Validando CPF: {cpf}");

            
            if (cpf.Length != 11)
            {
                Console.WriteLine($"[DEBUG] CPF length inválido: {cpf.Length}");
                return false;
            }

            
            if (cpf.All(c => c == cpf[0]))
            {
                Console.WriteLine($"[DEBUG] CPF com todos dígitos iguais: {cpf}");
                return false;
            }

            
            if (!cpf.All(char.IsDigit))
            {
                Console.WriteLine($"[DEBUG] CPF contém caracteres não numéricos: {cpf}");
                return false;
            }

            try
            {
                
                var sum = 0;
                for (int i = 0; i < 9; i++)
                {
                    sum += int.Parse(cpf[i].ToString()) * (10 - i);
                }

                var remainder = sum % 11;
                var digit1 = remainder < 2 ? 0 : 11 - remainder;

                if (int.Parse(cpf[9].ToString()) != digit1)
                {
                    Console.WriteLine($"[DEBUG] Primeiro dígito verificador inválido. Esperado: {digit1}, Recebido: {cpf[9]}");
                    return false;
                }

                
                sum = 0;
                for (int i = 0; i < 10; i++)
                {
                    sum += int.Parse(cpf[i].ToString()) * (11 - i);
                }

                remainder = sum % 11;
                var digit2 = remainder < 2 ? 0 : 11 - remainder;

                if (int.Parse(cpf[10].ToString()) != digit2)
                {
                    Console.WriteLine($"[DEBUG] Segundo dígito verificador inválido. Esperado: {digit2}, Recebido: {cpf[10]}");
                    return false;
                }

                Console.WriteLine($"[DEBUG] CPF válido: {cpf}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DEBUG] Erro na validação do CPF: {ex.Message}");
                return false;
            }
        }

        public async Task<IEnumerable<User>> GetUsersByTenantAsync(string tenantId)
        {
            return await _userRepository.GetUsersByTenantAsync(tenantId);
        }

    }
}