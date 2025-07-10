using BROS_ECommerce.Domain.Entities;
using BROS_ECommerce.Domain.Interfaces.Repository;
using BROS_ECommerce.Services.Interface.Services;
using BROS_ECommerce.Services.ViewModel.Imagem;
using Microsoft.AspNetCore.Hosting; 
using Microsoft.AspNetCore.Http;

namespace BROS_ECommerce.Services.Services
{
    public class ServiceImagem : IServiceImagem
    {
        private readonly IRepositoryImagem _repositoryImagem;
        private readonly IRepositoryProdutoImagem _repositoryProdutoImagem;
        private readonly IHostingEnvironment _environment; 
        private const string PASTA_UPLOAD = "uploads/imagens";
        private static readonly string[] TiposPermitidos = { "image/jpeg", "image/jpg", "image/png", "image/gif", "image/webp" };
        private const long TamanhoMaximo = 5 * 1024 * 1024; 

        public ServiceImagem(IRepositoryImagem repositoryImagem, IRepositoryProdutoImagem repositoryProdutoImagem, IHostingEnvironment environment) // ← MUDAR AQUI TAMBÉM
        {
            _repositoryImagem = repositoryImagem;
            _repositoryProdutoImagem = repositoryProdutoImagem;
            _environment = environment;
        }

        public async Task<List<ImagemViewModel>> ObterTodosAsync()
        {
            var imagens = await _repositoryImagem.ObterTodosAsync();
            return await MapearParaViewModelAsync(imagens);
        }

        public async Task<List<ImagemViewModel>> ObterAtivosAsync()
        {
            var imagens = await _repositoryImagem.ObterAtivosAsync();
            return await MapearParaViewModelAsync(imagens);
        }

        public async Task<ImagemViewModel?> ObterPorIdAsync(Guid id)
        {
            var imagem = await _repositoryImagem.ObterPorIdAsync(id);
            if (imagem == null) return null;

            var quantidadeProdutos = await _repositoryProdutoImagem.ContarProdutosPorImagemAsync(id);

            return new ImagemViewModel
            {
                IdImagem = imagem.IdImagem,
                NomeArquivo = imagem.NomeArquivo,
                CaminhoArquivo = imagem.CaminhoArquivo,
                TamanhoArquivo = imagem.TamanhoArquivo,
                TipoMime = imagem.TipoMime,
                AltText = imagem.AltText,
                DataCriacao = imagem.DataCriacao,
                DataAtualizacao = imagem.DataAtualizacao,
                Ativo = imagem.Ativo,
                QuantidadeProdutos = quantidadeProdutos
            };
        }

        public async Task<List<ImagemViewModel>> ObterPaginadoAsync(int pagina, int itensPorPagina, FiltroImagemViewModel? filtro = null)
        {
            var imagens = await _repositoryImagem.ObterPaginadoAsync(pagina, itensPorPagina);
            return await MapearParaViewModelAsync(imagens);
        }

        public async Task<List<ImagemViewModel>> ObterImagensNaoAssociadasAsync()
        {
            var imagens = await _repositoryImagem.ObterImagensNaoAssociadasAsync();
            return await MapearParaViewModelAsync(imagens);
        }

        public async Task<List<ImagemViewModel>> ObterImagensPorProdutoAsync(Guid idProduto)
        {
            var imagens = await _repositoryImagem.ObterImagensAssociadasAoProdutoAsync(idProduto);
            return await MapearParaViewModelAsync(imagens);
        }

        public async Task<Guid> AdicionarAsync(CadastrarImagemViewModel imagemVM)
        {
            if (!imagemVM.ValidarArquivos(out var erros))
            {
                throw new ArgumentException($"Arquivos inválidos: {string.Join(", ", erros)}");
            }

            var arquivo = imagemVM.Arquivos.First();
            var caminhoArquivo = await SalvarArquivoAsync(arquivo);

            var imagem = new Imagem
            {
                IdImagem = Guid.NewGuid(),
                NomeArquivo = arquivo.FileName ?? "arquivo_sem_nome",
                CaminhoArquivo = caminhoArquivo,
                TamanhoArquivo = arquivo.Length,
                TipoMime = arquivo.ContentType ?? "application/octet-stream",
                AltText = imagemVM.AltText,
                DataCriacao = DateTime.UtcNow,
                Ativo = true
            };

            await _repositoryImagem.AdicionarAsync(imagem);
            return imagem.IdImagem;
        }

        public async Task<List<Guid>> AdicionarMultiplasAsync(CadastrarImagemViewModel imagemVM)
        {
            if (!imagemVM.ValidarArquivos(out var erros))
            {
                throw new ArgumentException($"Arquivos inválidos: {string.Join(", ", erros)}");
            }

            var idsImagens = new List<Guid>();

            foreach (var arquivo in imagemVM.Arquivos)
            {
                var caminhoArquivo = await SalvarArquivoAsync(arquivo);

                var imagem = new Imagem
                {
                    IdImagem = Guid.NewGuid(),
                    NomeArquivo = arquivo.FileName ?? "arquivo_sem_nome",
                    CaminhoArquivo = caminhoArquivo,
                    TamanhoArquivo = arquivo.Length,
                    TipoMime = arquivo.ContentType ?? "application/octet-stream",
                    AltText = imagemVM.AltText,
                    DataCriacao = DateTime.UtcNow,
                    Ativo = true
                };

                await _repositoryImagem.AdicionarAsync(imagem);
                idsImagens.Add(imagem.IdImagem);
            }

            return idsImagens;
        }

        public async Task AtualizarAsync(ImagemViewModel imagemVM)
        {
            var imagem = await _repositoryImagem.ObterPorIdAsync(imagemVM.IdImagem);
            if (imagem == null) throw new ArgumentException("Imagem não encontrada");

            imagem.AltText = imagemVM.AltText;
            imagem.Ativo = imagemVM.Ativo;
            imagem.DataAtualizacao = DateTime.UtcNow;

            await _repositoryImagem.AtualizarAsync(imagem);
        }

        public async Task ExcluirAsync(Guid id)
        {
            var imagem = await _repositoryImagem.ObterPorIdAsync(id);
            if (imagem == null) return;

            
            await _repositoryProdutoImagem.RemoverTodasAssociacoesImagemAsync(id);

            
            await ExcluirArquivoFisicoAsync(imagem.CaminhoArquivo);

            
            await _repositoryImagem.ExcluirAsync(id);
        }

        public async Task ExcluirLogicamenteAsync(Guid id)
        {
            await _repositoryImagem.ExcluirLogicamenteAsync(id);
        }

        public async Task<List<ImagemViewModel>> BuscarPorNomeAsync(string nome)
        {
            var imagens = await _repositoryImagem.EncontrarAsync(i => i.NomeArquivo.Contains(nome) && i.Ativo);
            return await MapearParaViewModelAsync(imagens);
        }

        public async Task<List<ImagemViewModel>> BuscarPorTipoMimeAsync(string tipoMime)
        {
            var imagens = await _repositoryImagem.BuscarPorTipoMimeAsync(tipoMime);
            return await MapearParaViewModelAsync(imagens);
        }

        public async Task<bool> NomeArquivoExisteAsync(string nomeArquivo)
        {
            return await _repositoryImagem.NomeArquivoExisteAsync(nomeArquivo);
        }

        public async Task<int> ContarTotalAsync()
        {
            return await _repositoryImagem.ContarTotalAsync();
        }

        public async Task<int> ContarAtivosAsync()
        {
            return await _repositoryImagem.ContarAtivosAsync();
        }

        public async Task<long> ObterTamanhoTotalAsync()
        {
            return await _repositoryImagem.ObterTamanhoTotalImagensAsync();
        }

        
        public async Task AssociarImagemAoProdutoAsync(Guid idProduto, Guid idImagem, bool principal = false, int ordem = 0)
        {
            if (await _repositoryProdutoImagem.ExisteAssociacaoAsync(idProduto, idImagem))
                return;

            var produtoImagem = new ProdutoImagem
            {
                IdProdutoImagem = Guid.NewGuid(),
                IdProduto = idProduto,
                IdImagem = idImagem,
                Principal = principal,
                Ordem = ordem,
                DataAssociacao = DateTime.UtcNow
            };

            await _repositoryProdutoImagem.AdicionarAsync(produtoImagem);
        }

        public async Task AssociarImagensAoProdutoAsync(Guid idProduto, List<Guid> idsImagens)
        {
            for (int i = 0; i < idsImagens.Count; i++)
            {
                await AssociarImagemAoProdutoAsync(idProduto, idsImagens[i], i == 0, i);
            }
        }

        public async Task RemoverAssociacaoProdutoAsync(Guid idProduto, Guid idImagem)
        {
            await _repositoryProdutoImagem.RemoverAssociacaoAsync(idProduto, idImagem);
        }

        public async Task DefinirImagemPrincipalAsync(Guid idProduto, Guid idImagem)
        {
            await _repositoryProdutoImagem.DefinirImagemPrincipalAsync(idProduto, idImagem);
        }

        public async Task AtualizarOrdemImagensAsync(Guid idProduto, Dictionary<Guid, int> imagensOrdem)
        {
            await _repositoryProdutoImagem.AtualizarOrdemImagensAsync(idProduto, imagensOrdem);
        }

        
        public async Task<string> SalvarArquivoAsync(IFormFile arquivo)
        {
            if (!ValidarTipoArquivo(arquivo) || !ValidarTamanhoArquivo(arquivo))
                throw new ArgumentException("Arquivo inválido");

            var nomeUnico = GerarNomeUnicoArquivo(arquivo.FileName ?? "arquivo");
            var anoMes = DateTime.Now.ToString("yyyy/MM");
            var pastaDestino = Path.Combine(_environment.WebRootPath, PASTA_UPLOAD, anoMes);

            Directory.CreateDirectory(pastaDestino);

            var caminhoCompleto = Path.Combine(pastaDestino, nomeUnico);
            var caminhoRelativo = $"/{PASTA_UPLOAD}/{anoMes}/{nomeUnico}".Replace("\\", "/");

            using (var stream = new FileStream(caminhoCompleto, FileMode.Create))
            {
                await arquivo.CopyToAsync(stream);
            }

            return caminhoRelativo;
        }

        public async Task<List<string>> SalvarMultiplosArquivosAsync(List<IFormFile> arquivos)
        {
            var caminhos = new List<string>();
            foreach (var arquivo in arquivos)
            {
                var caminho = await SalvarArquivoAsync(arquivo);
                caminhos.Add(caminho);
            }
            return caminhos;
        }

        public async Task ExcluirArquivoFisicoAsync(string caminhoArquivo)
        {
            try
            {
                var caminhoCompleto = Path.Combine(_environment.WebRootPath, caminhoArquivo.TrimStart('/'));
                if (File.Exists(caminhoCompleto))
                {
                    File.Delete(caminhoCompleto);
                }
                await Task.CompletedTask;
            }
            catch
            {
                
            }
        }

       
        public bool ValidarTipoArquivo(IFormFile arquivo)
        {
            return TiposPermitidos.Contains(arquivo.ContentType?.ToLower() ?? "");
        }

        public bool ValidarTamanhoArquivo(IFormFile arquivo)
        {
            return arquivo.Length <= TamanhoMaximo;
        }

        public string GerarNomeUnicoArquivo(string nomeOriginal)
        {
            var extensao = Path.GetExtension(nomeOriginal);
            var nomeBase = Path.GetFileNameWithoutExtension(nomeOriginal);
            var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            var guid = Guid.NewGuid().ToString("N")[..8];

            return $"{nomeBase}_{timestamp}_{guid}{extensao}";
        }

        
        private async Task<List<ImagemViewModel>> MapearParaViewModelAsync(List<Imagem> imagens)
        {
            var viewModels = new List<ImagemViewModel>();

            foreach (var imagem in imagens)
            {
                var quantidadeProdutos = await _repositoryProdutoImagem.ContarProdutosPorImagemAsync(imagem.IdImagem);

                viewModels.Add(new ImagemViewModel
                {
                    IdImagem = imagem.IdImagem,
                    NomeArquivo = imagem.NomeArquivo,
                    CaminhoArquivo = imagem.CaminhoArquivo,
                    TamanhoArquivo = imagem.TamanhoArquivo,
                    TipoMime = imagem.TipoMime,
                    AltText = imagem.AltText,
                    DataCriacao = imagem.DataCriacao,
                    DataAtualizacao = imagem.DataAtualizacao,
                    Ativo = imagem.Ativo,
                    QuantidadeProdutos = quantidadeProdutos
                });
            }

            return viewModels;
        }
    }
}