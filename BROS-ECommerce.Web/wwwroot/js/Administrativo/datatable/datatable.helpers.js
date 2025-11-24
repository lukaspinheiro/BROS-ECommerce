import { configuracaoPadraoDatatables } from './datatable.config.js';

export function criarConfiguracaoTabela(extraColumnDefs = []) {

    const tituloPagina = document.title.replace(' - GymBros', '');
    const tituloRelatorio = `(${tituloPagina})`;

    const botoesComTitulo = configuracaoPadraoDatatables.buttons.map(botao => ({
        ...botao,
        title: tituloPagina
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
