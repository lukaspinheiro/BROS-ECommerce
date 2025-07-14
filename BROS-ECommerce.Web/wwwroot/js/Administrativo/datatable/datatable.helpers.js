import { configuracaoPadraoDatatables } from './datatable.config.js';

export function criarConfiguracaoTabela(extraColumnDefs = []) {

    const tituloPagina = document.title.replace(' - GymBros', '');
    const tituloRelatorio = `GymBros - Relatório (${tituloPagina})`;

    const botoesComTitulo = configuracaoPadraoDatatables.buttons.map(botao => ({
        ...botao,
        title: tituloRelatorio
    }));

    return {
        ...configuracaoPadraoDatatables,
        buttons: botoesComTitulo,
        columnDefs: [
            ...configuracaoPadraoDatatables.columnDefs,
            ...extraColumnDefs
        ]
    };
}
