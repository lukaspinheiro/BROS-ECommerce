using BROS_ECommerce.Domain.Entities;
using BROS_ECommerce.Domain.Interfaces.Repository;
using BROS_ECommerce.Services.Helpers;
using BROS_ECommerce.Services.Interface.Services;
using BROS_ECommerce.Services.ViewModel.Categoria;
using BROS_ECommerce.Services.ViewModel.CategoriaProduto;
using BROS_ECommerce.Services.ViewModel.Produto;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BROS_ECommerce.Services.Services
{
    public class ServiceCategoria : IServiceCategoria
    {
        private readonly IRepositoryCategoria _repositoryCategoria;
        private readonly IServiceCategoriaProduto _serviceCategoriaProduto;

        public ServiceCategoria(IRepositoryCategoria repositoryCategoria, IServiceCategoriaProduto serviceCategoriaProduto)
        {
            _repositoryCategoria = repositoryCategoria;
            _serviceCategoriaProduto = serviceCategoriaProduto;
        }


        public async Task<List<TabelaCategoriaViewModel>> ObterTabelaCategoriaAsync()
        {
            var categorias = await _repositoryCategoria.ObterTodasCategorias();

            return categorias.Select(c => new TabelaCategoriaViewModel(
                idCategoria: c.IdCategoria,
                nomeCategoria: c.NomeCategoria,
                descricao: c.Descricao ?? string.Empty,
                ativo: c.Ativo,
                dataCriacao: c.DataCriacao,
                dataAtualizacao: c.DataAtualizacao ?? DateTime.UtcNow
            )).ToList();
        }
        public async Task<List<Categoria>> ListarTodasAsync()
        {
            var categorias = await _repositoryCategoria.ObterTodasCategorias();
            return categorias.ToList();
        }


        public async Task AdicionarCategoriaAsync(CadastrarCategoriaViewModel CategoriaVM)
        {
            var CategoriaExiste = await _repositoryCategoria.CategoriaExisteAsync(CategoriaVM.NomeCategoria);
            if (CategoriaExiste)
            {
                throw new InvalidOperationException($"Já existe uma categoria com cadastrada com o nome '{CategoriaVM.NomeCategoria}'!");
            }

            var categoria = new Categoria
            {
                IdCategoria = Guid.NewGuid(),
                NomeCategoria = CategoriaVM.NomeCategoria,
                Descricao = CategoriaVM.Descricao,
                Ativo = CategoriaVM.Ativo,
                DataCriacao = TimeHelper.AgoraPortoVelho(),
                DataAtualizacao = TimeHelper.AgoraPortoVelho(),
            };

            await _repositoryCategoria.AdicionarAsync(categoria);
        }

        public async Task ExcluirAsync(Guid id)
        {
            try
            {
                await _serviceCategoriaProduto.RemoverPorCategoriaAsync(id);
                await _repositoryCategoria.ExcluirAsync(id);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Erro ao excluir Categoria: {ex.Message}");
            }
        }

        public async Task EditarCategoriaAsync(CadastrarCategoriaViewModel model)
        {
            var categoria = await _repositoryCategoria.BuscarPorIdAsync(model.IdCategoria);

            if (categoria == null || categoria.IdCategoria == Guid.Empty)
                throw new Exception("Categoria não encontrada.");

            categoria.NomeCategoria = model.NomeCategoria;
            categoria.Descricao = model.Descricao;
            categoria.Ativo = model.Ativo;
            categoria.DataAtualizacao = TimeHelper.AgoraPortoVelho();

            await _repositoryCategoria.AtualizarAsync(categoria);
        }
        public async Task<CategoriaViewModel?> ObterPorIdAsync(Guid idCategoria)
        {
            var categoria = await _repositoryCategoria.ObterPorIdAsync(idCategoria);

            if (categoria == null)
                return null;

            return new CategoriaViewModel
            {
                IdCategoria = categoria.IdCategoria,
                NomeCategoria = categoria.NomeCategoria,
                Descricao = categoria.Descricao
            };
        }

        public async Task<List<Categoria>> ListarParaMenuAsync()
        {
            var categorias = await _repositoryCategoria.ObterTodasCategorias();

            return categorias
                .Where(c => c.Ativo)
                .OrderBy(c => c.NomeCategoria)
                .ToList();
        }

    }
}
