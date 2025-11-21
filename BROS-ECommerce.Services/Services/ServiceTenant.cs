using BROS_ECommerce.Domain.Entities;
using BROS_ECommerce.Domain.Interfaces.Repository;
using BROS_ECommerce.Services.Helpers;
using BROS_ECommerce.Services.Interface.Services;
using BROS_ECommerce.Services.ViewModel.Tenant;

namespace BROS_ECommerce.Services.Services;

public class ServiceTenant : IServiceTenant
{
    private readonly IRepositoryTenant _repositoryTenant;
    public ServiceTenant(IRepositoryTenant repositoryTenant)
    {
        _repositoryTenant = repositoryTenant;
    }

    public async Task<List<TabelaTenantViewModel>> ObterTabelaTenantsAsync()
    {
        var tenants = await _repositoryTenant.ObterTodosAsync();

        return tenants.Select(e => new TabelaTenantViewModel
        {
            Id = e.Id,
            Nome = e.Nome,
            Telefone = e.Telefone,
            Email = e.Email,
            DataCriacao = e.DataCriacao,
            Ativo = e.Ativo
        }).ToList();
    }

    public async Task CadastrarTenantAsync(IndexTenantViewModel indexTenantViewModel)
    {
        var vm = indexTenantViewModel.CadastrarTenant;

        var tenantExiste = await _repositoryTenant.TenantExisteAsync(vm.Id);
        if (tenantExiste)
            throw new InvalidOperationException($"Já existe uma loja com o Domínio: '{vm.Id}'");

        var tenant = new Tenant
        {
            Id = vm.Id,
            Nome = vm.Nome,
            Email = vm.Email,
            Telefone = vm.Telefone,
            Ativo = true,
            DataCriacao = TimeHelper.AgoraPortoVelho(),
            
            CorMenu = "#FF4E4E",
            CorTextoMenu = "#FFFFFF",
            CorMenuInferior = "#FFFFFF",
            CorTextoMenuInferior = "#FF4E4E",
            CorFundo = "#FFFFFF",
            CorTexto = "#FF4E4E"

        };

        await _repositoryTenant.AdicionarAsync(tenant);
    }

    public async Task EditarTenantAsync(IndexTenantViewModel indexTenantViewModel)
    {
        var vm = indexTenantViewModel.CadastrarTenant;

        var tenant = await _repositoryTenant.BuscarPorId(vm.Id);

        if (tenant == null)
            throw new Exception("Loja não encontrada.");

        tenant.Nome = vm.Nome;
        tenant.Email = vm.Email;
        tenant.Telefone = vm.Telefone;
        tenant.Ativo = vm.Ativo;

        await _repositoryTenant.AtualizarAsync(tenant);
    }

}
