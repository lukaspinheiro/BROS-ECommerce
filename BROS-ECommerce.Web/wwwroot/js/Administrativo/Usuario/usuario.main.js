import { inicializarTabela } from './usuario.table.js';
import {
    abrirModalCadastro,
    editarUsuario,
    confirmarExclusao,
    fecharModalCadastrar,
    fecharModalExclusao
} from './usuario.modais.js';
import { verificarEspacos, limparFormulario } from './usuario.formulario.js';

document.addEventListener("DOMContentLoaded", function () {
    inicializarTabela();

    document.getElementById("btnCadastrarUsuario").addEventListener("click", function (e) {
        e.preventDefault();
        abrirModalCadastro();
    });

    document.getElementById("btn-confirmar-exclusao").addEventListener("click", function () {
        const id = this.dataset.id;
        $.ajax({
            url: '/Administrativo/Usuario/Excluir/' + id,
            type: 'POST',
            headers: {
                'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
            },
            success: function (response) {
                $('#modal-confirmar-exclusao').modal('hide');
                if (response.success) {
                    location.reload();
                } else {
                    alert('Erro ao excluir usuário: ' + response.message);
                }
            },
            error: function () {
                alert('Erro ao excluir usuário');
            }
        });
    });

    document.addEventListener("click", function (e) {
        const btnEditar = e.target.closest('.btn-editar-usuario');
        if (btnEditar) {
            editarUsuario(
                btnEditar.dataset.id,
                btnEditar.dataset.nome,
                btnEditar.dataset.email,
                btnEditar.dataset.cpf,
                btnEditar.dataset.genero,
                btnEditar.dataset.nascimento,
                btnEditar.dataset.ativo
            );
        }

        const btnExcluir = e.target.closest('.btn-excluir-usuario');
        if (btnExcluir) {
            confirmarExclusao(btnExcluir.dataset.id, btnExcluir.dataset.nome);
        }
    });

    document.querySelectorAll('.btn-excluir-usuario').forEach(btn => {
        btn.addEventListener("click", () => confirmarExclusao(btn.dataset.id, btn.dataset.nome));
    });

    document.getElementById("modal-cadastrar-usuario").addEventListener("hidden.bs.modal", limparFormulario);

    const camposParaVerificar = document.querySelectorAll('#form-cadastrar-usuario input, #form-cadastrar-usuario select');
    camposParaVerificar.forEach(campo => {
        campo.addEventListener("keyup", verificarEspacos);
        campo.addEventListener("change", verificarEspacos);
    });

    window.fecharModalCadastrar = fecharModalCadastrar;
    window.fecharModalExclusao = fecharModalExclusao;
});