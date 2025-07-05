export function verificarEspacos() {
    const form = document.getElementById("form-cadastrar-produto-no-estoque");
    const quantidade = form.querySelector("#Quantidade");
    const camposObrigatorios = [quantidade];
    const todosValidos = camposObrigatorios.every(campo =>
        campo && campo.value.trim().length > 0
    );

    document.getElementById("btn-disable").disabled = !todosValidos;
}
export function verificarEspacosAlterar() {
    const form = document.getElementById("form-alterar-quantidade-estoque");
    if (!form) return;

    const quantidade = form.querySelector('input[name="cadastrarEstoqueViewModel.Quantidade"]');
    const botao = form.querySelector('.btn-disable');

    if (!quantidade || !botao) return;

    const valido = quantidade.value.trim().length > 0;
    botao.disabled = !valido;
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


