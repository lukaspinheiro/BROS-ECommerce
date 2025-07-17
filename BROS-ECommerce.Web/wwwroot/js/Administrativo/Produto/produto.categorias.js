let produtoAtualId = null;

function abrirModalCategorias(idProduto) {
    produtoAtualId = idProduto;
    $.get('/Administrativo/Produto/ModalCategorias', { idProduto: idProduto }, function (html) {
        $('#div-modal-categorias').html(html);
        $('#modal-lista-categorias').modal('show');

        $('#tabela-categorias-produto').DataTable({
            paging: true,
            pageLength: 5,
            searching: false,
            info: false,
            ordering: false,
            fixedHeader: true,
            language: {
                emptyTable: "Nenhuma categoria adicionada",
                paginate: {
                    previous: "Anterior",
                    next: "Próxima"
                }
            }
        });
    });
}

function fecharModalCategorias() {
    $('#modal-lista-categorias').modal('hide');
    produtoAtualId = null;
}

function adicionarCategoriaAoProduto() {
    const idCategoria = $('#select-categoria-disponivel').val();
    const nomeCategoria = $('#select-categoria-disponivel option:selected').text();

    if (!idCategoria) {
        alert("Selecione uma categoria.");
        return;
    }

    if (!produtoAtualId || produtoAtualId === '00000000-0000-0000-0000-000000000000') {
        alert("Produto inválido.");
        return;
    }

    $.ajax({
        url: '/Administrativo/Produto/Associar',
        method: 'POST',
        data: {
            idProduto: produtoAtualId,
            idCategoria: idCategoria
        },
        success: function (response) {
            const novaLinha = `
                                <tr data-id="${response.idCategoriaProduto}">
                                    <td>${nomeCategoria}</td>
                                    <td>${response.descricaoCategoria ?? ''}</td>
                                    <td>
                                        <button type="button" class="btn btn-danger btn-sm" onclick="removerCategoria('${response.idCategoriaProduto}')">Remover</button>
                                    </td>
                                </tr>`;
            $('#tabela-categorias-produto tbody').append(novaLinha);
        },
        error: function () {
            alert("Erro ao associar categoria.");
        }
    });
}

function removerCategoria(idCategoriaProduto) {
    if (!confirm("Deseja realmente remover essa categoria?")) return;

    $.ajax({
        url: '/Administrativo/Produto/Remover',
        method: 'POST',
        data: { idCategoriaProduto: idCategoriaProduto },
        success: function () {
            $(`tr[data-id="${idCategoriaProduto}"]`).remove();
        },
        error: function () {
            alert("Erro ao remover categoria.");
        }
    });
}