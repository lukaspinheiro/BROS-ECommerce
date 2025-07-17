class CheckoutCarrinho {
    constructor() {
        this.carrinhoData = null;
        this.isLoading = false;
        this.init();
    }

    async init() {
        await this.carregarDadosCarrinho();
        this.atualizarResumo();
        this.configurarObservadores();
    }

    async carregarDadosCarrinho() {
        if (this.isLoading) return;

        this.isLoading = true;

        try {
            const response = await fetch('/Carrinho/ObterCarrinhoSidebar', {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json'
                },
                credentials: 'include'
            });

            if (response.ok) {
                const carrinhoData = await response.json();

                if (carrinhoData && !carrinhoData.erro) {
                    this.carrinhoData = carrinhoData;
                    this.mostrarLogSucesso('Dados do carrinho carregados com sucesso');

                    if (!carrinhoData.itens || carrinhoData.itens.length === 0) {
                        this.mostrarEstadoVazio();
                        this.mostrarLogAviso('Carrinho está vazio');
                        this.redirecionarParaCarrinho();
                    } else {
                        this.mostrarEstadoComProdutos();
                    }
                } else {
                    this.mostrarEstadoVazio();
                    this.mostrarLogAviso('Carrinho vazio ou não disponível');
                    this.redirecionarParaCarrinho();
                }
            } else {
                throw new Error('Erro ao carregar carrinho');
            }
        } catch (error) {
            console.error('Erro ao carregar dados do carrinho:', error);
            this.mostrarLogErro('Erro ao carregar dados do carrinho');
            this.mostrarEstadoVazio();
            this.redirecionarParaCarrinho();
        } finally {
            this.isLoading = false;
        }
    }

    mostrarEstadoComProdutos() {
        const summaryContent = document.getElementById('summary-content');
        const summaryEmpty = document.getElementById('summary-empty');
        const summaryLoading = document.getElementById('summary-loading');

        if (summaryContent) summaryContent.style.display = 'block';
        if (summaryEmpty) summaryEmpty.style.display = 'none';
        if (summaryLoading) summaryLoading.style.display = 'none';
    }

    mostrarEstadoVazio() {
        const summaryContent = document.getElementById('summary-content');
        const summaryEmpty = document.getElementById('summary-empty');
        const summaryLoading = document.getElementById('summary-loading');

        if (summaryContent) summaryContent.style.display = 'none';
        if (summaryEmpty) summaryEmpty.style.display = 'block';
        if (summaryLoading) summaryLoading.style.display = 'none';
    }

    atualizarResumo() {
        if (!this.carrinhoData) return;

        this.atualizarListaProdutos();
        this.atualizarSubtotal();
        this.atualizarQuantidadeItens();
        this.atualizarTotal();
        this.atualizarEstadoFrete();
    }

    atualizarListaProdutos() {
        const container = document.getElementById('products-container');
        if (!container || !this.carrinhoData.itens) return;

        container.innerHTML = '';

        this.carrinhoData.itens.forEach(item => {
            const productElement = this.criarElementoProduto(item);
            container.appendChild(productElement);
        });

        // Controlar scroll baseado na quantidade de produtos
        const quantidadeProdutos = this.carrinhoData.itens.length;
        container.setAttribute('data-product-count', quantidadeProdutos);

        // Log para debug
        console.log(`[CHECKOUT] ${quantidadeProdutos} produtos carregados no resumo`);
    }

    criarElementoProduto(item) {
        const productDiv = document.createElement('div');
        productDiv.className = 'product-item';

        const imagemUrl = item.imagemUrl || '/images/produto-sem-imagem.png';
        const precoFormatado = this.formatarMoeda(item.subtotal || 0);

        productDiv.innerHTML = `
            <img src="${imagemUrl}" 
                 alt="${item.nome}" 
                 class="product-image"
                 onerror="this.src='/images/produto-sem-imagem.png'">
            <div class="product-details">
                <div class="product-name" title="${item.nome}">${item.nome}</div>
                <div class="product-info">
                    <span class="product-quantity">Qtd: ${item.quantidade}</span>
                    <span class="product-price">${precoFormatado}</span>
                </div>
            </div>
        `;

        return productDiv;
    }

    atualizarSubtotal() {
        const subtotalElements = document.querySelectorAll('[data-resumo="subtotal"]');
        const valor = this.formatarMoeda(this.carrinhoData.total || 0);

        subtotalElements.forEach(element => {
            if (element) element.textContent = valor;
        });

        const resumoSubtotal = document.querySelector('#resumo-subtotal, .resumo-subtotal');
        if (resumoSubtotal) {
            resumoSubtotal.textContent = valor;
        }
    }

    atualizarQuantidadeItens() {
        const quantidadeElements = document.querySelectorAll('[data-resumo="quantidade"]');
        const quantidade = this.carrinhoData.quantidadeTotal || 0;

        quantidadeElements.forEach(element => {
            if (element) element.textContent = quantidade;
        });

        const resumoQuantidade = document.querySelector('#resumo-quantidade, .resumo-quantidade');
        if (resumoQuantidade) {
            resumoQuantidade.textContent = quantidade;
        }
    }

    atualizarTotal() {
        const totalElements = document.querySelectorAll('[data-resumo="total"]');
        const valor = this.formatarMoeda(this.carrinhoData.total || 0);

        totalElements.forEach(element => {
            if (element) element.textContent = valor;
        });

        const resumoTotal = document.querySelector('#resumo-total, .resumo-total');
        if (resumoTotal) {
            resumoTotal.textContent = valor;
        }
    }

    atualizarEstadoFrete() {
        const freteElements = document.querySelectorAll('[data-resumo="frete"]');

        freteElements.forEach(element => {
            if (element) element.textContent = 'A calcular';
        });

        const resumoFrete = document.querySelector('#resumo-frete, .resumo-frete');
        if (resumoFrete) {
            resumoFrete.textContent = 'A calcular';
        }
    }

    formatarMoeda(valor) {
        return new Intl.NumberFormat('pt-BR', {
            style: 'currency',
            currency: 'BRL'
        }).format(valor);
    }

    configurarObservadores() {
        const observer = new MutationObserver((mutations) => {
            mutations.forEach((mutation) => {
                if (mutation.type === 'childList') {
                    mutation.addedNodes.forEach((node) => {
                        if (node.nodeType === Node.ELEMENT_NODE) {
                            const resumoElements = node.querySelectorAll('[data-resumo]');
                            if (resumoElements.length > 0) {
                                this.atualizarResumo();
                            }
                        }
                    });
                }
            });
        });

        observer.observe(document.body, {
            childList: true,
            subtree: true
        });
    }

    redirecionarParaCarrinho() {
        setTimeout(() => {
            this.mostrarLogInfo('Redirecionando para o carrinho...');
            setTimeout(() => {
                window.location.href = '/Carrinho';
            }, 1500);
        }, 2000);
    }

    async recarregarCarrinho() {
        this.carrinhoData = null;
        await this.carregarDadosCarrinho();
        this.atualizarResumo();
        return this.carrinhoData;
    }

    obterDadosCarrinho() {
        return this.carrinhoData;
    }

    obterSubtotal() {
        return this.carrinhoData?.total || 0;
    }

    obterQuantidadeTotal() {
        return this.carrinhoData?.quantidadeTotal || 0;
    }

    obterItens() {
        return this.carrinhoData?.itens || [];
    }

    temItens() {
        return this.obterItens().length > 0;
    }

    calcularTotalComFrete(valorFrete = 0) {
        const subtotal = this.obterSubtotal();
        const total = subtotal + valorFrete;

        const freteElements = document.querySelectorAll('[data-resumo="frete"]');
        const valorFreteFormatado = valorFrete > 0 ? this.formatarMoeda(valorFrete) : 'A calcular';

        freteElements.forEach(element => {
            if (element) element.textContent = valorFreteFormatado;
        });

        const resumoFrete = document.querySelector('#resumo-frete, .resumo-frete');
        if (resumoFrete) {
            resumoFrete.textContent = valorFreteFormatado;
        }

        const totalElements = document.querySelectorAll('[data-resumo="total"]');
        const valorTotalFormatado = this.formatarMoeda(total);

        totalElements.forEach(element => {
            if (element) element.textContent = valorTotalFormatado;
        });

        const resumoTotal = document.querySelector('#resumo-total, .resumo-total');
        if (resumoTotal) {
            resumoTotal.textContent = valorTotalFormatado;
        }

        if (valorFrete > 0) {
            this.mostrarLogSucesso(`Frete calculado: ${valorFreteFormatado} | Total: ${valorTotalFormatado}`);
        } else {
            this.mostrarLogInfo('Frete será calculado após inserir endereço completo');
        }

        return total;
    }

    mostrarLog(mensagem, tipo = 'info') {
        if (window.CheckoutManager && window.CheckoutManager.addLog) {
            window.CheckoutManager.addLog(mensagem, tipo);
        } else {
            console.log(`[CARRINHO] ${tipo.toUpperCase()}: ${mensagem}`);
        }
    }

    mostrarLogSucesso(mensagem) {
        this.mostrarLog(mensagem, 'active');
    }

    mostrarLogErro(mensagem) {
        this.mostrarLog(mensagem, 'error');
    }

    mostrarLogAviso(mensagem) {
        this.mostrarLog(mensagem, 'warning');
    }

    mostrarLogInfo(mensagem) {
        this.mostrarLog(mensagem, 'info');
    }

    criarResumoDetalhado() {
        if (!this.carrinhoData || !this.temItens()) return null;

        const resumo = {
            itens: this.obterItens().map(item => ({
                id: item.idProduto,
                nome: item.nome,
                quantidade: item.quantidade,
                precoUnitario: item.precoUnitario,
                subtotal: item.subtotal,
                imagemUrl: item.imagemUrl
            })),
            quantidadeTotal: this.obterQuantidadeTotal(),
            subtotal: this.obterSubtotal(),
            frete: 0,
            total: this.obterSubtotal(),
            timestamp: new Date().toISOString()
        };

        return resumo;
    }

    validarCarrinhoParaCheckout() {
        if (!this.carrinhoData) {
            this.mostrarLogErro('Dados do carrinho não disponíveis');
            return false;
        }

        if (!this.temItens()) {
            this.mostrarLogErro('Carrinho vazio - adicione produtos antes de finalizar');
            return false;
        }

        if (this.obterSubtotal() <= 0) {
            this.mostrarLogErro('Valor do carrinho inválido');
            return false;
        }

        this.mostrarLogSucesso('Carrinho validado para checkout');
        return true;
    }
}

document.addEventListener('DOMContentLoaded', () => {
    if (document.querySelector('.checkout-container')) {
        window.CheckoutCarrinho = new CheckoutCarrinho();
    }
});