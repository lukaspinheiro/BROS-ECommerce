export const configuracaoPadraoDatatables = {
    dom: 'Bfrtip',
    responsive: true,
    pageLength: 10,
    order: [[0, "asc"]],
    language: {
        url: 'https://cdn.datatables.net/plug-ins/1.10.25/i18n/Portuguese-Brasil.json'
    },
    buttons: [
        {
            extend: 'csvHtml5',
            text: 'CSV',
            charset: "utf-8",
            bom: true,
            title: 'GymBros - Relatório',
            exportOptions: {
                modifier: { search: 'none' },
                columns: function (idx, data, node) {
                    const temClasseIgnorada =
                        node.classList.contains('nao-exportar') ||
                        node.classList.contains('img-relatorio');
                    const contemSlug = node.innerText.includes('Caminho URL:');
                    return !temClasseIgnorada && !contemSlug;
                }
            }
        },
        {
            extend: 'excelHtml5',
            text: 'EXCEL',
            title: 'GymBros - Relatório',
            exportOptions: {
                modifier: { search: 'none' },
                columns: function (idx, data, node) {
                    const temClasseIgnorada =
                        node.classList.contains('nao-exportar') ||
                        node.classList.contains('img-relatorio');
                    const contemSlug = node.innerText.includes('Caminho URL:');
                    return !temClasseIgnorada && !contemSlug;
                }
            }
        },
        {
            extend: 'pdfHtml5',
            text: 'PDF',
            title: 'GymBros - Relatório',
            orientation: 'landscape',
            pageSize: 'A4',
            exportOptions: {
                modifier: { search: 'none' },
                columns: function (idx, data, node) {
                    const temClasseIgnorada =
                        node.classList.contains('nao-exportar');
                    const contemSlug = node.innerText.includes('Caminho URL:');
                    return !temClasseIgnorada && !contemSlug;
                }
            },
            customize: function (doc) {
                const tabela = document.querySelector('#tabela');
                const linhas = tabela.querySelectorAll('tbody tr');

                linhas.forEach((linha, i) => {
                    const nomeCelula = linha.querySelector('td:nth-child(2)');
                    if (nomeCelula) {
                        nomeCelula.innerHTML = nomeCelula.innerHTML.replace(
                            /<div class="nao-exportar">[\s\S]*?<\/div>/,
                            ''
                        );
                    }
                    const img = linha.querySelector('img.img-relatorio');
                    if (img) {
                        const canvas = document.createElement('canvas');
                        const size = 45;
                        canvas.width = size;
                        canvas.height = size;
                        const ctx = canvas.getContext('2d');
                        ctx.drawImage(img, 0, 0, size, size);

                        const dataUrl = canvas.toDataURL('image/jpeg', 1); 
                        const pdfImg = {
                            image: dataUrl,
                            fit: [size, size],
                            alignment: 'left',
                            margin: [0, 2, 0, 2]
                        };

                        doc.content[1].table.body[i + 1][0] = pdfImg;
                    }
                });
                doc.styles.tableHeader.alignment = 'left';
                doc.styles.title = {
                    fontSize: 16,
                    alignment: 'left'
                };
                doc.content[1].table.widths = Array(doc.content[1].table.body[0].length + 1).join('*').split('');
            }
        }
    ],
    columnDefs: [
        { className: 'text-start', targets: '_all' },
        { type: 'string', targets: '_all' }
    ]
};
