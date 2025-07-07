export function verificarEspacos() {
    const form = document.getElementById("form-cadastrar-produto-no-estoque");
    const quantidade = form.querySelector("#Quantidade");
    const botao = document.getElementById("btn-disable");

    if (!quantidade || !botao) return;

    const valor = quantidade.value.trim();
    const ehInteiroValido = /^\d+$/.test(valor);
    botao.disabled = !ehInteiroValido;

    const erro = document.getElementById("nome-error");
    if (erro) {
        erro.style.display = ehInteiroValido ? "none" : "block";
    }
}

export function verificarEspacosAlterar() {
    const form = document.getElementById("form-alterar-quantidade-estoque");
    if (!form) return;

    const quantidade = form.querySelector('input[name="cadastrarEstoqueViewModel.Quantidade"]');
    const botao = form.querySelector('.btn-disable');
    if (!quantidade || !botao) return;

    const valor = quantidade.value.trim();

    const ehInteiroValido = /^\d+$/.test(valor);

    botao.disabled = !ehInteiroValido;
}



export function limparFormulario() {
    document.getElementById('form-cadastrar-produto-no-estoque').reset();
    document.getElementById('produto-estoque-id-edicao').value = '';
    document.querySelector('#modal-cadastrar-produto-no-estoque .modal-title').textContent = 'Adicionar Produto No Estoque';
    document.querySelector('#modal-cadastrar-produto-no-estoque .btn-success').textContent = 'ADICIONAR';
    document.getElementById('form-cadastrar-produto-no-estoque').action = '/Administrativo/Estoque/CadastrarProdutoNoEstoque';

    verificarEspacos();
    window.produtoEditandoId = null;
}


