
document.addEventListener("DOMContentLoaded", function () {
    const btnAbrirModal = document.getElementById("btn-abrir-modal");
    const modalCadastrar = new bootstrap.Modal(document.getElementById("modal-cadastrar-categoria"));

    if (btnAbrirModal) {
        btnAbrirModal.addEventListener("click", function () {
            limparFormularioCadastrar();
            modalCadastrar.show();
        });
    }
});

function limparFormularioCadastrar() {
    const form = document.getElementById("form-cadastrar-categoria");
    if (!form) return;

    form.reset();

    document.getElementById("NomeCategoria").value = "";
    document.getElementById("Descricao").value = "";
    document.getElementById("Ativo").checked = true;

    const spansErro = form.querySelectorAll(".text-danger");
    spansErro.forEach(span => span.textContent = "");
}
