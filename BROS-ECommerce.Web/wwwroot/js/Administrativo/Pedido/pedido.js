document.addEventListener('DOMContentLoaded', function () {
    console.log('🔄 Inicializando JavaScript dos Pedidos...');

    
    inicializarDataTable();

    
    configurarEventos();

    console.log('✅ JavaScript dos Pedidos inicializado com sucesso!');
});

function inicializarDataTable() {
    try {
        if (typeof $ !== 'undefined' && $('#tabela').length > 0) {
            console.log('📊 Inicializando DataTable...');

            
            import('../datatable/datatable.config.js').then(module => {
                const config = module.configuracaoPadraoDatatables;
                $('#tabela').DataTable(config);
                console.log('✅ DataTable inicializada com sucesso!');
            }).catch(error => {
                console.error('❌ Erro ao carregar configuração do DataTable:', error);
                
                $('#tabela').DataTable({
                    language: {
                        url: 'https://cdn.datatables.net/plug-ins/1.10.25/i18n/Portuguese-Brasil.json'
                    }
                });
            });
        } else {
            console.log('⚠️ DataTable não disponível ou tabela não encontrada');
        }
    } catch (error) {
        console.error('❌ Erro ao inicializar DataTable:', error);
    }
}

function configurarEventos() {
    console.log('🎯 Configurando eventos dos botões...');

   
    document.addEventListener('click', function (e) {

        
        if (e.target.closest('.excluir-pedido')) {
            e.preventDefault();
            console.log('🗑️ Clique no botão excluir detectado');

            const button = e.target.closest('.excluir-pedido');
            const idPedido = button.getAttribute('data-id');
            const numeroPedido = button.getAttribute('data-numero');

            console.log('📋 Dados do pedido:', { idPedido, numeroPedido });

            if (!idPedido || !numeroPedido) {
                console.error('❌ Dados do pedido não encontrados');
                alert('Erro: Dados do pedido não encontrados');
                return;
            }

           
            document.getElementById('idPedidoExcluir').value = idPedido;
            document.getElementById('numeroPedidoExcluir').textContent = numeroPedido;

            
            const modal = new bootstrap.Modal(document.getElementById('modalExcluirPedido'));
            modal.show();

            console.log('✅ Modal de exclusão aberto');
        }

        
        if (e.target.closest('.status-action')) {
            e.preventDefault();
            console.log('🔄 Clique no botão status detectado');

            const link = e.target.closest('.status-action');
            const idPedido = link.getAttribute('data-id');
            const novoStatus = link.getAttribute('data-status');

            console.log('📋 Dados do status:', { idPedido, novoStatus });

            if (!idPedido || !novoStatus) {
                console.error('❌ Dados do status não encontrados');
                alert('Erro: Dados do status não encontrados');
                return;
            }

            
            document.getElementById('idPedidoStatus').value = idPedido;
            document.getElementById('novoStatus').value = novoStatus;
            document.getElementById('novoStatusText').textContent = novoStatus;

            
            const modal = new bootstrap.Modal(document.getElementById('modalAlterarStatus'));
            modal.show();

            console.log('✅ Modal de alteração de status aberto');
        }
    });

    console.log('✅ Eventos configurados com sucesso!');
}