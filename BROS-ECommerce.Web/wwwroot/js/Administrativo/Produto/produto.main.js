import { inicializarTabelaProduto } from './produto.table.js';
import {
    abrirModalCadastro,
    verDetalhes,
    editarProduto,
    confirmarExclusao,
    fecharModalCadastrar,
    fecharModalDetalhes,
    fecharModalExclusao,
    inicializarEventosDetalhes
} from './produto.modais.js';
import { verificarEspacos, limparFormulario, marcarCampoTocado } from './produto.formulario.js';
import { inicializarImagemProduto } from './produto.imagem.js';


document.addEventListener("DOMContentLoaded", function () {
    inicializarEventosDetalhes();
    inicializarImagemProduto();
    inicializarTabelaProduto();

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
        btn.addEventListener("click", async () => {
            const id = btn.dataset.id;

            try {
                const response = await fetch(`/Administrativo/Produto/ObterDetalhes/${id}`);
                if (!response.ok) throw new Error("Erro ao buscar detalhes");
                const data = await response.json();

                editarProduto(
                    id,
                    data.nome,
                    data.slug,
                    data.tituloDescricao,
                    data.descricao,
                    data.preco,
                    data.imagens,
                    data.idImagemPrincipal
                );
            } catch (error) {
                console.error("Erro ao buscar detalhes do produto:", error);
                alert("Não foi possível carregar os detalhes do produto.");
            }
        });
    });




    document.querySelectorAll('.btn-excluir').forEach(btn => {
        btn.addEventListener("click", () => confirmarExclusao(btn.dataset.id, btn.dataset.nome));
    });

    document.getElementById("modal-cadastrar-produto").addEventListener("hidden.bs.modal", limparFormulario);

    const camposParaVerificar = document.querySelectorAll('#form-cadastrar-produto input');

    camposParaVerificar.forEach(campo => {
        campo.addEventListener("input", () => marcarCampoTocado(campo.id));
        campo.addEventListener("focus", () => marcarCampoTocado(campo.id)); // NOVO: ao focar também
    });

    window.fecharModalCadastrar = fecharModalCadastrar;
    window.fecharModalDetalhes = fecharModalDetalhes;
    window.fecharModalExclusao = fecharModalExclusao;
});
