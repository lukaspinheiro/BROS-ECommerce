
import { limparFormulario } from './estoque.formulario.js';
export function abrirModalCadastro() {
    limparFormulario();

    document.querySelector('#modal-cadastrar-produto-no-estoque .modal-title').textContent = 'Cadastrar Produto';
    document.querySelector('#modal-cadastrar-produto-no-estoque .btn-success').textContent = 'ADICIONAR';
    document.getElementById('form-cadastrar-produto-no-estoque').action = '/Administrativo/Estoque/CadastrarProdutoNoEstoque';
    document.getElementById('produto-estoque-id-edicao').value = '';

    $('#modal-cadastrar-produto-no-estoque').modal('show');
}

export function fecharModalCadastrar() {
    $('#modal-cadastrar-produto-no-estoque').modal('hide');
    limparFormulario();
}
