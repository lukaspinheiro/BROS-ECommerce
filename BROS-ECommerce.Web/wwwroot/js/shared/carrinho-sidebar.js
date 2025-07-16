const CarrinhoSidebar = {
    sidebar: null,
    overlay: null,
    emptyState: null,
    itemsState: null,
    itemsList: null,
    totalElement: null,
    quantidadeElement: null,
    isOpen: false,
    carrinho: null,

    init() {
        console.log('[SIDEBAR] Inicializando CarrinhoSidebar...');

        this.sidebar = document.getElementById('carrinho-sidebar');
        this.overlay = document.getElementById('carrinho-sidebar-overlay');
        this.emptyState = document.getElementById('carrinho-sidebar-empty');
        this.itemsState = document.getElementById('carrinho-sidebar-items');
        this.itemsList = document.getElementById('carrinho-items-list');
        this.totalElement = document.getElementById('carrinho-total-preco');
        this.quantidadeElement = document.getElementById('carrinho-quantidade-items');

        if (!this.sidebar) {
            console.error('[SIDEBAR] Elemento carrinho-sidebar não encontrado');
            return;
        }

        this.configurarEventos();
        this.carregarContadorInicial();

        console.log('[SIDEBAR] CarrinhoSidebar inicializado com sucesso');
    },

    configurarEventos() {
        if (this.overlay) {
            this.overlay.addEventListener('click', () => this.fechar());
        }

        const closeBtn = document.getElementById('carrinho-sidebar-close');
        if (closeBtn) {
            closeBtn.addEventListener('click', () => this.fechar());
        }

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

        console.log('[SIDEBAR] Sidebar aberta');
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
            console.log('[SIDEBAR] Carregando carrinho do servidor...');

            const response = await fetch('/Carrinho/ObterCarrinhoSidebar', {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json'
                }
            });

            if (!response.ok) {
                throw new Error(`HTTP ${response.status}`);
            }

            const carrinhoData = await response.json();
            console.log('[SIDEBAR] Dados do carrinho recebidos:', carrinhoData);

            this.atualizarCarrinho(carrinhoData);
        } catch (error) {
            console.error('[SIDEBAR] Erro ao carregar carrinho:', error);
            this.mostrarEstadoVazio();
        }
    },

    async adicionarProduto(produtoId, quantidade = 1) {
        try {
            console.log('[SIDEBAR] Adicionando produto:', produtoId, 'quantidade:', quantidade);

            const token = document.querySelector('input[name="__RequestVerificationToken"]')?.value;

            const response = await fetch('/Carrinho/AdicionarProduto', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded',
                },
                body: `produtoId=${produtoId}&quantidade=${quantidade}` +
                    (token ? `&__RequestVerificationToken=${token}` : '')
            });

            const result = await response.json();
            console.log('[SIDEBAR] Resposta do servidor:', result);

            if (result.sucesso) {
                this.mostrarFeedback('Adicionado ao carrinho!', 'success');
                this.atualizarCarrinho(result.carrinho);
                this.abrir();
            } else {
                if (result.redirectToLogin) {
                    this.mostrarFeedback('Redirecionando...', 'info');
                    setTimeout(() => {
                        window.location.href = '/Autenticacao/Login';
                    }, 800);
                } else {
                    this.mostrarFeedback(result.mensagem || 'Erro ao adicionar produto', 'error');
                }
            }

            return result;
        } catch (error) {
            console.error('[SIDEBAR] Erro ao adicionar produto:', error);
            this.mostrarFeedback('Erro ao adicionar produto ao carrinho', 'error');
            throw error;
        }
    },

    async alterarQuantidade(produtoId, novaQuantidade) {
        if (novaQuantidade < 0) return;

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
                this.mostrarFeedback('Quantidade atualizada', 'success');
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
                this.mostrarFeedback('Removido do carrinho', 'success');
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

        this.carrinho.itens.forEach(item => {
            console.log('[SIDEBAR] Renderizando item:', item.nome);
            const itemElement = this.criarElementoItem(item);
            this.itemsList.appendChild(itemElement);
        });

        console.log('[SIDEBAR] =================================');
    },

    criarElementoItem(item) {
        const itemDiv = document.createElement('div');
        itemDiv.className = 'carrinho-item';
        itemDiv.innerHTML = `
            <div class="item-imagem">
                <img src="${item.imagemUrl || '/images/produto-sem-imagem.png'}" alt="${item.nome}">
            </div>
            <div class="item-info">
                <h4 class="item-nome">${item.nome}</h4>
                <p class="item-preco">R$ ${item.precoUnitario.toFixed(2).replace('.', ',')}</p>
                <div class="item-quantidade">
                    <button onclick="CarrinhoSidebar.alterarQuantidade('${item.idProduto}', ${item.quantidade - 1})" 
                            class="btn-quantidade" ${item.quantidade <= 1 ? 'disabled' : ''}>
                        <i class="fas fa-minus"></i>
                    </button>
                    <span class="quantidade">${item.quantidade}</span>
                    <button onclick="CarrinhoSidebar.alterarQuantidade('${item.idProduto}', ${item.quantidade + 1})" 
                            class="btn-quantidade">
                        <i class="fas fa-plus"></i>
                    </button>
                </div>
                <div class="item-subtotal">
                    Subtotal: R$ ${item.subtotal.toFixed(2).replace('.', ',')}
                </div>
            </div>
            <button onclick="CarrinhoSidebar.removerItem('${item.idProduto}')" class="btn-remover">
                <i class="fas fa-trash"></i>
            </button>
        `;
        return itemDiv;
    },

    atualizarTotais() {
        if (this.totalElement && this.carrinho) {
            this.totalElement.textContent = `R$ ${this.carrinho.total.toFixed(2).replace('.', ',')}`;
        }
    },

    atualizarContadores(quantidade = null) {
        const qtd = quantidade !== null ? quantidade : (this.carrinho?.quantidadeTotal || 0);

        if (this.quantidadeElement) {
            this.quantidadeElement.textContent = qtd;
        }

        const contadorCarrinho = document.getElementById('contador-carrinho');
        if (contadorCarrinho) {
            contadorCarrinho.textContent = qtd;
            contadorCarrinho.style.display = qtd > 0 ? 'inline' : 'none';
        }

        console.log('[SIDEBAR] Contadores atualizados para:', qtd);
    },

    mostrarFeedback(mensagem, tipo = 'info') {
        let toast = document.querySelector('.carrinho-toast');
        if (!toast) {
            toast = document.createElement('div');
            toast.className = 'carrinho-toast';
            document.body.appendChild(toast);
        }

        toast.innerHTML = `
            <div class="toast-content">
                <div class="toast-icon">
                    <i class="${this.obterIconeFeedback(tipo)}"></i>
                </div>
                <div class="toast-message">${mensagem}</div>
            </div>
            <button class="toast-close">
                <i class="fas fa-times"></i>
            </button>
        `;

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

    obterIconeFeedback(tipo) {
        switch (tipo) {
            case 'success': return 'fas fa-check-circle';
            case 'error': return 'fas fa-exclamation-circle';
            default: return 'fas fa-info-circle';
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

    async irParaCarrinho() {
        try {
            const response = await fetch('/Carrinho/ValidarEstoqueCompleto', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]')?.value || ''
                }
            });

            if (response.status === 401) {
                this.mostrarFeedback('Redirecionando...', 'info');
                setTimeout(() => {
                    window.location.href = '/Autenticacao/Login';
                }, 800);
                return;
            }

            const result = await response.json();

            if (!result.sucesso && result.carrinho) {
                this.atualizarCarrinho(result.carrinho);
                this.mostrarFeedback(result.mensagem, 'error');
            }

            window.location.href = '/Carrinho';
        } catch (error) {
            console.error('[SIDEBAR] Erro ao validar estoque:', error);
            window.location.href = '/Carrinho';
        }
    },

    async finalizarCompra() {
        try {
            const response = await fetch('/Carrinho/ValidarEstoqueCompleto', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]')?.value || ''
                }
            });

            if (response.status === 401) {
                this.mostrarFeedback('Redirecionando...', 'info');
                setTimeout(() => {
                    window.location.href = '/Autenticacao/Login';
                }, 800);
                return;
            }

            const result = await response.json();

            if (result.sucesso) {
                window.location.href = '/Carrinho/Checkout';
            } else {
                this.mostrarFeedback(result.mensagem, 'error');

                if (result.carrinho) {
                    this.atualizarCarrinho(result.carrinho);
                }
            }
        } catch (error) {
            console.error('[SIDEBAR] Erro ao validar estoque:', error);
            this.mostrarFeedback('Erro ao validar estoque. Tente novamente.', 'error');
        }
    }
};

document.addEventListener('DOMContentLoaded', function () {
    CarrinhoSidebar.init();
});

window.CarrinhoSidebar = CarrinhoSidebar;