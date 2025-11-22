using BROS_ECommerce.Services.Interface.Services;
using BROS_ECommerce.Services.ViewModel.Usuario;
using Microsoft.AspNetCore.Mvc;

namespace BROS_ECommerce.Web.Areas.AdministrativoSuper.Controllers
{
    public class TenantUserController : BaseSuperAdminController
    {
        private readonly IServiceUser _userService;

        public TenantUserController(IServiceUser userService)
        {
            _userService = userService;
        }

        public async Task<IActionResult> Index(string tenantId)
        {
            if (string.IsNullOrWhiteSpace(tenantId))
                return BadRequest("TenantId é obrigatório.");

            var usuarios = await _userService.GetUsersByTenantAsync(tenantId);
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
                    Ativo = u.Ativo,
                    DataCriacao = u.DataCriacao,
                    DataAtualizacao = u.DataAtualizacao
                }).ToList()
            };

            ViewBag.TenantId = tenantId;

            return View(viewModel);
        }
    }
}
