import { inicializarTabela } from './categoria.table.js';

document.addEventListener("DOMContentLoaded", function () {
    inicializarTabela();

    // Botão de abrir modal de cadastro (apenas limpa o form, o Bootstrap cuida de abrir)
    const btnAbrirModal = document.getElementById("btn-abrir-modal");
    if (btnAbrirModal) {
        btnAbrirModal.addEventListener("click", limparFormularioCadastrar);
    }

    // Modal de exclusão
    document.querySelectorAll('.btn-excluir').forEach(button => {
        button.addEventListener('click', function () {
            const id = this.dataset.idCategoria;
            const nome = this.dataset.nome;

            document.getElementById('categoria-excluir-nome').textContent = nome;
            document.getElementById('input-id-exclusao').value = id;
        });
    });

    const btnConfirmar = document.getElementById('btn-confirmar-exclusao');
    if (btnConfirmar) {
        btnConfirmar.addEventListener('click', () => {
            document.getElementById('form-excluir-categoria').submit();
        });
    }

    // Modal de edição
    document.querySelectorAll('.btn-editar').forEach(button => {
        button.addEventListener('click', function () {
            const id = this.dataset.idCategoria;
            const nome = this.dataset.nome;
            const descricao = this.dataset.descricao;
            const ativo = this.dataset.ativo === "True" || this.dataset.ativo === "true";

            document.getElementById('input-id-edicao').value = id;
            document.getElementById('NomeCategoriaEditar').value = nome;
            document.getElementById('DescCategoriaEditar').value = descricao;
            document.getElementById('StatusCategoriaEditar').value = ativo ? "true" : "false";
        });
    });

    // Validação campos cadastrar
    const nomeInput = document.getElementById('NomeCategoriaCadastrar');
    const descricaoInput = document.getElementById('DescCategoriaCadastrar');
    const btnSubmit = document.getElementById('btn-disable');

    if (nomeInput && descricaoInput && btnSubmit) {
        function validarCamposCadastrar() {
            const nomePreenchido = nomeInput.value.trim().length > 0;
            const descricaoPreenchida = descricaoInput.value.trim().length > 0;
            btnSubmit.disabled = !(nomePreenchido && descricaoPreenchida);
        }

        nomeInput.addEventListener('input', validarCamposCadastrar);
        descricaoInput.addEventListener('input', validarCamposCadastrar);
        validarCamposCadastrar();
    }

    // Validação campos editar
    const nomeEditarInput = document.getElementById('NomeCategoriaEditar');
    const descricaoEditarInput = document.getElementById('DescCategoriaEditar');
    const btnSubmitEditar = document.querySelector('#modal-editar-categoria button[type="submit"]');

    if (nomeEditarInput && descricaoEditarInput && btnSubmitEditar) {
        function validarCamposEditar() {
            const nomePreenchido = nomeEditarInput.value.trim().length > 0;
            const descricaoPreenchida = descricaoEditarInput.value.trim().length > 0;
            btnSubmitEditar.disabled = !(nomePreenchido && descricaoPreenchida);
        }

        nomeEditarInput.addEventListener('input', validarCamposEditar);
        descricaoEditarInput.addEventListener('input', validarCamposEditar);
        validarCamposEditar();
    }
});

function limparFormularioCadastrar() {
    const form = document.getElementById("form-cadastrar-categoria");
    if (form) {
        form.reset();
        document.getElementById("NomeCategoriaCadastrar").value = "";
        document.getElementById("DescCategoriaCadastrar").value = "";
        form.querySelectorAll(".text-danger").forEach(span => span.textContent = "");
    }
}
