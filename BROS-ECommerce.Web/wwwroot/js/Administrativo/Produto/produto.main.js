import { inicializarTabela } from './produto.table.js';
import {
    abrirModalCadastro,
    verDetalhes,
    editarProduto,
    confirmarExclusao,
    fecharModalCadastrar,
    fecharModalDetalhes,
    fecharModalExclusao
} from './produto.modais.js';
import { verificarEspacos, limparFormulario } from './produto.formulario.js';


document.addEventListener("DOMContentLoaded", function () {
    inicializarTabela();

    document.getElementById("btn-abrir-modal").addEventListener("click", function (e) {
        e.preventDefault();
        abrirModalCadastro();
    });

    document.getElementById("btn-confirmar-exclusao").addEventListener("click", function () {
        const id = this.dataset.id;

        $.ajax({
            url: '/Administrativo/Produto/Excluir/' + id,
            type: 'POST',
            headers: {
                'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
            },
            success: function (response) {
                $('#modal-confirmar-exclusao').modal('hide');
                if (response.success) {
                    location.reload();
                } else {
                    alert('Erro ao excluir produto: ' + response.message);
                }
            },
            error: function () {
                alert('Erro ao excluir produto');
            }
        });
    });

    document.querySelectorAll('.btn-ver-detalhes').forEach(btn => {
        btn.addEventListener("click", () => verDetalhes(btn.dataset));
    });

    document.querySelectorAll('.btn-editar').forEach(btn => {
        btn.addEventListener("click", () => editarProduto(
            btn.dataset.id,
            btn.dataset.nome,
            btn.dataset.slug,
            btn.dataset.titulo,
            btn.dataset.descricao,
            btn.dataset.preco
        ));
    });

    document.querySelectorAll('.btn-excluir').forEach(btn => {
        btn.addEventListener("click", () => confirmarExclusao(btn.dataset.id, btn.dataset.nome));
    });

    document.getElementById("modal-cadastrar-produto").addEventListener("hidden.bs.modal", limparFormulario);

    const camposParaVerificar = document.querySelectorAll('#form-cadastrar-produto input');

    camposParaVerificar.forEach(campo => {
        campo.addEventListener("keyup", verificarEspacos);
    });

    window.fecharModalCadastrar = fecharModalCadastrar;
    window.fecharModalDetalhes = fecharModalDetalhes;
    window.fecharModalExclusao = fecharModalExclusao;
});
