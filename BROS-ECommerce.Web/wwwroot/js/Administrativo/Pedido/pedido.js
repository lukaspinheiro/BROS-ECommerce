document.addEventListener('DOMContentLoaded', function () {
    console.log('🔄 Inicializando JavaScript dos Pedidos...');

    inicializarDataTable();
    configurarEventos();
    inicializarFiltros();

    console.log('✅ JavaScript dos Pedidos inicializado com sucesso!');
});

function inicializarDataTable() {
    try {
        if (typeof $ !== 'undefined' && $('#tabela').length > 0) {
            console.log('📊 Inicializando DataTable...');

            $('#tabela').DataTable({
                language: {
                    url: '//cdn.datatables.net/plug-ins/1.13.6/i18n/pt-BR.json'
                },
                responsive: true,
                pageLength: 25,
                lengthMenu: [[10, 25, 50, 100], [10, 25, 50, 100]],
                order: [[1, 'desc']],
                columnDefs: [
                    { targets: [0, 5], orderable: false },
                    { targets: [4], type: 'currency' }
                ],
                dom: '<"row"<"col-sm-12 col-md-6"B><"col-sm-12 col-md-6"f>>rt<"row"<"col-sm-12 col-md-5"i><"col-sm-12 col-md-7"p>>',
                buttons: [
                    {
                        extend: 'excel',
                        text: '<i class="fa fa-file-excel me-1"></i> Excel',
                        className: 'btn btn-excel btn-sm',
                        title: 'Pedidos_' + new Date().toISOString().split('T')[0],
                        exportOptions: {
                            columns: [0, 1, 2, 3, 4]
                        }
                    },
                    {
                        extend: 'pdf',
                        text: '<i class="fa fa-file-pdf me-1"></i> PDF',
                        className: 'btn btn-pdf btn-sm',
                        title: 'Pedidos_' + new Date().toISOString().split('T')[0],
                        orientation: 'landscape',
                        pageSize: 'A4',
                        exportOptions: {
                            columns: [0, 1, 2, 3, 4]
                        }
                    },
                    {
                        extend: 'csv',
                        text: '<i class="fa fa-file-csv me-1"></i> CSV',
                        className: 'btn btn-csv btn-sm',
                        title: 'Pedidos_' + new Date().toISOString().split('T')[0],
                        exportOptions: {
                            columns: [0, 1, 2, 3, 4]
                        }
                    }
                ]
            });

            console.log('✅ DataTable inicializada com sucesso!');
        }
    } catch (error) {
        console.error('❌ Erro ao inicializar DataTable:', error);
    }
}

function configurarEventos() {
    console.log('🔧 Configurando eventos...');

    document.addEventListener('click', function (e) {
        if (e.target.closest('.status-action')) {
            e.preventDefault();
            const link = e.target.closest('.status-action');
            const idPedido = link.getAttribute('data-id');
            const novoStatus = link.getAttribute('data-status');

            alterarStatusPedido(idPedido, novoStatus);
        }

        if (e.target.closest('.excluir-pedido')) {
            e.preventDefault();
            const button = e.target.closest('.excluir-pedido');
            const idPedido = button.getAttribute('data-id');
            const numeroPedido = button.getAttribute('data-numero');

            confirmarExclusao(idPedido, numeroPedido);
        }
    });

    console.log('✅ Eventos configurados!');
}

function inicializarFiltros() {
    console.log('🔍 Inicializando filtros...');

    const filtros = document.querySelectorAll('.filtro-input');
    filtros.forEach(filtro => {
        filtro.addEventListener('input', function () {
            debounce(aplicarFiltros, 500)();
        });
    });

    const filtroStatus = document.querySelector('select[name="Filtro.Status"]');
    if (filtroStatus) {
        filtroStatus.addEventListener('change', aplicarFiltros);
    }

    console.log('✅ Filtros inicializados!');
}

function alterarStatusPedido(idPedido, novoStatus) {
    console.log(`🔄 Alterando status do pedido ${idPedido} para ${novoStatus}...`);

    mostrarLoading();

    const form = document.createElement('form');
    form.method = 'POST';
    form.action = '/Administrativo/Pedido/AtualizarStatus';
    form.style.display = 'none';

    const tokenInput = document.createElement('input');
    tokenInput.type = 'hidden';
    tokenInput.name = '__RequestVerificationToken';
    tokenInput.value = document.querySelector('input[name="__RequestVerificationToken"]').value;

    const idInput = document.createElement('input');
    idInput.type = 'hidden';
    idInput.name = 'idPedido';
    idInput.value = idPedido;

    const statusInput = document.createElement('input');
    statusInput.type = 'hidden';
    statusInput.name = 'novoStatus';
    statusInput.value = novoStatus;

    form.appendChild(tokenInput);
    form.appendChild(idInput);
    form.appendChild(statusInput);
    document.body.appendChild(form);

    fetch('/Administrativo/Pedido/AtualizarStatus', {
        method: 'POST',
        body: new FormData(form)
    })
        .then(response => {
            if (response.ok) {
                atualizarStatusNaInterface(idPedido, novoStatus);
                mostrarNotificacao(`Status alterado para ${novoStatus} com sucesso!`, 'success');
            } else {
                mostrarNotificacao('Erro ao alterar status do pedido.', 'error');
            }
        })
        .catch(error => {
            console.error('Erro:', error);
            mostrarNotificacao('Erro ao alterar status do pedido.', 'error');
        })
        .finally(() => {
            ocultarLoading();
            document.body.removeChild(form);
        });
}

function atualizarStatusNaInterface(idPedido, novoStatus) {
    const rows = document.querySelectorAll('tbody tr');

    rows.forEach(row => {
        const detalhesLink = row.querySelector('a[href*="detalhes"]');
        if (detalhesLink && detalhesLink.href.includes(idPedido)) {
            const statusBadge = row.querySelector('.badge');
            if (statusBadge) {
                statusBadge.className = 'badge bg-' + getStatusClass(novoStatus);
                statusBadge.textContent = novoStatus;
            }
        }
    });
}

function getStatusClass(status) {
    const statusMap = {
        'Pendente': 'warning',
        'Confirmado': 'info',
        'Processando': 'primary',
        'Enviado': 'success',
        'Entregue': 'success',
        'Cancelado': 'danger'
    };
    return statusMap[status] || 'secondary';
}

function confirmarExclusao(idPedido, numeroPedido) {
    console.log(`❌ Confirmando exclusão do pedido ${numeroPedido}...`);

    document.getElementById('idPedidoExcluir').value = idPedido;
    document.getElementById('numeroPedidoExcluir').textContent = numeroPedido;

    const modal = new bootstrap.Modal(document.getElementById('modalExcluirPedido'));
    modal.show();
}

function aplicarFiltros() {
    console.log('🔍 Aplicando filtros...');

    const table = $('#tabela').DataTable();

    const numeroPedido = document.querySelector('input[name="Filtro.NumeroPedido"]')?.value || '';
    const nomeCliente = document.querySelector('input[name="Filtro.NomeCliente"]')?.value || '';
    const status = document.querySelector('select[name="Filtro.Status"]')?.value || '';

    if (numeroPedido) {
        table.column(0).search(numeroPedido);
    } else {
        table.column(0).search('');
    }

    if (nomeCliente) {
        table.column(2).search(nomeCliente);
    } else {
        table.column(2).search('');
    }

    if (status) {
        table.column(3).search(status);
    } else {
        table.column(3).search('');
    }

    table.draw();
}

function limparFiltros() {
    console.log('🧹 Limpando filtros...');

    document.querySelector('input[name="Filtro.NumeroPedido"]').value = '';
    document.querySelector('input[name="Filtro.NomeCliente"]').value = '';
    document.querySelector('select[name="Filtro.Status"]').value = '';
    document.querySelector('input[name="Filtro.DataInicio"]').value = '';
    document.querySelector('input[name="Filtro.DataFim"]').value = '';

    const table = $('#tabela').DataTable();
    table.search('').columns().search('').draw();
}

function debounce(func, wait) {
    let timeout;
    return function executedFunction(...args) {
        const later = () => {
            clearTimeout(timeout);
            func(...args);
        };
        clearTimeout(timeout);
        timeout = setTimeout(later, wait);
    };
}

function mostrarLoading() {
    console.log('⏳ Mostrando loading...');

    const loading = document.createElement('div');
    loading.id = 'loading-overlay';
    loading.innerHTML = `
        <div class="d-flex justify-content-center align-items-center" style="height: 100vh; position: fixed; top: 0; left: 0; right: 0; bottom: 0; background: rgba(255,255,255,0.8); z-index: 9999;">
            <div class="spinner-border text-primary" role="status">
                <span class="visually-hidden">Carregando...</span>
            </div>
        </div>
    `;
    document.body.appendChild(loading);
}

function ocultarLoading() {
    console.log('✅ Ocultando loading...');

    const loading = document.getElementById('loading-overlay');
    if (loading) {
        loading.remove();
    }
}

function mostrarNotificacao(mensagem, tipo = 'success') {
    console.log(`📢 Notificação: ${mensagem}`);

    const toast = document.createElement('div');
    toast.className = `toast align-items-center text-white bg-${tipo} border-0`;
    toast.setAttribute('role', 'alert');
    toast.setAttribute('aria-live', 'assertive');
    toast.setAttribute('aria-atomic', 'true');
    toast.style.position = 'fixed';
    toast.style.top = '20px';
    toast.style.right = '20px';
    toast.style.zIndex = '9999';

    toast.innerHTML = `
        <div class="d-flex">
            <div class="toast-body">
                ${mensagem}
            </div>
            <button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast"></button>
        </div>
    `;

    document.body.appendChild(toast);

    const bsToast = new bootstrap.Toast(toast);
    bsToast.show();

    setTimeout(() => {
        toast.remove();
    }, 5000);
}

window.pedidoAdmin = {
    alterarStatus: alterarStatusPedido,
    confirmarExclusao: confirmarExclusao,
    aplicarFiltros: aplicarFiltros,
    limparFiltros: limparFiltros,
    mostrarLoading: mostrarLoading,
    ocultarLoading: ocultarLoading,
    mostrarNotificacao: mostrarNotificacao
};