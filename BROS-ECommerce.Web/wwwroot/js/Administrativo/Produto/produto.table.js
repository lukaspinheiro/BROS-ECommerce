export function inicializarTabela() {
    const tabela = $('#tabela');
    if (tabela.length > 0 && tabela.find("tbody tr").length > 0 && !tabela.find("tbody tr td[colspan]").length) {
        tabela.DataTable({
            language: {
                url: "//cdn.datatables.net/plug-ins/1.10.24/i18n/Portuguese-Brasil.json"
            },
            pageLength: 10,
            responsive: true,
            columnDefs: [
                { className: 'text-start', targets: '_all' },
                { type: 'string', targets: '_all' },
                { searchable: false, targets: [0, 5] }
            ]
        });
    }
}
