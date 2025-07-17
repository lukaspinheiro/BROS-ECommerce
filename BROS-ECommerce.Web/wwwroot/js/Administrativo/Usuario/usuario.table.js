import { criarConfiguracaoTabela } from '../datatable/datatable.helpers.js';

export function inicializarTabela() {
    const tabela = $('#tabela');
    if (
        tabela.length > 0 &&
        tabela.find("tbody tr").length > 0 &&
        !tabela.find("tbody tr td[colspan]").length
    ) {
        const config = criarConfiguracaoTabela([
            { searchable: false, targets: [0, 7] }
        ]);
        tabela.DataTable(config);
    }
}
