const CarrinhoSidebar = {
    sidebar: null,
    overlay: null,
    quantidadeItems: null,
    emptyState: null,
    itemsState: null,
    itemsList: null,
    totalPreco: null,

    isOpen: false,
    carrinho: {
        items: [],
        total: 0,
        quantidadeTotal: 0
    },

    init() {
        this.bindElements();
        this.bindEvents();
        this.carregarCarrinho();
        this.carregarContadorInicial();
        console.log('[SIDEBAR] CarrinhoSidebar inicializado');
    },

    bindElements() {
        this.sidebar = document.getElementById('carrinho-sidebar');
        this.overlay = document.getElementById('carrinho-sidebar-overlay');
        this.quantidadeItems = document.getElementById('carrinho-quantidade-items');
        this.emptyState = document.getElementById('carrinho-sidebar-empty');
        this.itemsState = document.getElementById('carrinho-sidebar-items');
        this.itemsList = document.getElementById('carrinho-items-list');
        this.totalPreco = document.getElementById('carrinho-total-preco');
    },

    bindEvents() {
        document.getElementById('carrinho-sidebar-close')?.addEventListener('click', () => {
            this.fechar();
        });

        this.overlay?.addEventListener('click', () => {
            this.fechar();
        });

        document.addEventListener('keydown', (e) => {
            if (e.key === 'Escape' && this.isOpen) {
                this.fechar();
            }
        });

        const carrinhoLink = document.querySelector('a[href="/Carrinho"]');
        if (carrinhoLink) {
            carrinhoLink.addEventListener('click', (e) => {
                e.preventDefault();
                this.toggle();
            });
        }

        const carrinhoIcon = document.querySelector('.fa-cart-shopping');
        if (carrinhoIcon) {
            carrinhoIcon.addEventListener('click', (e) => {
                e.preventDefault();
                e.stopPropagation();
                this.toggle();
            });
        }
    },

    abrir() {
        if (!this.sidebar || !this.overlay) return;

        this.isOpen = true;
        this.sidebar.classList.add('active');
        this.overlay.classList.add('active');
        document.body.style.overflow = 'hidden';

        console.log('[SIDEBAR] Sidebar aberta - NÃO recarregando carrinho para evitar sobrescrita');
    },

    async abrirECarregar() {
        if (!this.sidebar || !this.overlay) return;

        this.isOpen = true;
        this.sidebar.classList.add('active');
        this.overlay.classList.add('active');
        document.body.style.overflow = 'hidden';

        console.log('[SIDEBAR] Sidebar aberta - carregando carrinho do servidor');
        await this.carregarCarrinho();
    },

    fechar() {
        if (!this.sidebar || !this.overlay) return;

        this.isOpen = false;
        this.sidebar.classList.remove('active');
        this.overlay.classList.remove('active');
        document.body.style.overflow = '';
    },

    toggle() {
        if (this.isOpen) {
            this.fechar();
        } else {
            this.abrirECarregar();
        }
    },

    async carregarCarrinho() {
        try {
            const response = await fetch('/Carrinho/ObterCarrinhoSidebar', {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json',
                    'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]')?.value || ''
                }
            });

            if (response.ok) {
                const data = await response.json();
                this.atualizarCarrinho(data);
            } else {
                console.error('[SIDEBAR] Erro ao carregar carrinho:', response.status);
                this.atualizarCarrinho(null);
            }
        } catch (error) {
            console.error('[SIDEBAR] Erro na requisição do carrinho:', error);
            this.atualizarCarrinho(null);
        }
    },

    async adicionarProduto(produtoId, quantidade = 1) {
        try {
            console.log('[SIDEBAR] =================================');
            console.log('[SIDEBAR] INÍCIO - Adicionando produto');
            console.log('[SIDEBAR] Produto ID:', produtoId);
            console.log('[SIDEBAR] Quantidade:', quantidade);

            this.mostrarLoader();

            const tokenElement = document.querySelector('input[name="__RequestVerificationToken"]');
            const token = tokenElement ? tokenElement.value : '';

            console.log('[SIDEBAR] Token encontrado:', !!token);

            const response = await fetch('/Carrinho/AdicionarProduto', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded',
                },
                body: `produtoId=${encodeURIComponent(produtoId)}&quantidade=${quantidade}${token ? `&__RequestVerificationToken=${encodeURIComponent(token)}` : ''}`
            });

            console.log('[SIDEBAR] Response status:', response.status);

            if (!response.ok) {
                const errorText = await response.text();
                console.error('[SIDEBAR] Response error:', errorText);
                throw new Error(`Erro na requisição: ${response.status} - ${response.statusText}`);
            }

            const result = await response.json();
            console.log('[SIDEBAR] Resultado recebido:', result);

            if (result.sucesso) {
                console.log('[SIDEBAR] Produto adicionado com sucesso!');
                console.log('[SIDEBAR] Dados do carrinho:', result.carrinho);

                this.mostrarFeedback('Produto adicionado ao carrinho!', 'success');
                this.atualizarCarrinho(result.carrinho);
                this.abrir();

                console.log('[SIDEBAR] Sidebar atualizada e aberta');
            } else {
                console.log('[SIDEBAR] Falha ao adicionar:', result.mensagem);
                this.mostrarFeedback(result.mensagem || 'Erro ao adicionar produto', 'error');
            }

            console.log('[SIDEBAR] =================================');
        } catch (error) {
            console.error('[SIDEBAR] Erro ao adicionar produto:', error);
            this.mostrarFeedback('Erro ao adicionar produto ao carrinho: ' + error.message, 'error');
        } finally {
            this.ocultarLoader();
        }
    },

    async alterarQuantidade(button, incremento) {
        const item = button.closest('.carrinho-item');
        const produtoId = item.dataset.produtoId;
        const quantidadeAtual = parseInt(item.querySelector('.quantidade').textContent);
        const novaQuantidade = quantidadeAtual + incremento;

        if (novaQuantidade <= 0) {
            await this.removerItem(produtoId);
            return;
        }

        try {
            const response = await fetch('/Carrinho/AtualizarQuantidadeSidebar', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]')?.value || ''
                },
                body: JSON.stringify({
                    produtoId: produtoId,
                    quantidade: novaQuantidade
                })
            });

            const result = await response.json();

            if (result.sucesso) {
                this.atualizarCarrinho(result.carrinho);
            } else {
                this.mostrarFeedback(result.mensagem || 'Erro ao atualizar quantidade', 'error');
            }
        } catch (error) {
            console.error('[SIDEBAR] Erro ao alterar quantidade:', error);
            this.mostrarFeedback('Erro ao atualizar quantidade', 'error');
        }
    },

    async removerItem(produtoId) {
        try {
            const response = await fetch('/Carrinho/RemoverProdutoSidebar', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]')?.value || ''
                },
                body: JSON.stringify({
                    produtoId: produtoId
                })
            });

            const result = await response.json();

            if (result.sucesso) {
                this.mostrarFeedback('Produto removido do carrinho', 'success');
                this.atualizarCarrinho(result.carrinho);
            } else {
                this.mostrarFeedback(result.mensagem || 'Erro ao remover produto', 'error');
            }
        } catch (error) {
            console.error('[SIDEBAR] Erro ao remover produto:', error);
            this.mostrarFeedback('Erro ao remover produto', 'error');
        }
    },

    atualizarCarrinho(carrinhoData) {
        console.log('[SIDEBAR] =================================');
        console.log('[SIDEBAR] INÍCIO - Atualizando carrinho');
        console.log('[SIDEBAR] Dados recebidos:', carrinhoData);

        if (!carrinhoData || !carrinhoData.itens || carrinhoData.itens.length === 0) {
            console.log('[SIDEBAR] Carrinho vazio - mostrando estado vazio');
            this.mostrarEstadoVazio();
            return;
        }

        console.log('[SIDEBAR] Carrinho com itens:', carrinhoData.itens.length);

        this.carrinho = carrinhoData;
        this.mostrarEstadoComItens();
        this.renderizarItens();
        this.atualizarTotais();
        this.atualizarContadores();

        console.log('[SIDEBAR] Carrinho atualizado com sucesso');
        console.log('[SIDEBAR] =================================');
    },

    mostrarEstadoVazio() {
        console.log('[SIDEBAR] Mostrando estado vazio');
        if (this.emptyState && this.itemsState) {
            this.emptyState.style.display = 'flex';
            this.itemsState.style.display = 'none';
        }
        this.atualizarContadores(0);
    },

    mostrarEstadoComItens() {
        console.log('[SIDEBAR] Mostrando estado com itens');
        if (this.emptyState && this.itemsState) {
            this.emptyState.style.display = 'none';
            this.itemsState.style.display = 'flex';
        }
    },

    renderizarItens() {
        console.log('[SIDEBAR] =================================');
        console.log('[SIDEBAR] INÍCIO - Renderizando itens');

        if (!this.itemsList) {
            console.error('[SIDEBAR] itemsList não encontrado');
            return;
        }

        if (!this.carrinho.itens) {
            console.error('[SIDEBAR] carrinho.itens não encontrado');
            return;
        }

        const template = document.getElementById('carrinho-item-template');
        if (!template) {
            console.error('[SIDEBAR] Template carrinho-item-template não encontrado');
            return;
        }

        console.log('[SIDEBAR] Limpando lista e renderizando', this.carrinho.itens.length, 'itens');
        this.itemsList.innerHTML = '';

        this.carrinho.itens.forEach((item, index) => {
            console.log(`[SIDEBAR] Renderizando item ${index + 1}:`, item);

            const itemElement = template.content.cloneNode(true);

            const carrinhoItem = itemElement.querySelector('.carrinho-item');
            if (carrinhoItem) {
                carrinhoItem.dataset.produtoId = item.idProduto;
            }

            const img = itemElement.querySelector('.item-imagem img');
            if (img) {
                img.src = item.imagemUrl || '/img/produto-placeholder.jpg';
                img.alt = item.nome;
            }

            const nome = itemElement.querySelector('.item-nome');
            if (nome) {
                nome.textContent = item.nome;
            }

            const preco = itemElement.querySelector('.item-preco');
            if (preco) {
                preco.textContent = this.formatarPreco(item.precoUnitario);
            }

            const quantidade = itemElement.querySelector('.quantidade');
            if (quantidade) {
                quantidade.textContent = item.quantidade;
            }

            this.itemsList.appendChild(itemElement);
            console.log(`[SIDEBAR] Item ${index + 1} adicionado ao DOM`);
        });

        console.log('[SIDEBAR] Renderização concluída');
        console.log('[SIDEBAR] =================================');
    },

    atualizarTotais() {
        if (this.totalPreco && this.carrinho.total !== undefined) {
            const totalFormatado = this.formatarPreco(this.carrinho.total);
            this.totalPreco.textContent = totalFormatado;
            console.log('[SIDEBAR] Total atualizado:', totalFormatado);
        }
    },

    atualizarContadores(quantidade = null) {
        const qtd = quantidade !== null ? quantidade : (this.carrinho.quantidadeTotal || 0);

        if (this.quantidadeItems) {
            this.quantidadeItems.textContent = qtd;
        }

        const contadorExistente = document.querySelector('#contador-carrinho');
        if (contadorExistente) {
            contadorExistente.textContent = qtd;
            contadorExistente.style.display = qtd > 0 ? 'inline-block' : 'none';
        }

        console.log('[SIDEBAR] Contadores atualizados:', qtd);
    },

    formatarPreco(valor) {
        return new Intl.NumberFormat('pt-BR', {
            style: 'currency',
            currency: 'BRL'
        }).format(valor);
    },

    mostrarLoader() {
        console.log('[SIDEBAR] Mostrando loader');
        let loader = document.getElementById('carrinho-loader');
        if (!loader) {
            loader = document.createElement('div');
            loader.id = 'carrinho-loader';
            loader.innerHTML = `
                <div style="position: fixed; top: 0; left: 0; width: 100%; height: 100%; 
                            background: rgba(0,0,0,0.5); z-index: 10000; display: flex; 
                            align-items: center; justify-content: center;">
                    <div style="background: white; padding: 2rem; border-radius: 8px; 
                               display: flex; align-items: center; gap: 1rem;">
                        <div style="width: 20px; height: 20px; border: 3px solid #f3f3f3; 
                                   border-top: 3px solid #FF4E4E; border-radius: 50%; 
                                   animation: spin 1s linear infinite;"></div>
                        <span>Adicionando produto...</span>
                    </div>
                </div>
                <style>
                    @keyframes spin {
                        0% { transform: rotate(0deg); }
                        100% { transform: rotate(360deg); }
                    }
                </style>
            `;
            document.body.appendChild(loader);
        }
        loader.style.display = 'flex';
    },

    ocultarLoader() {
        console.log('[SIDEBAR] Ocultando loader');
        const loader = document.getElementById('carrinho-loader');
        if (loader) {
            loader.style.display = 'none';
        }
    },

    mostrarFeedback(mensagem, tipo = 'success') {
        console.log(`[SIDEBAR] Feedback ${tipo}:`, mensagem);

        const toast = document.getElementById('carrinho-toast');
        if (!toast) {
            console.error('[SIDEBAR] Toast element não encontrado');
            return;
        }

        const toastMessage = toast.querySelector('.toast-message');
        const toastIcon = toast.querySelector('.toast-icon i');

        if (toastMessage) {
            toastMessage.textContent = mensagem;
        }

        if (toastIcon) {
            toastIcon.className = tipo === 'success' ? 'fas fa-check-circle' : 'fas fa-exclamation-circle';
        }

        toast.className = `carrinho-toast ${tipo} show`;

        setTimeout(() => {
            toast.classList.remove('show');
        }, 3000);

        const closeBtn = toast.querySelector('.toast-close');
        if (closeBtn) {
            closeBtn.onclick = () => {
                toast.classList.remove('show');
            };
        }
    },

    carregarContadorInicial() {
        fetch('/Carrinho/QuantidadeItens')
            .then(response => response.json())
            .then(data => {
                this.atualizarContadores(data.quantidade || 0);
                console.log('[SIDEBAR] Contador inicial carregado:', data.quantidade || 0);
            })
            .catch(error => {
                console.error('[SIDEBAR] Erro ao carregar contador inicial:', error);
                this.atualizarContadores(0);
            });
    },

    adicionarCupom() {
        this.mostrarFeedback('Funcionalidade de cupom em desenvolvimento', 'info');
    },

    irParaCarrinho() {
        window.location.href = '/Carrinho';
    },

    finalizarCompra() {
        window.location.href = '/Carrinho/Checkout';
    }
};

document.addEventListener('DOMContentLoaded', function () {
    CarrinhoSidebar.init();
});

window.CarrinhoSidebar = CarrinhoSidebar;