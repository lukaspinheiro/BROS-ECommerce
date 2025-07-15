

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
        console.log('CarrinhoSidebar inicializado com ícone existente');
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

        this.carregarCarrinho();
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
            this.abrir();
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
                console.error('Erro ao carregar carrinho:', response.status);
                this.atualizarCarrinho(null);
            }
        } catch (error) {
            console.error('Erro na requisição do carrinho:', error);
            this.atualizarCarrinho(null);
        }
    },

 
    async adicionarProduto(produtoId, quantidade = 1) {
        try {
            console.log('[SIDEBAR] Adicionando produto:', produtoId, 'Quantidade:', quantidade);

            this.mostrarLoader();

            const tokenElement = document.querySelector('input[name="__RequestVerificationToken"]');
            const token = tokenElement ? tokenElement.value : '';

            console.log('[SIDEBAR] Token encontrado:', !!token);
            console.log('[SIDEBAR] Fazendo requisição para /Carrinho/AdicionarProduto');

            const response = await fetch('/Carrinho/AdicionarProduto', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded', 
                },
                body: `produtoId=${encodeURIComponent(produtoId)}&quantidade=${quantidade}${token ? `&__RequestVerificationToken=${encodeURIComponent(token)}` : ''}`
            });

            console.log('[SIDEBAR] Response status:', response.status);
            console.log('[SIDEBAR] Response ok:', response.ok);

            if (!response.ok) {
                const errorText = await response.text();
                console.error('[SIDEBAR] Response error:', errorText);
                throw new Error(`Erro na requisição: ${response.status} - ${response.statusText}`);
            }

            const result = await response.json();
            console.log('[SIDEBAR] Resultado:', result);

            if (result.sucesso) {
                this.mostrarFeedback('Produto adicionado ao carrinho!', 'success');
                this.atualizarCarrinho(result.carrinho);
                this.abrir();
            } else {
                this.mostrarFeedback(result.mensagem || 'Erro ao adicionar produto', 'error');
            }
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
            console.error('Erro ao alterar quantidade:', error);
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
            console.error('Erro ao remover produto:', error);
            this.mostrarFeedback('Erro ao remover produto', 'error');
        }
    },

    
    atualizarCarrinho(carrinhoData) {
        if (!carrinhoData || !carrinhoData.itens || carrinhoData.itens.length === 0) {
            this.mostrarEstadoVazio();
            return;
        }

        this.carrinho = carrinhoData;
        this.mostrarEstadoComItens();
        this.renderizarItens();
        this.atualizarTotais();
        this.atualizarContadores();
    },

  
    mostrarEstadoVazio() {
        if (this.emptyState && this.itemsState) {
            this.emptyState.style.display = 'flex';
            this.itemsState.style.display = 'none';
        }
        this.atualizarContadores(0);
    },

    
    mostrarEstadoComItens() {
        if (this.emptyState && this.itemsState) {
            this.emptyState.style.display = 'none';
            this.itemsState.style.display = 'flex';
        }
    },

    
    renderizarItens() {
        if (!this.itemsList || !this.carrinho.itens) return;

        const template = document.getElementById('carrinho-item-template');
        if (!template) return;

        this.itemsList.innerHTML = '';

        this.carrinho.itens.forEach(item => {
            const itemElement = template.content.cloneNode(true);

            
            itemElement.querySelector('.carrinho-item').dataset.produtoId = item.idProduto;
            itemElement.querySelector('.item-imagem img').src = item.imagemUrl || '/img/produto-placeholder.jpg';
            itemElement.querySelector('.item-imagem img').alt = item.nome;
            itemElement.querySelector('.item-nome').textContent = item.nome;
            itemElement.querySelector('.item-preco').textContent = this.formatarPreco(item.precoUnitario);
            itemElement.querySelector('.quantidade').textContent = item.quantidade;

            this.itemsList.appendChild(itemElement);
        });
    },

    
    atualizarTotais() {
        if (this.totalPreco && this.carrinho.total !== undefined) {
            this.totalPreco.textContent = this.formatarPreco(this.carrinho.total);
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

        const carrinhoIcon = document.querySelector('.fa-cart-shopping');
        if (carrinhoIcon) {
            if (qtd > 0) {
                carrinhoIcon.style.color = '#FF4E4E';
                carrinhoIcon.classList.add('carrinho-com-itens');
            } else {
                carrinhoIcon.style.color = '';
                carrinhoIcon.classList.remove('carrinho-com-itens');
            }
        }
    },

    
    irParaCarrinho() {
        window.location.href = '/Carrinho';
    },

    
    finalizarCompra() {
        if (!this.carrinho.itens || this.carrinho.itens.length === 0) {
            this.mostrarFeedback('Adicione produtos ao carrinho primeiro', 'warning');
            return;
        }
        window.location.href = '/Carrinho/Checkout';
    },

    
    adicionarCupom() {
        this.mostrarFeedback('Funcionalidade de cupom em desenvolvimento', 'info');
    },


    mostrarFeedback(mensagem, tipo = 'success') {
        this.mostrarToast(mensagem, tipo);
    },

    
    mostrarToast(mensagem, tipo = 'success', duracao = 4000) {
        const toast = document.createElement('div');
        toast.className = `carrinho-toast ${tipo}`;

        const icones = {
            success: 'fa-check-circle',
            error: 'fa-exclamation-circle',
            warning: 'fa-exclamation-triangle',
            info: 'fa-info-circle'
        };

        toast.innerHTML = `
            <div class="toast-content">
                <div class="toast-icon ${tipo}">
                    <i class="fas ${icones[tipo] || icones.info}"></i>
                </div>
                <div class="toast-message">${mensagem}</div>
            </div>
            <button class="toast-close">
                <i class="fas fa-times"></i>
            </button>
            <div class="toast-progress ${tipo}"></div>
        `;

        
        document.body.appendChild(toast);

        
        setTimeout(() => {
            toast.classList.add('show');
        }, 100);

        
        const progress = toast.querySelector('.toast-progress');
        if (progress) {
            progress.style.width = '100%';
            setTimeout(() => {
                progress.style.width = '0%';
                progress.style.transition = `width ${duracao}ms linear`;
            }, 200);
        }

        const autoClose = setTimeout(() => {
            this.fecharToast(toast);
        }, duracao);

        const closeBtn = toast.querySelector('.toast-close');
        closeBtn.addEventListener('click', () => {
            clearTimeout(autoClose);
            this.fecharToast(toast);
        });
    },

    fecharToast(toast) {
        toast.classList.remove('show');
        setTimeout(() => {
            if (toast.parentNode) {
                toast.parentNode.removeChild(toast);
            }
        }, 400);
    },

    
    mostrarLoader() {
        const sidebar = document.getElementById('carrinho-sidebar');
        if (sidebar) {
            sidebar.classList.add('loading');
        }

        const navbarCarrinho = document.querySelector('.navbar-carrinho');
        if (navbarCarrinho) {
            navbarCarrinho.classList.add('loading');
        }
    },

    
    ocultarLoader() {
        const sidebar = document.getElementById('carrinho-sidebar');
        if (sidebar) {
            sidebar.classList.remove('loading');
        }

        const navbarCarrinho = document.querySelector('.navbar-carrinho');
        if (navbarCarrinho) {
            navbarCarrinho.classList.remove('loading');
        }
    },

    
    async carregarContadorInicial() {
        try {
            const response = await fetch('/Carrinho/QuantidadeItens');
            if (response.ok) {
                const data = await response.json();
                this.atualizarContadores(data.quantidade);
            }
        } catch (error) {
            console.error('Erro ao carregar contador inicial:', error);
        }
    },
    formatarPreco(valor) {
        return new Intl.NumberFormat('pt-BR', {
            style: 'currency',
            currency: 'BRL'
        }).format(valor || 0);
    }
};


document.addEventListener('DOMContentLoaded', () => {
    CarrinhoSidebar.init();
});


window.CarrinhoSidebar = CarrinhoSidebar;