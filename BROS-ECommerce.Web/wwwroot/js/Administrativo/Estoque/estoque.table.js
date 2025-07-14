import { criarConfiguracaoTabela } from '../datatable/datatable.helpers.js';

export function inicializarTabelaEstoque() {
    const tabela = $('#tabela');
    if (
        tabela.length > 0 &&
        tabela.find("tbody tr").length > 0 &&
        !tabela.find("tbody tr td[colspan]").length
    ) {
        const config = criarConfiguracaoTabela([
            { searchable: false, targets: [0, 4] }
        ]);
        tabela.DataTable(config);
    }
}
