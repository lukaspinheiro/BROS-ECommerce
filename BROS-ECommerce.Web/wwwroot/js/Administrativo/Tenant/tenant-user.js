import { criarConfiguracaoTabela } from '../datatable/datatable.helpers.js';
document.addEventListener("DOMContentLoaded", function () {
    const tabela = $('#tabela');
    if (
        tabela.length > 0 &&
        tabela.find("tbody tr").length > 0 &&
        !tabela.find("tbody tr td[colspan]").length
    ) {
        const config = criarConfiguracaoTabela([
            { searchable: true, targets: [0, 3] }
        ]);
        tabela.DataTable(config);
    }
});