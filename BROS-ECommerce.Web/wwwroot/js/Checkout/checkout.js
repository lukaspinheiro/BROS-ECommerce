class CheckoutManager {
    constructor() {
        this.currentStep = 1;
        this.maxStep = 3;
        this.userData = {
            contato: {},
            endereco: {}
        };
        this.isValid = {
            contato: false,
            endereco: false
        };

        this.init();
    }

    init() {
        this.initializeElements();
        this.initializeValidation();
        this.setupEventListeners();
        this.setupMasks();
        this.updateUI();
        this.addLog('Preencha as informações de Contato', 'info');
    }

    initializeElements() {
        this.progressSteps = document.querySelectorAll('.progress-step');
        this.tabContents = document.querySelectorAll('.tab-content');
        this.logsContainer = document.querySelector('.status-logs');
    }

    setupEventListeners() {
        const btnContinuarEndereco = document.getElementById('btn-continuar-endereco');
        if (btnContinuarEndereco) {
            btnContinuarEndereco.addEventListener('click', (e) => {
                e.preventDefault();
                this.handleContatoSubmit();
            });
        }

        const btnContinuarPagamento = document.getElementById('btn-continuar-pagamento');
        if (btnContinuarPagamento) {
            btnContinuarPagamento.addEventListener('click', (e) => {
                e.preventDefault();
                this.handleEnderecoSubmit();
            });
        }

        const btnVoltarContato = document.getElementById('btn-voltar-contato');
        if (btnVoltarContato) {
            btnVoltarContato.addEventListener('click', () => {
                this.goToStep(1);
            });
        }

        const btnVoltarEndereco = document.getElementById('btn-voltar-endereco');
        if (btnVoltarEndereco) {
            btnVoltarEndereco.addEventListener('click', () => {
                this.goToStep(2);
            });
        }

        const btnVoltarCarrinhoContato = document.getElementById('btn-voltar-carrinho-contato');
        if (btnVoltarCarrinhoContato) {
            btnVoltarCarrinhoContato.addEventListener('click', () => {
                window.location.href = '/Carrinho';
            });
        }

        const btnVoltarCarrinho = document.getElementById('btn-voltar-carrinho');
        if (btnVoltarCarrinho) {
            btnVoltarCarrinho.addEventListener('click', () => {
                window.location.href = '/Carrinho';
            });
        }

        const cepInput = document.getElementById('cep');
        if (cepInput) {
            cepInput.addEventListener('blur', () => {
                if (cepInput.value.replace(/\D/g, '').length === 8) {
                    this.buscarCEP();
                }
            });
        }

        document.querySelectorAll('.progress-step').forEach(step => {
            step.addEventListener('click', (e) => {
                const stepNumber = parseInt(e.currentTarget.dataset.step);
                this.goToStep(stepNumber);
            });
        });
    }

    setupMasks() {
        const telefoneInput = document.getElementById('telefone');
        if (telefoneInput) {
            telefoneInput.addEventListener('input', (e) => {
                let value = e.target.value.replace(/\D/g, '');

                if (value.length <= 11) {
                    if (value.length >= 11) {
                        value = value.replace(/(\d{2})(\d{5})(\d{4})/, '($1) $2-$3');
                    } else if (value.length >= 7) {
                        value = value.replace(/(\d{2})(\d{4})(\d{0,4})/, '($1) $2-$3');
                    } else if (value.length >= 3) {
                        value = value.replace(/(\d{2})(\d{0,5})/, '($1) $2');
                    } else if (value.length >= 1) {
                        value = value.replace(/(\d{0,2})/, '($1');
                    }
                }

                e.target.value = value;
            });
        }

        const cepInput = document.getElementById('cep');
        if (cepInput) {
            cepInput.addEventListener('input', (e) => {
                let value = e.target.value.replace(/\D/g, '');
                if (value.length <= 8) {
                    if (value.length > 5) {
                        value = value.replace(/(\d{5})(\d{0,3})/, '$1-$2');
                    }
                }
                e.target.value = value;
            });
        }

        const cpfInput = document.getElementById('cpf');
        if (cpfInput) {
            cpfInput.addEventListener('input', (e) => {
                let value = e.target.value.replace(/\D/g, '');
                if (value.length <= 11) {
                    value = value.replace(/(\d{3})(\d{3})(\d{3})(\d{2})/, '$1.$2.$3-$4');
                }
                e.target.value = value;
            });
        }
    }

    validateField(input) {
        if (!input) return false;

        const validationRules = input.dataset.validation;
        if (!validationRules) return true;

        const rules = validationRules.split('|');
        const value = input.value.trim();

        for (const rule of rules) {
            if (rule === 'required' && !value) {
                this.showFieldError(input, 'Este campo é obrigatório');
                this.addLogTemporario('Erro no campo ' + this.obterNomeCampo(input.id), 'error');
                return false;
            }

            if (rule.startsWith('min:')) {
                const minLength = parseInt(rule.split(':')[1]);
                if (value.length < minLength) {
                    this.showFieldError(input, `Mínimo ${minLength} caracteres`);
                    this.addLogTemporario('Erro no campo ' + this.obterNomeCampo(input.id), 'error');
                    return false;
                }
            }

            if (rule.startsWith('max:')) {
                const maxLength = parseInt(rule.split(':')[1]);
                if (value.length > maxLength) {
                    this.showFieldError(input, `Máximo ${maxLength} caracteres`);
                    this.addLogTemporario('Erro no campo ' + this.obterNomeCampo(input.id), 'error');
                    return false;
                }
            }

            if (rule === 'email') {
                const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
                if (!emailRegex.test(value)) {
                    this.showFieldError(input, 'E-mail inválido');
                    this.addLogTemporario('Erro no campo E-mail', 'error');
                    return false;
                }
            }

            if (rule === 'cpf') {
                if (!this.validarCPF(value)) {
                    this.showFieldError(input, 'CPF inválido');
                    this.addLogTemporario('Erro no campo CPF', 'error');
                    return false;
                }
            }

            if (rule === 'phone') {
                const phoneRegex = /^\(\d{2}\)\s\d{4,5}-\d{4}$/;
                if (!phoneRegex.test(value)) {
                    this.showFieldError(input, 'Telefone inválido');
                    this.addLogTemporario('Erro no campo Telefone', 'error');
                    return false;
                }
            }
        }

        this.showFieldSuccess(input);
        return true;
    }

    obterNomeCampo(campoId) {
        const nomes = {
            'nome-completo': 'Nome',
            'email': 'E-mail',
            'telefone': 'Telefone',
            'cpf': 'CPF',
            'cep': 'CEP',
            'logradouro': 'Logradouro',
            'numero': 'Número',
            'bairro': 'Bairro',
            'cidade': 'Cidade',
            'estado': 'Estado'
        };
        return nomes[campoId] || campoId;
    }

    validarCPF(cpf) {
        cpf = cpf.replace(/\D/g, '');

        if (cpf.length !== 11 || /^(\d)\1+$/.test(cpf)) {
            return false;
        }

        let soma = 0;
        for (let i = 0; i < 9; i++) {
            soma += parseInt(cpf.charAt(i)) * (10 - i);
        }
        let resto = 11 - (soma % 11);
        if (resto === 10 || resto === 11) resto = 0;
        if (resto !== parseInt(cpf.charAt(9))) return false;

        soma = 0;
        for (let i = 0; i < 10; i++) {
            soma += parseInt(cpf.charAt(i)) * (11 - i);
        }
        resto = 11 - (soma % 11);
        if (resto === 10 || resto === 11) resto = 0;
        return resto === parseInt(cpf.charAt(10));
    }

    showFieldError(input, message) {
        input.classList.remove('is-valid');
        input.classList.add('is-invalid');

        const feedback = input.parentNode.querySelector('.invalid-feedback');
        if (feedback) {
            feedback.textContent = message;
        }
    }

    showFieldSuccess(input) {
        input.classList.remove('is-invalid');
        input.classList.add('is-valid');
    }

    addLog(message, type = 'info', temporary = false) {
        if (!this.logsContainer) return;

        const timeString = new Date().toLocaleTimeString('pt-BR', {
            hour: '2-digit',
            minute: '2-digit'
        });

        const logItem = document.createElement('div');
        logItem.className = `log-item ${type}`;

        const iconClass =
            type === 'error' ? 'fa-circle-exclamation' :
                type === 'active' ? 'fa-circle-check' :
                    type === 'processing' ? 'fa-spinner fa-spin' :
                        'fa-circle-info';

        logItem.innerHTML = `
            <div class="log-icon">
                <i class="fa-solid ${iconClass}"></i>
            </div>
            <div class="log-content">
                <span class="log-title">${message}</span>
                <span class="log-time">${timeString}</span>
            </div>
        `;

        const existingLogs = this.logsContainer.querySelectorAll('.log-item');
        if (existingLogs.length >= 3) {
            existingLogs[0].remove();
        }

        this.logsContainer.appendChild(logItem);
        logItem.scrollIntoView({ behavior: 'smooth', block: 'nearest' });

        if (temporary) {
            setTimeout(() => {
                if (logItem.parentNode) {
                    logItem.remove();
                }
            }, 4000);
        }
    }

    addLogTemporario(message, type = 'info') {
        this.addLog(message, type, true);
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
            this.addLogTemporario('Preencha todos os dados de Contato', 'error');
            return;
        }

        if (window.CheckoutCarrinho && !window.CheckoutCarrinho.validarCarrinhoParaCheckout()) {
            this.addLogTemporario('Verifique os dados do carrinho antes de continuar', 'error');
            return;
        }

        const formData = new FormData(document.getElementById('form-contato'));
        this.userData.contato = Object.fromEntries(formData);

        this.addLogTemporario('Dados de Contato preenchidos', 'active');
        this.isValid.contato = true;
        this.goToStep(2);
    }

    handleEnderecoSubmit() {
        if (!this.validateForm('form-endereco')) {
            this.addLogTemporario('Preencha todos os dados de Endereço', 'error');
            return;
        }

        const formData = new FormData(document.getElementById('form-endereco'));
        this.userData.endereco = Object.fromEntries(formData);

        this.updateEnderecoPreview();
        this.addLogTemporario('Dados de Endereço preenchidos', 'active');
        this.isValid.endereco = true;
        this.goToStep(3);
    }

    updateEnderecoPreview() {
        const endereco = this.userData.endereco;
        if (!endereco) return;

        const enderecoCompleto = `${endereco.logradouro}, ${endereco.numero}${endereco.complemento ? `, ${endereco.complemento}` : ''} - ${endereco.bairro}, ${endereco.cidade}/${endereco.estado}`;

        const enderecoElement = document.getElementById('endereco-completo');
        const cepElement = document.getElementById('endereco-cep');

        if (enderecoElement) enderecoElement.textContent = enderecoCompleto;
        if (cepElement) cepElement.textContent = `CEP: ${endereco.cep}`;

        const previewElement = document.getElementById('endereco-preview');
        if (previewElement) previewElement.style.display = 'block';
    }

    async buscarCEP() {
        const cepInput = document.getElementById('cep');
        if (!cepInput) return;

        const cep = cepInput.value.replace(/\D/g, '');

        if (cep.length !== 8) {
            this.addLogTemporario('CEP deve ter 8 dígitos', 'error');
            return;
        }

        try {
            const response = await fetch(`https://viacep.com.br/ws/${cep}/json/`);
            const data = await response.json();

            if (data.erro) {
                throw new Error('CEP não encontrado');
            }

            const logradouroInput = document.getElementById('logradouro');
            const bairroInput = document.getElementById('bairro');
            const cidadeInput = document.getElementById('cidade');
            const estadoInput = document.getElementById('estado');

            if (logradouroInput) logradouroInput.value = data.logradouro || '';
            if (bairroInput) bairroInput.value = data.bairro || '';
            if (cidadeInput) cidadeInput.value = data.localidade || '';
            if (estadoInput) estadoInput.value = data.uf || '';

            [logradouroInput, bairroInput, cidadeInput, estadoInput].forEach(input => {
                if (input) this.validateField(input);
            });

            this.addLogTemporario('Endereço preenchido com sucesso', 'active');

            setTimeout(() => {
                if (window.FreteCalculator) {
                    window.FreteCalculator.recalcularFrete();
                }
            }, 500);

            const numeroInput = document.getElementById('numero');
            if (numeroInput) numeroInput.focus();

        } catch (error) {
            this.addLogTemporario('Revise os seus dados', 'error');
            console.error('Erro ao buscar CEP:', error);
        }
    }

    goToStep(step) {
        if (step < 1 || step > this.maxStep) return;

        if (step > 1 && !this.isValid.contato) {
            this.addLogTemporario('Preencha todos os dados de Contato primeiro', 'error');
            return;
        }

        if (step > 2 && !this.isValid.endereco) {
            this.addLogTemporario('Preencha todos os dados de Endereço primeiro', 'error');
            return;
        }

        this.currentStep = step;
        this.updateUI();

        if (step === 1) {
            this.addLog('Preencha as informações de Contato', 'info');
        } else if (step === 2) {
            this.addLog('Preencha as informações de Endereço', 'info');
        } else if (step === 3) {
            this.addLog('Selecione a forma de pagamento', 'info');
        }
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
        this.progressSteps.forEach((step, index) => {
            const stepNumber = index + 1;
            step.classList.toggle('active', stepNumber <= this.currentStep);
            step.classList.toggle('completed', stepNumber < this.currentStep);
        });

        this.tabContents.forEach((tab, index) => {
            const stepNumber = index + 1;
            tab.classList.toggle('active', stepNumber === this.currentStep);
        });
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
        window.CheckoutManager = this;

        const finalForm = document.getElementById('form-finalizar-pedido');
        if (finalForm) {
            finalForm.addEventListener('submit', (e) => {
                e.preventDefault();

                const selectedPayment = document.querySelector('input[name="metodo"]:checked');
                if (!selectedPayment) {
                    this.addLogTemporario('Selecione uma forma de pagamento', 'error');
                    return;
                }

                if (window.CheckoutCarrinho && !window.CheckoutCarrinho.validarCarrinhoParaCheckout()) {
                    this.addLogTemporario('Dados do carrinho inválidos para finalização', 'error');
                    return;
                }

                
                this.enviarFormularioPagamento(selectedPayment.value);
            });
        }
    }

    enviarFormularioPagamento(metodo) {
        this.addLogTemporario('Finalizando pedido...', 'processing');

        const form = document.getElementById('form-finalizar-pedido');
        if (!form) {
            this.addLogTemporario('Erro: formulário não encontrado', 'error');
            return;
        }

        
        const existingInputs = form.querySelectorAll('input[type="hidden"]');
        existingInputs.forEach(input => {
            if (input.name !== '__RequestVerificationToken') {
                input.remove();
            }
        });

        
        const inputContato = document.createElement('input');
        inputContato.type = 'hidden';
        inputContato.name = 'dadosContato';
        inputContato.value = JSON.stringify(this.userData.contato);
        form.appendChild(inputContato);

        
        const inputEndereco = document.createElement('input');
        inputEndereco.type = 'hidden';
        inputEndereco.name = 'dadosEndereco';
        inputEndereco.value = JSON.stringify(this.userData.endereco);
        form.appendChild(inputEndereco);

     
        console.log('Dados de contato:', this.userData.contato);
        console.log('Dados de endereço:', this.userData.endereco);
        console.log('Método:', metodo);

        
        form.submit();
    }
}

document.addEventListener('DOMContentLoaded', () => {
    new CheckoutManager();
});