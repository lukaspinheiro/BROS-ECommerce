class CheckoutForms {
    constructor() {
        this.currentTab = 'contato';
        this.contatoData = {};
        this.enderecoData = {};
        this.isContatoValidated = false;
        this.isEnderecoValidated = false;

        this.init();
    }

    init() {
        this.setupEventListeners();
        this.loadUserData();
        this.setupMasks();
        this.updateTabStates();
    }

    setupEventListeners() {
        const btnContatoProximo = document.getElementById('btn-contato-proximo');
        const btnEnderecoVoltar = document.getElementById('btn-endereco-voltar');
        const btnEnderecoFinalizar = document.getElementById('btn-endereco-finalizar');
        const cepInput = document.getElementById('cep');

        if (btnContatoProximo) {
            btnContatoProximo.addEventListener('click', () => this.nextToEndereco());
        }

        if (btnEnderecoVoltar) {
            btnEnderecoVoltar.addEventListener('click', () => this.backToContato());
        }

        if (btnEnderecoFinalizar) {
            btnEnderecoFinalizar.addEventListener('click', () => this.finalizarDados());
        }

        if (cepInput) {
            cepInput.addEventListener('blur', () => this.buscarCep());
        }

        document.querySelectorAll('#form-contato input').forEach(input => {
            input.addEventListener('blur', () => this.validateContatoField(input));
            input.addEventListener('input', () => this.clearFieldError(input));
        });

        document.querySelectorAll('#form-endereco input, #form-endereco select').forEach(input => {
            input.addEventListener('blur', () => this.validateEnderecoField(input));
            input.addEventListener('input', () => this.clearFieldError(input));
        });
    }

    async loadUserData() {
        try {
            const response = await fetch('/api/usuario/dados-checkout');
            if (response.ok) {
                const userData = await response.json();
                this.preencherDadosUsuario(userData);
            }
        } catch (error) {
            console.warn('Não foi possível carregar dados do usuário:', error);
        }
    }

    preencherDadosUsuario(userData) {
        if (userData.nome) {
            document.getElementById('nomeCompleto').value = userData.nome;
        }
        if (userData.email) {
            document.getElementById('email').value = userData.email;
        }
        if (userData.cpf) {
            document.getElementById('cpf').value = userData.cpf;
        }
        if (userData.estado) {
            const estadoSelect = document.getElementById('estado');
            if (estadoSelect) {
                estadoSelect.value = userData.estado;
            }
        }
    }

    setupMasks() {
        this.maskTelefone();
        this.maskCep();
        this.maskCpf();
    }

    maskTelefone() {
        const telefoneInput = document.getElementById('telefone');
        if (telefoneInput) {
            telefoneInput.addEventListener('input', (e) => {
                let value = e.target.value.replace(/\D/g, '');
                if (value.length <= 11) {
                    value = value.replace(/^(\d{2})(\d{4,5})(\d{4})$/, '($1) $2-$3');
                }
                e.target.value = value;
            });
        }
    }

    maskCep() {
        const cepInput = document.getElementById('cep');
        if (cepInput) {
            cepInput.addEventListener('input', (e) => {
                let value = e.target.value.replace(/\D/g, '');
                if (value.length <= 8) {
                    value = value.replace(/^(\d{5})(\d{3})$/, '$1-$2');
                }
                e.target.value = value;
            });
        }
    }

    maskCpf() {
        const cpfInput = document.getElementById('cpf');
        if (cpfInput && !cpfInput.readOnly) {
            cpfInput.addEventListener('input', (e) => {
                let value = e.target.value.replace(/\D/g, '');
                if (value.length <= 11) {
                    value = value.replace(/^(\d{3})(\d{3})(\d{3})(\d{2})$/, '$1.$2.$3-$4');
                }
                e.target.value = value;
            });
        }
    }

    async buscarCep() {
        const cepInput = document.getElementById('cep');
        const cep = cepInput.value.replace(/\D/g, '');

        if (cep.length !== 8) return;

        this.showLoading();

        try {
            const response = await fetch(`https://viacep.com.br/ws/${cep}/json/`);
            const data = await response.json();

            if (!data.erro) {
                document.getElementById('logradouro').value = data.logradouro || '';
                document.getElementById('bairro').value = data.bairro || '';
                document.getElementById('cidade').value = data.localidade || '';
                document.getElementById('estado').value = data.uf || '';

                document.getElementById('numero').focus();
            } else {
                this.showFieldError(cepInput, 'CEP não encontrado');
            }
        } catch (error) {
            this.showFieldError(cepInput, 'Erro ao buscar CEP');
        } finally {
            this.hideLoading();
        }
    }

    validateContatoField(field) {
        const value = field.value.trim();
        let isValid = true;
        let errorMessage = '';

        switch (field.id) {
            case 'nomeCompleto':
                if (!value) {
                    isValid = false;
                    errorMessage = 'Nome completo é obrigatório';
                } else if (value.length < 3) {
                    isValid = false;
                    errorMessage = 'Nome deve ter pelo menos 3 caracteres';
                }
                break;

            case 'email':
                const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
                if (!value) {
                    isValid = false;
                    errorMessage = 'E-mail é obrigatório';
                } else if (!emailRegex.test(value)) {
                    isValid = false;
                    errorMessage = 'E-mail inválido';
                }
                break;

            case 'telefone':
                const telefoneRegex = /^\(\d{2}\)\s\d{4,5}-\d{4}$/;
                if (!value) {
                    isValid = false;
                    errorMessage = 'Telefone é obrigatório';
                } else if (!telefoneRegex.test(value)) {
                    isValid = false;
                    errorMessage = 'Formato inválido. Use: (11) 99999-9999';
                }
                break;

            case 'cpf':
                const cpfRegex = /^\d{3}\.\d{3}\.\d{3}-\d{2}$/;
                if (!value) {
                    isValid = false;
                    errorMessage = 'CPF é obrigatório';
                } else if (!cpfRegex.test(value)) {
                    isValid = false;
                    errorMessage = 'Formato inválido. Use: 999.999.999-99';
                }
                break;
        }

        if (isValid) {
            this.showFieldSuccess(field);
        } else {
            this.showFieldError(field, errorMessage);
        }

        return isValid;
    }

    validateEnderecoField(field) {
        const value = field.value.trim();
        let isValid = true;
        let errorMessage = '';

        switch (field.id) {
            case 'cep':
                const cepRegex = /^\d{5}-\d{3}$/;
                if (!value) {
                    isValid = false;
                    errorMessage = 'CEP é obrigatório';
                } else if (!cepRegex.test(value)) {
                    isValid = false;
                    errorMessage = 'Formato inválido. Use: 99999-999';
                }
                break;

            case 'logradouro':
                if (!value) {
                    isValid = false;
                    errorMessage = 'Logradouro é obrigatório';
                }
                break;

            case 'numero':
                if (!value) {
                    isValid = false;
                    errorMessage = 'Número é obrigatório';
                }
                break;

            case 'bairro':
                if (!value) {
                    isValid = false;
                    errorMessage = 'Bairro é obrigatório';
                }
                break;

            case 'cidade':
                if (!value) {
                    isValid = false;
                    errorMessage = 'Cidade é obrigatória';
                }
                break;

            case 'estado':
                if (!value) {
                    isValid = false;
                    errorMessage = 'Estado é obrigatório';
                }
                break;
        }

        if (isValid) {
            this.showFieldSuccess(field);
        } else {
            this.showFieldError(field, errorMessage);
        }

        return isValid;
    }

    showFieldSuccess(field) {
        field.classList.remove('is-invalid');
        field.classList.add('is-valid');
        const feedback = field.nextElementSibling;
        if (feedback && feedback.classList.contains('invalid-feedback')) {
            feedback.textContent = '';
        }
    }

    showFieldError(field, message) {
        field.classList.remove('is-valid');
        field.classList.add('is-invalid');
        const feedback = field.nextElementSibling;
        if (feedback && feedback.classList.contains('invalid-feedback')) {
            feedback.textContent = message;
        }
    }

    clearFieldError(field) {
        field.classList.remove('is-invalid', 'is-valid');
        const feedback = field.nextElementSibling;
        if (feedback && feedback.classList.contains('invalid-feedback')) {
            feedback.textContent = '';
        }
    }

    validateAllContatoFields() {
        const fields = document.querySelectorAll('#form-contato input[required]');
        let allValid = true;

        fields.forEach(field => {
            if (!this.validateContatoField(field)) {
                allValid = false;
            }
        });

        return allValid;
    }

    validateAllEnderecoFields() {
        const fields = document.querySelectorAll('#form-endereco input[required], #form-endereco select[required]');
        let allValid = true;

        fields.forEach(field => {
            if (!this.validateEnderecoField(field)) {
                allValid = false;
            }
        });

        return allValid;
    }

    nextToEndereco() {
        if (this.validateAllContatoFields()) {
            this.collectContatoData();
            this.switchTab('endereco');
            this.isContatoValidated = true;
            this.updateTabStates();
            this.updateFinalizarButton();
        }
    }

    backToContato() {
        this.switchTab('contato');
        this.updateTabStates();
    }

    finalizarDados() {
        if (this.validateAllEnderecoFields()) {
            this.collectEnderecoData();
            this.isEnderecoValidated = true;
            this.updateTabStates();
            this.updateFinalizarButton();
            this.showSuccessMessage();
        }
    }

    switchTab(tabName) {
        document.querySelectorAll('.tab-content').forEach(content => {
            content.classList.remove('active');
        });

        document.querySelectorAll('.tab-btn').forEach(btn => {
            btn.classList.remove('active');
        });

        document.getElementById(`tab-${tabName}`).classList.add('active');
        document.querySelector(`[data-tab="${tabName}"]`).classList.add('active');

        this.currentTab = tabName;
    }

    updateTabStates() {
        const contatoTab = document.querySelector('[data-tab="contato"]');
        const enderecoTab = document.querySelector('[data-tab="endereco"]');

        if (this.isContatoValidated) {
            contatoTab.classList.add('completed');
            enderecoTab.classList.remove('disabled');
        }

        if (this.isEnderecoValidated) {
            enderecoTab.classList.add('completed');
        }
    }

    updateFinalizarButton() {
        const btnFinalizar = document.getElementById('btn-finalizar');
        if (btnFinalizar) {
            btnFinalizar.disabled = !(this.isContatoValidated && this.isEnderecoValidated);
        }
    }

    collectContatoData() {
        this.contatoData = {
            nomeCompleto: document.getElementById('nomeCompleto').value,
            email: document.getElementById('email').value,
            telefone: document.getElementById('telefone').value,
            cpf: document.getElementById('cpf').value
        };

        document.getElementById('contato-data').value = JSON.stringify(this.contatoData);
    }

    collectEnderecoData() {
        this.enderecoData = {
            cep: document.getElementById('cep').value,
            logradouro: document.getElementById('logradouro').value,
            numero: document.getElementById('numero').value,
            complemento: document.getElementById('complemento').value,
            bairro: document.getElementById('bairro').value,
            cidade: document.getElementById('cidade').value,
            estado: document.getElementById('estado').value
        };

        document.getElementById('endereco-data').value = JSON.stringify(this.enderecoData);
    }

    showLoading() {
        let overlay = document.querySelector('.loading-overlay');
        if (!overlay) {
            overlay = document.createElement('div');
            overlay.className = 'loading-overlay';
            overlay.innerHTML = '<div class="loading-spinner"></div>';
            document.querySelector('.form-checkout').appendChild(overlay);
        }
        overlay.style.display = 'flex';
    }

    hideLoading() {
        const overlay = document.querySelector('.loading-overlay');
        if (overlay) {
            overlay.style.display = 'none';
        }
    }

    showSuccessMessage() {
        const message = document.createElement('div');
        message.className = 'alert alert-success alert-dismissible fade show';
        message.innerHTML = `
            <i class="fas fa-check-circle"></i> 
            Dados de entrega preenchidos com sucesso! 
            Agora selecione a forma de pagamento.
            <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
        `;

        const container = document.querySelector('.checkout-container');
        container.insertBefore(message, container.firstChild);

        setTimeout(() => {
            message.remove();
        }, 5000);
    }

    isContatoComplete() {
        return this.isContatoValidated;
    }

    isEnderecoComplete() {
        return this.isEnderecoValidated;
    }
}

document.addEventListener('DOMContentLoaded', () => {
    window.CheckoutForms = new CheckoutForms();
});