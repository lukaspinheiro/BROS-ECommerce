const camposTocados = new Set();
export let tentouEnviar = false;

export function marcarTentativaEnvio() {
    tentouEnviar = true;
    verificarEspacos();
}

export function verificarEspacos() {
    const form = document.getElementById("form-cadastrar-produto");
    if (!form) return;

    const campos = [
        { id: "TituloDescricao", erro: "titulo-error", max: 50 },
        { id: "Descricao", erro: "descricao-error", max: 300 },
        { id: "Nome", erro: "nome-error", max: 50 },
        { id: "Slug", erro: "slug-error", max: 30 },
        { id: "Preco", erro: "preco-error", tipo: "numero" }
    ];

    let todosValidos = true;

    campos.forEach(c => {
        const input = form.querySelector(`#${c.id}`);
        const erroSpan = document.getElementById(c.erro);
        let valido = true;

        if (!input || !erroSpan) return;

        const valor = input.value.trim();

        if (valor.length === 0) {
            valido = false;
        } else if (c.tipo === "numero") {
            valido = !isNaN(parseFloat(valor)) && parseFloat(valor) > 0;
        } else if (c.max && valor.length > c.max) {
            valido = false;
        }
        erroSpan.style.display = (camposTocados.has(c.id) || tentouEnviar) && !valido ? "block" : "none";

        if (!valido) todosValidos = false;
    });

    const indicePrincipal = document.getElementById("IndiceImagemPrincipal");
    const imagemPrincipalErro = document.getElementById("imagem-principal-error");
    const imagemPrincipalSelecionada = indicePrincipal && indicePrincipal.value !== "";

    if (imagemPrincipalErro) {
        const imagensPresentes = document.getElementById('preview-imagens')?.children.length > 0;
        imagemPrincipalErro.style.display = !imagemPrincipalSelecionada && imagensPresentes ? "block" : "none";
    }

    const botao = document.getElementById("btn-disable");
    if (botao) {
        botao.disabled = !(todosValidos && imagemPrincipalSelecionada);
    }
}

export function marcarCampoTocado(id) {
    camposTocados.add(id);
    verificarEspacos();
}

export function limparFormulario() {
    document.getElementById('form-cadastrar-produto').reset();
    document.getElementById('produto-id-edicao').value = '';
    document.querySelector('#modal-cadastrar-produto .modal-title').textContent = 'Cadastrar Produto';
    document.querySelector('#modal-cadastrar-produto .btn-success').textContent = 'ADICIONAR';
    document.getElementById('form-cadastrar-produto').action = '/Administrativo/Produto/CadastrarProduto';

    camposTocados.clear();
    tentouEnviar = false;
    verificarEspacos();
    window.produtoEditandoId = null;
}
