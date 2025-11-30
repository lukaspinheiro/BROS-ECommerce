class FreteCalculator {
    constructor() {
        this.freteRules = {
            mesmaRegiao: { prefixo: '76', valor: 5.00, descricao: 'Entrega local' },
            mesmoEstado: { estado: 'RO', valor: 10.00, descricao: 'Entrega estadual' },
            outroEstado: { valor: 20.00, descricao: 'Entrega nacional' }
        };

        this.valorFreteAtual = 0;
        this.init();
    }

    init() {
        this.setupFreteWatcher();
    }

    setupFreteWatcher() {
        const cepInput = document.getElementById('cep');
        const estadoSelect = document.getElementById('estado');

        if (cepInput) {
            cepInput.addEventListener('blur', () => {
                const cep = cepInput.value.replace(/\D/g, '');
                if (cep.length === 8) {
                    setTimeout(() => this.calculateFrete(), 500);
                }
            });
        }

        if (estadoSelect) {
            estadoSelect.addEventListener('change', () => {
                setTimeout(() => this.calculateFrete(), 200);
            });
        }

        const observer = new MutationObserver(() => {
            setTimeout(() => this.calculateFrete(), 300);
        });

        if (estadoSelect) {
            observer.observe(estadoSelect, {
                attributes: true,
                attributeFilter: ['value']
            });
        }
    }

    calculateFrete() {
        const cep = document.getElementById('cep')?.value.replace(/\D/g, '');
        const estado = document.getElementById('estado')?.value;

        if (!cep || cep.length !== 8 || !estado) {
            this.resetFrete();
            return;
        }

        const cepPrefix = cep.substring(0, 2);
        let freteValue = 0;
        let freteDescription = '';

        //if (cepPrefix === this.freteRules.mesmaRegiao.prefixo) {
        //    freteValue = this.freteRules.mesmaRegiao.valor;
        //    freteDescription = this.freteRules.mesmaRegiao.descricao;
        //} else if (estado === this.freteRules.mesmoEstado.estado) {
        //    freteValue = this.freteRules.mesmoEstado.valor;
        //    freteDescription = this.freteRules.mesmoEstado.descricao;
        //} else {
        //    freteValue = this.freteRules.outroEstado.valor;
        //    freteDescription = this.freteRules.outroEstado.descricao;
        //}

        this.valorFreteAtual = freteValue;
        this.updateFreteDisplay(freteValue, freteDescription);
    }

    updateFreteDisplay(freteValue, description) {
        if (window.CheckoutCarrinho) {
            window.CheckoutCarrinho.calcularTotalComFrete(freteValue);
        } else {
            this.fallbackUpdateDisplay(freteValue, description);
        }

        if (window.CheckoutManager) {
            const valorFormatado = new Intl.NumberFormat('pt-BR', {
                style: 'currency',
                currency: 'BRL'
            }).format(freteValue);

            window.CheckoutManager.addLog(
                `Frete calculado: ${valorFormatado} (${description})`,
                'active'
            );
        }
    }

    fallbackUpdateDisplay(freteValue, description) {
        const freteElements = document.querySelectorAll('[data-resumo="frete"], .resumo-frete');
        const valorFreteFormatado = new Intl.NumberFormat('pt-BR', {
            style: 'currency',
            currency: 'BRL'
        }).format(freteValue);

        freteElements.forEach(element => {
            if (element) element.textContent = valorFreteFormatado;
        });

        const subtotalText = document.querySelector('[data-resumo="subtotal"], .resumo-subtotal')?.textContent || 'R$ 0,00';
        const subtotalValue = this.parseMoneyString(subtotalText);
        const novoTotal = subtotalValue + freteValue;

        const totalElements = document.querySelectorAll('[data-resumo="total"], .resumo-total');
        const valorTotalFormatado = new Intl.NumberFormat('pt-BR', {
            style: 'currency',
            currency: 'BRL'
        }).format(novoTotal);

        totalElements.forEach(element => {
            if (element) element.textContent = valorTotalFormatado;
        });
    }

    parseMoneyString(moneyStr) {
        return parseFloat(
            moneyStr.replace(/[R$\s]/g, '')
                .replace(/\./g, '')
                .replace(',', '.')
        ) || 0;
    }

    resetFrete() {
        this.valorFreteAtual = 0;

        if (window.CheckoutCarrinho) {
            window.CheckoutCarrinho.calcularTotalComFrete(0);
        } else {
            const freteElements = document.querySelectorAll('[data-resumo="frete"], .resumo-frete');
            freteElements.forEach(element => {
                if (element) element.textContent = 'A calcular';
            });

            const subtotalText = document.querySelector('[data-resumo="subtotal"], .resumo-subtotal')?.textContent || 'R$ 0,00';
            const totalElements = document.querySelectorAll('[data-resumo="total"], .resumo-total');

            totalElements.forEach(element => {
                if (element) element.textContent = subtotalText;
            });
        }
    }

    getFreteAtual() {
        return this.valorFreteAtual;
    }

    recalcularFrete() {
        this.calculateFrete();
    }
}

document.addEventListener('DOMContentLoaded', () => {
    if (document.querySelector('.checkout-container')) {
        window.FreteCalculator = new FreteCalculator();
    }
});