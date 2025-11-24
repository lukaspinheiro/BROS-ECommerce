using BROS_ECommerce.Services.ViewModel.Imagem;
using Microsoft.AspNetCore.Http;

namespace BROS_ECommerce.Services.Interface.Services
{
    public interface IServiceImagem
    {
        Task<List<ImagemViewModel>> ObterTodosAsync();
        Task<List<ImagemViewModel>> ObterAtivosAsync();
        Task<ImagemViewModel?> ObterPorIdAsync(Guid id);
        Task<ImagemViewModel?> ObterImagemPorIdAsync(Guid id);
        Task<List<ImagemViewModel>> ObterPaginadoAsync(int pagina, int itensPorPagina, FiltroImagemViewModel? filtro = null);
        Task<List<ImagemViewModel>> ObterImagensNaoAssociadasAsync();
        Task<List<ImagemViewModel>> ObterImagensPorProdutoAsync(Guid idProduto);

        Task<Guid> AdicionarAsync(CadastrarImagemViewModel imagemVM);
        Task<List<Guid>> AdicionarMultiplasAsync(CadastrarImagemViewModel imagemVM);
        Task AtualizarAsync(ImagemViewModel imagemVM);
        Task ExcluirAsync(Guid id);
        Task ExcluirLogicamenteAsync(Guid id);

        Task<List<ImagemViewModel>> BuscarPorNomeAsync(string nome);
        Task<List<ImagemViewModel>> BuscarPorTipoMimeAsync(string tipoMime);
        Task<bool> NomeArquivoExisteAsync(string nomeArquivo);
        Task<int> ContarTotalAsync();
        Task<int> ContarAtivosAsync();
        Task<long> ObterTamanhoTotalAsync();

        
        Task AssociarImagemAoProdutoAsync(Guid idProduto, Guid idImagem, bool principal = false, int ordem = 0);
        Task AssociarImagensAoProdutoAsync(Guid idProduto, List<Guid> idsImagens);
        Task RemoverAssociacaoProdutoAsync(Guid idProduto, Guid idImagem);
        Task DefinirImagemPrincipalAsync(Guid idProduto, Guid idImagem);
        Task AtualizarOrdemImagensAsync(Guid idProduto, Dictionary<Guid, int> imagensOrdem);

        
        Task<string> SalvarArquivoAsync(IFormFile arquivo);
        Task<List<string>> SalvarMultiplosArquivosAsync(List<IFormFile> arquivos);
        Task ExcluirArquivoFisicoAsync(string caminhoArquivo);

       
        bool ValidarTipoArquivo(IFormFile arquivo);
        bool ValidarTamanhoArquivo(IFormFile arquivo);
        string GerarNomeUnicoArquivo(string nomeOriginal);
    }
}