function Filtrar(e, fn, jsDatatable) {
    const idElemento = `#tabela-partial`
    $(idElemento).html(divAlerts('load'));

    const jsonConfigPadraoDataTable = jsDatatable ?? configuracaoPadraoDatatables

    e.preventDefault();

    const { temParametros, formData } = obterParametrosURL();

    const formCurrent = temParametros ? formData : new FormData(e.currentTarget);

    if (temParametros) {
        for (const name in e.currentTarget) {
            const element = e.currentTarget[name];

            if (formData.get(element?.name)) {

                const result = preencherFormPorQuery(element.tagName);

                result(element, formData.get(element.name))
            }

        }
    }

    $.ajax(
        {
            type: "POST",
            data: formCurrent,
            enctype: 'multipart/form-data',
            cache: false,
            contentType: false,
            processData: false,
            url: $(e.currentTarget).attr("action"),
            success: function (response) {
                $('#tabela_wrapper').remove();

                $(idElemento).html(response);

                if (ehUmaFunction(fn))
                    fn();
                else if (!$.fn.DataTable.isDataTable('#tabela')) {

                    $('#tabela').DataTable(jsonConfigPadraoDataTable);
                }

                ObterTrDaTabela('tr')
            },
            error: function (data) {
                $(idElemento).html(divAlerts('erro'));
            }
        });
};