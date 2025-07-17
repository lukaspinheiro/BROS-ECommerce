class FreteCalculator {
    constructor() {
        this.freteRules = {
            mesmaRegiao: { prefixo: '76', valor: 5.00, descricao: 'Mesma região' },
            mesmoEstado: { estado: 'RO', valor: 10.00, descricao: 'Mesmo estado' },
            outroEstado: { valor: 20.00, descricao: 'Outro estado' }
        };

        this.init();
    }

    init() {
        this.setupFreteWatcher();
    }

    setupFreteWatcher() {
        const cepInput = document.getElementById('cep');
        const estadoSelect = document.getElementById('estado');

        if (cepInput) {
            cepInput.addEventListener('blur', () => this.calculateFrete());
            cepInput.addEventListener('input', () => {
                const cep = cepInput.value.replace(/\D/g, '');
                if (cep.length === 8) {
                    setTimeout(() => this.calculateFrete(), 1000);
                }
            });
        }

        if (estadoSelect) {
            estadoSelect.addEventListener('change', () => this.calculateFrete());
        }
    }

    calculateFrete() {
        const cep = document.getElementById('cep')?.value.replace(/\D/g, '');
        const estado = document.getElementById('estado')?.value;

        if (!cep || cep.length !== 8) {
            this.updateFreteDisplay('A calcular', 0);
            return;
        }

        const cepPrefix = cep.substring(0, 2);
        let freteValue = 0;
        let freteDescription = '';

        if (cepPrefix === this.freteRules.mesmaRegiao.prefixo) {
            freteValue = this.freteRules.mesmaRegiao.valor;
            freteDescription = this.freteRules.mesmaRegiao.descricao;
        } else if (estado === this.freteRules.mesmoEstado.estado) {
            freteValue = this.freteRules.mesmoEstado.valor;
            freteDescription = this.freteRules.mesmoEstado.descricao;
        } else if (estado && estado !== this.freteRules.mesmoEstado.estado) {
            freteValue = this.freteRules.outroEstado.valor;
            freteDescription = this.freteRules.outroEstado.descricao;
        }

        this.updateFreteDisplay(freteDescription, freteValue);
        this.updateTotalDisplay(freteValue);

        if (window.checkoutManager) {
            window.checkoutManager.addLog(`Frete calculado: R$ ${freteValue.toFixed(2)} (${freteDescription})`, 'active');
        }
    }

    updateFreteDisplay(description, value) {
        const freteElement = document.querySelector('.summary-item:nth-child(2) .d-flex span:last-child');
        if (freteElement) {
            freteElement.textContent = value > 0 ? `R$ ${value.toFixed(2)}` : description;
        }
    }

    updateTotalDisplay(freteValue) {
        const subtotalElement = document.querySelector('.summary-item:first-child .d-flex span:last-child');
        const totalElement = document.querySelector('.summary-total .d-flex span:last-child');

        if (subtotalElement && totalElement) {
            const subtotalText = subtotalElement.textContent.replace('R$ ', '').replace(',', '.');
            const subtotal = parseFloat(subtotalText) || 400.00;

            const total = subtotal + freteValue;
            totalElement.textContent = `R$ ${total.toFixed(2).replace('.', ',')}`;
        }
    }
}

document.addEventListener('DOMContentLoaded', () => {
    new FreteCalculator();
});