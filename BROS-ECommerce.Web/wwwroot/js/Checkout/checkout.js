class CheckoutManager {
    constructor() {
        this.currentStep = 1;
        this.maxStep = 3;
        this.userData = {};
        this.isValid = {
            contato: false,
            endereco: false,
            pagamento: false
        };

        this.init();
    }

    init() {
        this.setupEventListeners();
        this.initializeValidation();
        this.loadUserData();
        this.addLog('Checkout iniciado', 'active');
    }

    setupEventListeners() {
        // Formulário de contato
        document.getElementById('form-contato')?.addEventListener('submit', (e) => {
            e.preventDefault();
            this.handleContatoSubmit();
        });

        // Formulário de endereço
        document.getElementById('form-endereco')?.addEventListener('submit', (e) => {
            e.preventDefault();
            this.handleEnderecoSubmit();
        });

        // Botões de navegação
        document.getElementById('btn-voltar-contato')?.addEventListener('click', () => {
            this.goToStep(1);
        });

        document.getElementById('btn-voltar-endereco')?.addEventListener('click', () => {
            this.goToStep(2);
        });

        document.getElementById('btn-voltar-carrinho-contato')?.addEventListener('click', () => {
            window.location.href = '/Carrinho';
        });

        document.getElementById('btn-voltar-carrinho')?.addEventListener('click', () => {
            window.location.href = '/Carrinho';
        });

        // Buscar CEP
        document.getElementById('btn-buscar-cep')?.addEventListener('click', () => {
            this.buscarCEP();
        });

        // Auto buscar CEP quando completar 8 dígitos
        document.getElementById('cep')?.addEventListener('input', (e) => {
            const cep = e.target.value.replace(/\D/g, '');
            if (cep.length === 8) {
                setTimeout(() => this.buscarCEP(), 500);
            }
        });

        // Validação em tempo real
        this.setupRealTimeValidation();

        // Máscaras
        this.setupMasks();
    }

    setupRealTimeValidation() {
        const inputs = document.querySelectorAll('input[data-validation], select[data-validation]');

        inputs.forEach(input => {
            input.addEventListener('blur', () => this.validateField(input));
            input.addEventListener('input', () => {
                if (input.classList.contains('is-invalid')) {
                    this.validateField(input);
                }
            });
        });
    }

    setupMasks() {
        // Máscara para telefone
        const telefoneInput = document.getElementById('telefone');
        if (telefoneInput) {
            telefoneInput.addEventListener('input', (e) => {
                let value = e.target.value.replace(/\D/g, '');
                if (value.length >= 11) {
                    value = value.replace(/(\d{2})(\d{5})(\d{4})/, '($1) $2-$3');
                } else if (value.length >= 6) {
                    value = value.replace(/(\d{2})(\d{4})(\d{0,4})/, '($1) $2-$3');
                } else if (value.length >= 2) {
                    value = value.replace(/(\d{2})(\d{0,5})/, '($1) $2');
                }
                e.target.value = value;
            });
        }

        // Máscara para CPF
        const cpfInput = document.getElementById('cpf');
        if (cpfInput) {
            cpfInput.addEventListener('input', (e) => {
                let value = e.target.value.replace(/\D/g, '');
                value = value.replace(/(\d{3})(\d{3})(\d{3})(\d{2})/, '$1.$2.$3-$4');
                e.target.value = value;
            });
        }

        // Máscara para CEP
        const cepInput = document.getElementById('cep');
        if (cepInput) {
            cepInput.addEventListener('input', (e) => {
                let value = e.target.value.replace(/\D/g, '');
                value = value.replace(/(\d{5})(\d{3})/, '$1-$2');
                e.target.value = value;
            });
        }
    }

    validateField(input) {
        const validations = input.dataset.validation?.split('|') || [];
        const value = input.value.trim();

        let isValid = true;
        let errorMessage = '';

        for (const validation of validations) {
            const [rule, param] = validation.split(':');

            switch (rule) {
                case 'required':
                    if (!value) {
                        isValid = false;
                        errorMessage = 'Este campo é obrigatório';
                    }
                    break;

                case 'email':
                    if (value && !/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(value)) {
                        isValid = false;
                        errorMessage = 'Digite um e-mail válido';
                    }
                    break;

                case 'phone':
                    if (value && !/^\(\d{2}\) \d{4,5}-\d{4}$/.test(value)) {
                        isValid = false;
                        errorMessage = 'Digite um telefone válido';
                    }
                    break;

                case 'cpf':
                    if (value && !this.validateCPF(value)) {
                        isValid = false;
                        errorMessage = 'CPF inválido';
                    }
                    break;

                case 'cep':
                    if (value && !/^\d{5}-\d{3}$/.test(value)) {
                        isValid = false;
                        errorMessage = 'CEP deve ter o formato 00000-000';
                    }
                    break;

                case 'min':
                    if (value && value.length < parseInt(param)) {
                        isValid = false;
                        errorMessage = `Mínimo de ${param} caracteres`;
                    }
                    break;

                case 'max':
                    if (value && value.length > parseInt(param)) {
                        isValid = false;
                        errorMessage = `Máximo de ${param} caracteres`;
                    }
                    break;
            }

            if (!isValid) break;
        }

        this.updateFieldValidation(input, isValid, errorMessage);
        return isValid;
    }

    updateFieldValidation(input, isValid, errorMessage) {
        const invalidFeedback = input.parentElement.querySelector('.invalid-feedback');

        if (isValid) {
            input.classList.remove('is-invalid');
            input.classList.add('is-valid');
            if (invalidFeedback) invalidFeedback.textContent = '';
        } else {
            input.classList.remove('is-valid');
            input.classList.add('is-invalid');
            if (invalidFeedback) invalidFeedback.textContent = errorMessage;
        }
    }

    validateCPF(cpf) {
        cpf = cpf.replace(/\D/g, '');

        if (cpf.length !== 11 || /^(\d)\1{10}$/.test(cpf)) return false;

        let sum = 0;
        for (let i = 0; i < 9; i++) {
            sum += parseInt(cpf.charAt(i)) * (10 - i);
        }
        let remainder = (sum * 10) % 11;
        if (remainder === 10 || remainder === 11) remainder = 0;
        if (remainder !== parseInt(cpf.charAt(9))) return false;

        sum = 0;
        for (let i = 0; i < 10; i++) {
            sum += parseInt(cpf.charAt(i)) * (11 - i);
        }
        remainder = (sum * 10) % 11;
        if (remainder === 10 || remainder === 11) remainder = 0;
        if (remainder !== parseInt(cpf.charAt(10))) return false;

        return true;
    }

    async loadUserData() {
        try {
            this.showLoading('Carregando dados do usuário...');

            // Simular carregamento dos dados do usuário
            await new Promise(resolve => setTimeout(resolve, 1000));

            const userData = {
                nome: 'João Silva Santos',
                email: 'joao.silva@gmail.com',
                cpf: '12345678901'
            };

            // Preencher campos com dados do usuário
            document.getElementById('nome-completo').value = userData.nome;
            document.getElementById('email').value = userData.email;
            document.getElementById('cpf').value = userData.cpf.replace(/(\d{3})(\d{3})(\d{3})(\d{2})/, '$1.$2.$3-$4');

            // Validar campos preenchidos
            this.validateField(document.getElementById('nome-completo'));
            this.validateField(document.getElementById('email'));
            this.validateField(document.getElementById('cpf'));

            this.hideLoading();
            this.addLog('Dados do usuário carregados', 'active');

        } catch (error) {
            this.hideLoading();
            this.addLog('Erro ao carregar dados do usuário', 'error');
            console.error('Erro ao carregar dados do usuário:', error);
        }
    }

    validateForm(formId) {
        const form = document.getElementById(formId);
        if (!form) return false;

        const inputs = form.querySelectorAll('input[data-validation], select[data-validation]');
        let isValid = true;

        inputs.forEach(input => {
            if (!this.validateField(input)) {
                isValid = false;
            }
        });

        return isValid;
    }

    handleContatoSubmit() {
        if (!this.validateForm('form-contato')) {
            this.addLog('Corrija os erros no formulário de contato', 'error');
            return;
        }

        const formData = new FormData(document.getElementById('form-contato'));
        this.userData.contato = Object.fromEntries(formData);

        this.addLog('Dados de contato validados', 'active');
        this.isValid.contato = true;
        this.goToStep(2);
    }

    handleEnderecoSubmit() {
        if (!this.validateForm('form-endereco')) {
            this.addLog('Corrija os erros no formulário de endereço', 'error');
            return;
        }

        const formData = new FormData(document.getElementById('form-endereco'));
        this.userData.endereco = Object.fromEntries(formData);

        this.updateEnderecoPreview();
        this.addLog('Endereço de entrega confirmado', 'active');
        this.isValid.endereco = true;
        this.goToStep(3);
    }

    updateEnderecoPreview() {
        const endereco = this.userData.endereco;
        if (!endereco) return;

        const enderecoCompleto = `${endereco.logradouro}, ${endereco.numero}${endereco.complemento ? `, ${endereco.complemento}` : ''} - ${endereco.bairro}, ${endereco.cidade}/${endereco.estado}`;

        document.getElementById('endereco-completo').textContent = enderecoCompleto;
        document.getElementById('endereco-cep').textContent = `CEP: ${endereco.cep}`;
        document.getElementById('endereco-preview').style.display = 'block';
    }

    async buscarCEP() {
        const cepInput = document.getElementById('cep');
        const cep = cepInput.value.replace(/\D/g, '');

        if (cep.length !== 8) {
            this.addLog('CEP deve ter 8 dígitos', 'error');
            return;
        }

        try {
            this.showLoading('Buscando endereço...');
            this.addLog('Consultando CEP...', 'processing');

            const response = await fetch(`https://viacep.com.br/ws/${cep}/json/`);
            const data = await response.json();

            if (data.erro) {
                throw new Error('CEP não encontrado');
            }

            // Preencher campos com dados do CEP
            document.getElementById('logradouro').value = data.logradouro || '';
            document.getElementById('bairro').value = data.bairro || '';
            document.getElementById('cidade').value = data.localidade || '';
            document.getElementById('estado').value = data.uf || '';

            // Validar campos preenchidos
            this.validateField(document.getElementById('logradouro'));
            this.validateField(document.getElementById('bairro'));
            this.validateField(document.getElementById('cidade'));
            this.validateField(document.getElementById('estado'));

            this.hideLoading();
            this.addLog('Endereço encontrado com sucesso', 'active');

            // Focar no campo número
            document.getElementById('numero').focus();

        } catch (error) {
            this.hideLoading();
            this.addLog('Erro ao buscar CEP', 'error');
            console.error('Erro ao buscar CEP:', error);
        }
    }

    goToStep(step) {
        if (step < 1 || step > this.maxStep) return;

        // Validar passos anteriores
        if (step > 1 && !this.isValid.contato) {
            this.addLog('Complete os dados de contato primeiro', 'error');
            return;
        }

        if (step > 2 && !this.isValid.endereco) {
            this.addLog('Complete o endereço de entrega primeiro', 'error');
            return;
        }

        this.currentStep = step;
        this.updateUI();
        this.addLog(`Navegando para: ${this.getStepName(step)}`, 'active');
    }

    getStepName(step) {
        const names = {
            1: 'Dados de Contato',
            2: 'Endereço de Entrega',
            3: 'Forma de Pagamento'
        };
        return names[step] || 'Passo desconhecido';
    }

    updateUI() {
        // Atualizar progress steps
        document.querySelectorAll('.progress-step').forEach((step, index) => {
            const stepNumber = index + 1;
            step.classList.remove('active', 'completed');

            if (stepNumber === this.currentStep) {
                step.classList.add('active');
            } else if (stepNumber < this.currentStep) {
                step.classList.add('completed');
            }
        });

        // Atualizar conteúdo das abas
        document.querySelectorAll('.tab-content').forEach(tab => {
            tab.classList.remove('active');
        });

        const activeTab = document.getElementById(`tab-${this.getTabName(this.currentStep)}`);
        if (activeTab) {
            activeTab.classList.add('active');
        }
    }

    getTabName(step) {
        const names = {
            1: 'contato',
            2: 'endereco',
            3: 'pagamento'
        };
        return names[step] || 'contato';
    }

    addLog(message, type = 'active') {
        const logsContainer = document.getElementById('logs-container');
        if (!logsContainer) return;

        const now = new Date();
        const timeString = now.toLocaleTimeString('pt-BR', {
            hour: '2-digit',
            minute: '2-digit'
        });

        const logItem = document.createElement('div');
        logItem.className = `log-item ${type}`;

        const iconClass = type === 'error' ? 'fa-circle-exclamation' :
            type === 'processing' ? 'fa-spinner fa-spin' :
                'fa-circle-check';

        logItem.innerHTML = `
            <div class="log-icon">
                <i class="fa-solid ${iconClass}"></i>
            </div>
            <div class="log-content">
                <span class="log-title">${message}</span>
                <span class="log-time">${timeString}</span>
            </div>
        `;

        // Remover logs antigos (manter apenas os últimos 5)
        const existingLogs = logsContainer.querySelectorAll('.log-item');
        if (existingLogs.length >= 5) {
            existingLogs[0].remove();
        }

        logsContainer.appendChild(logItem);

        // Scroll para o último log
        logItem.scrollIntoView({ behavior: 'smooth', block: 'nearest' });
    }

    showLoading(message = 'Carregando...') {
        const overlay = document.createElement('div');
        overlay.id = 'loading-overlay';
        overlay.className = 'loading-overlay';
        overlay.innerHTML = `
            <div class="loading-content">
                <div class="loading-spinner">
                    <i class="fa-solid fa-spinner fa-spin"></i>
                </div>
                <div class="loading-text">${message}</div>
                <div class="loading-subtext">Aguarde um momento...</div>
            </div>
        `;

        document.body.appendChild(overlay);
    }

    hideLoading() {
        const overlay = document.getElementById('loading-overlay');
        if (overlay) {
            overlay.remove();
        }
    }

    initializeValidation() {
        // Configurar validação do formulário final
        const finalForm = document.getElementById('form-finalizar-pedido');
        if (finalForm) {
            finalForm.addEventListener('submit', (e) => {
                const selectedPayment = document.querySelector('input[name="metodo"]:checked');
                if (!selectedPayment) {
                    e.preventDefault();
                    this.addLog('Selecione uma forma de pagamento', 'error');
                    return;
                }

                // Adicionar dados aos campos ocultos
                document.getElementById('dados-contato').value = JSON.stringify(this.userData.contato);
                document.getElementById('dados-endereco').value = JSON.stringify(this.userData.endereco);

                this.addLog('Finalizando pedido...', 'processing');
                this.showLoading('Processando seu pedido...');
            });
        }
    }
}

// Inicializar o checkout quando a página carregar
document.addEventListener('DOMContentLoaded', () => {
    new CheckoutManager();
});