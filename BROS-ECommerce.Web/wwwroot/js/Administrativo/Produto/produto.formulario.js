export function verificarEspacos() {
    const form = document.getElementById("form-cadastrar-produto");
    const titulo = form.querySelector("#TituloDescricao");
    const descricao = form.querySelector("#Descricao");
    const nome = form.querySelector("#Nome");
    const slug = form.querySelector("#Slug");
    const preco = form.querySelector("#Preco");
    const camposObrigatorios = [titulo, descricao, nome, slug, preco];
    const todosValidos = camposObrigatorios.every(campo =>
        campo && campo.value.trim().length > 0
    );

    document.getElementById("btn-disable").disabled = !todosValidos;
}

export function limparFormulario() {
    document.getElementById('form-cadastrar-produto').reset();
    document.getElementById('produto-id-edicao').value = '';
    document.querySelector('#modal-cadastrar-produto .modal-title').textContent = 'Cadastrar Produto';
    document.querySelector('#modal-cadastrar-produto .btn-success').textContent = 'ADICIONAR';
    document.getElementById('form-cadastrar-produto').action = '/Administrativo/Produto/CadastrarProduto';

    verificarEspacos();
    window.produtoEditandoId = null;
}
