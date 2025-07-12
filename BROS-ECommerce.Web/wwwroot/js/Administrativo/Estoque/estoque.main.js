import { inicializarTabela } from './estoque.table.js';
import {
    abrirModalCadastro,
    fecharModalCadastrar,
    abrirModalAlterarQuantidade,
    fecharModalAlterarQuantidade
} from './estoque.modais.js';

import { verificarEspacos, limparFormulario, verificarEspacosAlterar} from './estoque.formulario.js';

document.addEventListener("DOMContentLoaded", function () {
    inicializarTabela();


    document.getElementById("btn-abrir-modal").addEventListener("click", function (e) {
        e.preventDefault();
        abrirModalCadastro();
    });

    document.getElementById("modal-cadastrar-produto-no-estoque").addEventListener("hidden.bs.modal", limparFormulario);

    const camposParaVerificar = document.querySelectorAll('#form-cadastrar-produto-no-estoque input');
    camposParaVerificar.forEach(campo => {
        campo.addEventListener("keyup", verificarEspacos);
    });

    const camposAlterar = document.querySelectorAll('#form-alterar-quantidade-estoque input');
    camposAlterar.forEach(campo => {
        campo.addEventListener("keyup", verificarEspacosAlterar);
    });


    document.querySelectorAll('.btn-alterar').forEach(btn => {
        btn.addEventListener('click', function () {
            abrirModalAlterarQuantidade({
                idProduto: this.dataset.idProduto,
                nome: this.dataset.nome,
                quantidade: this.dataset.quantidade,
                ultimaAtualizacao: this.dataset.atualizacao
            });
        });
    });

    document.getElementById('produtoSelect').addEventListener('change', function () {
        var select = this;
        var optionSelecionada = select.options[select.selectedIndex];
        var imagemUrl = optionSelecionada.getAttribute('data-image-url') || '';

        var imgPreview = document.getElementById('previewImagemProduto');
        imgPreview.src = imagemUrl;
    });

    document.getElementById('produtoSelect').dispatchEvent(new Event('change'));

    window.fecharModalAlterarQuantidade = fecharModalAlterarQuantidade;
    window.fecharModalCadastrar = fecharModalCadastrar;
});
$(document).ready(function () {
    setTimeout(() => {
        $('#alert-sucesso').alert('close');
        $('#alert-erro').alert('close');
    }, 2500);
});
