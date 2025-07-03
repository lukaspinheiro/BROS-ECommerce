import {
    abrirModalCadastro,
    fecharModalCadastrar
} from './estoque.modais.js';
import { verificarEspacos, limparFormulario } from './estoque.formulario.js';

document.addEventListener("DOMContentLoaded", function () {
    //inicializarTabela();

    document.getElementById("btn-abrir-modal").addEventListener("click", function (e) {
        e.preventDefault();
        abrirModalCadastro();
    });

    document.getElementById("modal-cadastrar-produto-no-estoque").addEventListener("hidden.bs.modal", limparFormulario);

    const camposParaVerificar = document.querySelectorAll('#form-cadastrar-produto-no-estoque input');

    camposParaVerificar.forEach(campo => {
        campo.addEventListener("keyup", verificarEspacos);
    });

    window.fecharModalCadastrar = fecharModalCadastrar;
});