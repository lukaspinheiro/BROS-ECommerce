import { criarConfiguracaoTabela } from '../datatable/datatable.helpers.js';

document.addEventListener("DOMContentLoaded", function () {

    const tabela = $('#tabela');
    if (
        tabela.length > 0 &&
        tabela.find("tbody tr").length > 0 &&
        !tabela.find("tbody tr td[colspan]").length
    ) {
        const config = criarConfiguracaoTabela([
            { searchable: false, targets: [1, 4] }
        ]);
        tabela.DataTable(config);
    }
});
