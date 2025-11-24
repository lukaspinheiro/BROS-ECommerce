using BROS_ECommerce.Domain.Entities;
using BROS_ECommerce.Domain.Interfaces.Crud;

namespace BROS_ECommerce.Domain.Interfaces.Repository
{
    public interface IRepositoryImagem : IDisposable, IRepositoryBase<Imagem>, IAdicionarAsync<Imagem>, IAtualizarAsync<Imagem>, IBuscarPorIdAsync<Imagem>, IEncontrarAsync<Imagem>
    {
        Task<List<Imagem>> ObterTodosAsync();
        Task<List<Imagem>> ObterAtivosAsync();
        Task<Imagem?> ObterPorIdAsync(Guid id);
        Task<Imagem?> ObterLogosPorIdAsync(Guid id);
        Task<Imagem?> ObterPorNomeArquivoAsync(string nomeArquivo);
        Task<Imagem?> ObterPorCaminhoAsync(string caminho);
        Task<List<Imagem>> BuscarPorTipoMimeAsync(string tipoMime);
        Task<List<Imagem>> ObterPaginadoAsync(int pagina, int tamanhoPagina);
        Task<int> ContarTotalAsync();
        Task<int> ContarAtivosAsync();
        Task ExcluirAsync(Guid id);
        Task ExcluirLogicamenteAsync(Guid id);
        Task<List<Imagem>> ObterImagensNaoAssociadasAsync();
        Task<List<Imagem>> ObterImagensAssociadasAoProdutoAsync(Guid idProduto);
        Task<bool> NomeArquivoExisteAsync(string nomeArquivo);
        Task<long> ObterTamanhoTotalImagensAsync();
    }
}