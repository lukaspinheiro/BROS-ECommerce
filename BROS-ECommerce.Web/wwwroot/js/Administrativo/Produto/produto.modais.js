import { limparFormulario, verificarEspacos } from './produto.formulario.js';

export function abrirModalCadastro() {
    limparFormulario();

    document.querySelector('#modal-cadastrar-produto .modal-title').textContent = 'Cadastrar Produto';
    document.querySelector('#modal-cadastrar-produto .btn-success').textContent = 'ADICIONAR';
    document.getElementById('form-cadastrar-produto').action = '/Administrativo/Produto/CadastrarProduto';
    document.getElementById('produto-id-edicao').value = '';

    $('#modal-cadastrar-produto').modal('show');
}

export function verDetalhes({ id, nome, slug, titulo, descricao, preco, imagens }) {
    document.getElementById('detalhe-nome').textContent = nome;
    document.getElementById('detalhe-slug').textContent = slug;
    document.getElementById('detalhe-titulo').textContent = titulo;
    document.getElementById('detalhe-descricao').textContent = descricao;
    document.getElementById('detalhe-preco').textContent = 'R$ ' + parseFloat(preco).toFixed(2);

    const divImagens = document.getElementById('detalhe-imagens');
    divImagens.innerHTML = '';

    if (Array.isArray(imagens) && imagens.length > 0) {
        imagens.forEach(url => {
            const img = document.createElement('img');
            img.src = url;
            img.alt = 'Imagem do produto';
            img.className = 'img-thumbnail mr-2';
            img.style.maxHeight = '100px';
            divImagens.appendChild(img);
        });
    } else {
        divImagens.innerHTML = '<p class="text-muted">Nenhuma imagem disponível</p>';
    }

    $('#modal-detalhes-produto').modal('show');
}
export function inicializarEventosDetalhes() {
    document.querySelectorAll('.btn-ver-detalhes').forEach(btn => {
        btn.addEventListener('click', () => {
            const id = btn.getAttribute('data-id');

            fetch(`/Administrativo/Produto/ObterDetalhes/${id}`)
                .then(res => {
                    if (!res.ok) throw new Error("Erro ao buscar detalhes");
                    return res.json();
                })
                .then(data => {
                    verDetalhes({
                        id: id,
                        nome: data.nome,
                        slug: data.slug,
                        titulo: data.tituloDescricao,
                        descricao: data.descricao,
                        preco: data.preco,
                        imagens: data.imagens
                    });
                })
                .catch(() => alert('Erro ao carregar os detalhes do produto'));
        });
    });
}


export function editarProduto(id, nome, slug, titulo, descricao, preco) {
    window.produtoEditandoId = id;

    document.getElementById('Nome').value = nome;
    document.getElementById('Slug').value = slug;
    document.getElementById('TituloDescricao').value = titulo;
    document.getElementById('Descricao').value = descricao;
    document.getElementById('Preco').value = preco;
    document.getElementById('produto-id-edicao').value = id;

    document.querySelector('#modal-cadastrar-produto .modal-title').textContent = 'Editar Produto';
    document.querySelector('#modal-cadastrar-produto .btn-success').textContent = 'ATUALIZAR';
    
    document.getElementById('form-cadastrar-produto').action = '/Administrativo/Produto/AtualizarProduto';
    document.getElementById('btn-disable').disabled = false;

    $('#modal-cadastrar-produto').modal('show');
}

export function confirmarExclusao(id, nome) {
    document.getElementById('produto-excluir-nome').textContent = nome;
    document.getElementById('btn-confirmar-exclusao').setAttribute('data-id', id);

    $('#modal-confirmar-exclusao').modal('show');
}

export function fecharModalCadastrar() {
    $('#modal-cadastrar-produto').modal('hide');
    limparFormulario();
}

export function fecharModalDetalhes() {
    $('#modal-detalhes-produto').modal('hide');
}

export function fecharModalExclusao() {
    $('#modal-confirmar-exclusao').modal('hide');
}