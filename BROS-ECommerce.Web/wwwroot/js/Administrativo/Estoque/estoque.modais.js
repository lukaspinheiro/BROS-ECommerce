
import { limparFormulario } from './estoque.formulario.js';
export function abrirModalCadastro() {
    limparFormulario();

    document.querySelector('#modal-cadastrar-produto-no-estoque .modal-title').textContent = 'Cadastrar Produto';
    document.querySelector('#modal-cadastrar-produto-no-estoque .btn-success').textContent = 'ADICIONAR';
    document.getElementById('form-cadastrar-produto-no-estoque').action = '/Administrativo/Estoque/CadastrarProdutoNoEstoque';
    document.getElementById('produto-estoque-id-edicao').value = '';

    $('#modal-cadastrar-produto-no-estoque').modal('show');
}
export function abrirModalAlterarQuantidade({ idProduto, nome, quantidade, ultimaAtualizacao }) {
    const modal = document.getElementById('modal-alterar-quantidade-estoque');
    if (!modal) return;

    document.getElementById('id-produto-alterar').value = idProduto;
    document.getElementById('quantidade-alterar').value = quantidade;
    document.getElementById('nome-produto-alterar').value = nome;
    document.getElementById('data-atualizacao-alterar').value = ultimaAtualizacao;

    $('#modal-alterar-quantidade-estoque').modal('show');
}


export function fecharModalCadastrar() {
    $('#modal-cadastrar-produto-no-estoque').modal('hide');
    limparFormulario();
}
export function fecharModalAlterarQuantidade() {
    $('#modal-alterar-quantidade-estoque').modal('hide');

    const form = document.getElementById('form-alterar-quantidade-estoque');
    form.reset();
}
