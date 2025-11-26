using BROS_ECommerce.Domain.Entities;
using BROS_ECommerce.Domain.Interfaces.Repository;
using BROS_ECommerce.Services.Helpers;
using BROS_ECommerce.Services.Interface.Services;
using BROS_ECommerce.Services.ViewModel.Imagem;
using BROS_ECommerce.Services.ViewModel.Personalizar;
using BROS_ECommerce.Services.ViewModel.Tenant;
using Microsoft.AspNetCore.Http;

namespace BROS_ECommerce.Services.Services;

public class ServiceTenant : IServiceTenant
{
    private readonly IRepositoryTenant _repositoryTenant;
    private readonly IServiceImagem _serviceImagem;
    public ServiceTenant(IRepositoryTenant repositoryTenant, IServiceImagem serviceImagem)
    {
        _repositoryTenant = repositoryTenant;
        _serviceImagem = serviceImagem;
    }

    public async Task<List<TabelaTenantViewModel>> ObterTabelaTenantsAsync()
    {
        var tenants = await _repositoryTenant.ObterTodosAsync();
        var lista = new List<TabelaTenantViewModel>();

        foreach (var e in tenants)
        {
            string? logoUrl = null;

            if (e.IdLogo.HasValue)
            {
                var imagem = await _serviceImagem.ObterImagemPorIdAsync(e.IdLogo.Value);
                if (imagem != null)
                    logoUrl = imagem.CaminhoArquivo;
            }

            lista.Add(new TabelaTenantViewModel
            {
                Id = e.Id,
                Nome = e.Nome,
                Telefone = e.Telefone,
                Email = e.Email,
                DataCriacao = e.DataCriacao,
                Ativo = e.Ativo,
                LogoUrl = logoUrl,
                CorMenu = e.CorMenu
            });
        }
        return lista;
    }

    public async Task CadastrarTenantAsync(IndexTenantViewModel indexTenantViewModel)
    {
        var vm = indexTenantViewModel.CadastrarTenant;

        var tenantExiste = await _repositoryTenant.TenantExisteAsync(vm.Id);
        if (tenantExiste)
            throw new InvalidOperationException($"Já existe uma loja com o Domínio: '{vm.Id}'");

        Guid? idLogo = null;

        if (vm.LogoArquivo != null)
        {
            var imagemVM = new CadastrarImagemViewModel
            {
                Arquivos = new List<IFormFile> { vm.LogoArquivo },
                AltText = $"Logo da loja {vm.Nome}"
            };

            idLogo = await _serviceImagem.AdicionarAsync(imagemVM);
        }

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
            CorTexto = "#FF4E4E",
            IdLogo = idLogo

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

    public async Task AtualizarTemaAsync(Tenant tenant, PersonalizarLojaViewModel model)
    {
        // Atualiza as cores
        tenant.CorMenu = model.CorMenu;
        tenant.CorTextoMenu = model.CorTextoMenu;

        tenant.CorFundo = model.CorFundo;
        tenant.CorTexto = model.CorTexto;

        tenant.CorMenuInferior = model.CorMenuInferior;
        tenant.CorTextoMenuInferior = model.CorTextoMenuInferior;

        // Atualiza a logo (opcional)
        if (model.LogoArquivo != null)
        {
            var imagemVM = new CadastrarImagemViewModel
            {
                Arquivos = new List<IFormFile> { model.LogoArquivo },
                AltText = $"Logo da loja {tenant.Nome}"
            };

            var novaLogoId = await _serviceImagem.AdicionarAsync(imagemVM);
            tenant.IdLogo = novaLogoId;
        }

        await _repositoryTenant.AtualizarAsync(tenant);
    }

}
