using BROS_ECommerce.Domain.Entities;
using BROS_ECommerce.Domain.Interfaces.Repository;
using BROS_ECommerce.Services.Interface.Services;
using BROS_ECommerce.Services.ViewModel.Promocao;

namespace BROS_ECommerce.Services.Services
{
    public class ServicePromocao : IServicePromocao
    {
        private readonly IRepositoryPromocao _repoPromocao;
        private readonly IRepositoryProduto _repoProduto;

        public ServicePromocao(IRepositoryPromocao repoPromocao, IRepositoryProduto repoProduto)
        {
            _repoPromocao = repoPromocao;
            _repoProduto = repoProduto;
        }

        public async Task<IEnumerable<PromocaoViewModel>> ObterTodosAsync()
        {
            var promocoes = await _repoPromocao.ObterTodasComProdutoAsync();

            return promocoes.Select(p => new PromocaoViewModel
            {
                IdPromocao = p.IdPromocao,
                Nome = p.Nome,
                Descricao = p.Descricao,
                IdProduto = p.IdProduto,
                NomeProduto = p.Produto.Nome,
                ImagemProduto = p.Produto.ProdutoImagens.FirstOrDefault(i => i.Principal)?.Imagem.CaminhoArquivo ?? "",
                PrecoOriginal = p.Produto.Preco,
                ValorDesconto = p.ValorDesconto,
                PercentualDesconto = p.PercentualDesconto ?? Math.Round((p.ValorDesconto / p.Produto.Preco) * 100, 0),
                PrecoPromocional = p.Produto.Preco - p.ValorDesconto
            });
        }

        public async Task<IEnumerable<PromocaoViewModel>> ObterAtivosParaHomeAsync()
        {
            var promocoes = await _repoPromocao.ObterAtivasAsync();

            return promocoes.Select(p => new PromocaoViewModel
            {
                IdPromocao = p.IdPromocao,
                Nome = p.Nome,
                NomeProduto = p.Produto.Nome,
                Descricao = p.Descricao,
                IdProduto = p.IdProduto,
                ImagemProduto = p.Produto.ProdutoImagens.FirstOrDefault(i => i.Principal)?.Imagem.CaminhoArquivo ?? "",
                PrecoOriginal = p.Produto.Preco,
                ValorDesconto = p.ValorDesconto,
                PercentualDesconto = p.PercentualDesconto ?? Math.Round((p.ValorDesconto / p.Produto.Preco) * 100, 0),
                PrecoPromocional = p.Produto.Preco - p.ValorDesconto
            });
        }

        public async Task AdicionarAsync(CadastrarPromocaoViewModel vm)
        {
            var promocao = new Promocao
            {
                IdPromocao = Guid.NewGuid(),
                Nome = vm.Nome,
                Descricao = vm.Descricao,
                IdProduto = vm.IdProduto,
                PercentualDesconto = vm.PercentualDesconto,
                ValorDesconto = vm.ValorDesconto ?? 0,
                DataInicio = vm.DataInicio,
                DataFim = vm.DataFim,
                Ativo = true,
                DataCriacao = DateTime.UtcNow,
                DataAtualizacao = DateTime.UtcNow
            };

            await _repoPromocao.AdicionarAsync(promocao);
        }

        public async Task AtualizarAsync(CadastrarPromocaoViewModel vm)
        {
            var promocao = await _repoPromocao.ObterPorIdAsync(vm.IdPromocao.Value);
            if (promocao == null)
                throw new Exception("Promoção não encontrada");

            promocao.Nome = vm.Nome;
            promocao.Descricao = vm.Descricao;
            promocao.IdProduto = vm.IdProduto;
            promocao.PercentualDesconto = vm.PercentualDesconto;
            promocao.ValorDesconto = vm.ValorDesconto ?? 0;
            promocao.DataInicio = vm.DataInicio;
            promocao.DataFim = vm.DataFim;
            promocao.DataAtualizacao = DateTime.UtcNow;

            await _repoPromocao.AtualizarAsync(promocao);
        }

        public async Task ExcluirAsync(Guid id)
        {
            await _repoPromocao.ExcluirAsync(id);
        }

        public async Task<CadastrarPromocaoViewModel?> ObterPorIdAsync(Guid id)
        {
            var promocao = await _repoPromocao.ObterPorIdAsync(id);
            if (promocao == null)
                return null;

            return new CadastrarPromocaoViewModel
            {
                IdPromocao = promocao.IdPromocao,
                Nome = promocao.Nome,
                Descricao = promocao.Descricao,
                IdProduto = promocao.IdProduto,
                PercentualDesconto = promocao.PercentualDesconto,
                ValorDesconto = promocao.ValorDesconto,
                DataInicio = promocao.DataInicio,
                DataFim = promocao.DataFim
            };
        }
    }
}
