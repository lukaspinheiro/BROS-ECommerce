using BROS_ECommerce.Services.Interface.Services;
using Microsoft.AspNetCore.Mvc;
using BROS_ECommerce.Services.ViewModel.Usuario;

namespace BROS_ECommerce.Web.Areas.Administrativo.Controllers
{
    [Area("Administrativo")]
    [Route("Administrativo/Usuario")]
    public class UsuarioController : Controller
    {
        private readonly IServiceUser _serviceUser;

        public UsuarioController(IServiceUser serviceUser)
        {
            _serviceUser = serviceUser;
        }

        [HttpGet("Index")]
        public async Task<IActionResult> Index()
        {
            var usuarios = await _serviceUser.GetAllUsersAsync();
            var viewModel = new IndexUsuarioViewModel
            {
                Tabela = usuarios.Select(u => new UsuarioTabelaViewModel
                {
                    IdUser = u.IdUser,
                    Email = u.Email,
                    Cpf = u.Cpf,
                    Nome = u.Nome,
                    Nascimento = u.Nascimento,
                    Genero = u.Genero,
                    Perfil = u.Perfil,
                    Ativo = u.Ativo,
                    DataCriacao = u.DataCriacao,
                    DataAtualizacao = u.DataAtualizacao
                }).ToList()
            };

            return View(viewModel);
        }

        [HttpPost("Cadastrar")]
        public async Task<IActionResult> Cadastrar(CadastrarUsuarioViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Erro"] = "Dados inválidos!";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                await _serviceUser.CreateUserAsync(model.Email, model.Cpf, model.Nome, model.Nascimento, model.Senha, model.Genero, model.Perfil);
                TempData["Sucesso"] = "Usuário cadastrado com sucesso!";
            }
            catch (Exception ex)
            {
                TempData["Erro"] = $"Erro ao cadastrar: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost("Editar")]
        public async Task<IActionResult> Editar(UsuarioTabelaViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Erro"] = "Dados inválidos!";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                var user = await _serviceUser.GetUserByIdAsync(model.IdUser);
                if (user == null)
                {
                    TempData["Erro"] = "Usuário não encontrado!";
                    return RedirectToAction(nameof(Index));
                }

                user.Nome = model.Nome;
                user.Email = model.Email;
                user.Cpf = model.Cpf;
                user.Nascimento = model.Nascimento;
                user.Perfil = model.Perfil;
                user.Genero = model.Genero;
                user.DataAtualizacao = DateTime.UtcNow;

                await _serviceUser.UpdateUserAsync(user);
                TempData["Sucesso"] = "Usuário atualizado com sucesso!";
            }
            catch (Exception ex)
            {
                TempData["Erro"] = $"Erro ao atualizar: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost("Excluir/{id}")]
        public async Task<IActionResult> Excluir(Guid id)
        {
            try
            {
                await _serviceUser.DeleteUserAsync(id);
                TempData["Sucesso"] = "Usuário removido com sucesso!";
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                TempData["Erro"] = $"Erro ao excluir: {ex.Message}";
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
