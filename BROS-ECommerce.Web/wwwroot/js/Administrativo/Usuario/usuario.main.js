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

    const btnCadastrar = document.getElementById("btnCadastrarUsuario");
    if (btnCadastrar) {
        btnCadastrar.addEventListener("click", function (e) {
            e.preventDefault();
            abrirModalCadastro();
        });
    }

    const btnConfirmarExclusao = document.getElementById("btn-confirmar-exclusao");
    if (btnConfirmarExclusao) {
        btnConfirmarExclusao.addEventListener("click", function () {
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
    }

    // Delegação para cliques em editar e excluir
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

    const modalCadastrar = document.getElementById("modal-cadastrar-usuario");
    if (modalCadastrar) {
        modalCadastrar.addEventListener("hidden.bs.modal", limparFormulario);
    }

    const camposParaVerificar = document.querySelectorAll('#form-cadastrar-usuario input, #form-cadastrar-usuario select');
    camposParaVerificar.forEach(campo => {
        campo.addEventListener("keyup", verificarEspacos);
        campo.addEventListener("change", verificarEspacos);
    });

    window.fecharModalCadastrar = fecharModalCadastrar;
    window.fecharModalExclusao = fecharModalExclusao;
});
