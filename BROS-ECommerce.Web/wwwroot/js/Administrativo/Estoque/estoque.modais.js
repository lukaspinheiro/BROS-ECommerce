
import { limparFormulario } from './estoque.formulario.js';
export function abrirModalCadastro() {
    limparFormulario();

    document.querySelector('#modal-cadastrar-produto-no-estoque .modal-title').textContent = 'Adicionar Produto Ao Estoque';
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

    const dataFormatada = formatarData(ultimaAtualizacao);
    document.getElementById('data-atualizacao-alterar').value = dataFormatada;

    $('#modal-alterar-quantidade-estoque').modal('show');
}


function formatarData(dataISO) {
    const data = new Date(dataISO);

    const dia = String(data.getDate()).padStart(2, '0');
    const mes = String(data.getMonth() + 1).padStart(2, '0');
    const ano = data.getFullYear();

    const hora = String(data.getHours()).padStart(2, '0');
    const minuto = String(data.getMinutes()).padStart(2, '0');

    return `${dia}/${mes}/${ano} ${hora}:${minuto}`;
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
