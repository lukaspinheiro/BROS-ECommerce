class CheckoutUserData {
    constructor() {
        this.userData = null;
        this.isLoading = false;
        this.init();
    }

    async init() {
        await this.carregarDadosUsuario();
        this.preencherFormulario();
        this.configurarValidacaoEmTempoReal();
    }

    async carregarDadosUsuario() {
        if (this.isLoading) return;

        this.isLoading = true;

        try {
            const response = await fetch('/Conta/Perfil/DadosCheckout', {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json'
                },
                credentials: 'include'
            });

            if (response.ok) {
                const result = await response.json();

                if (result.authenticated && result.nomeCompleto) {
                    this.userData = result;
                    this.mostrarLogSucesso('Dados do usuário carregados com sucesso');
                } else {
                    this.mostrarLogInfo('Usuário não autenticado - preenchimento manual permitido');
                }
            } else {
                this.mostrarLogInfo('Dados do usuário não disponíveis - preenchimento manual');
            }
        } catch (error) {
            console.warn('Erro ao carregar dados do usuário:', error);
            this.mostrarLogInfo('Preenchimento manual dos dados necessário');
        } finally {
            this.isLoading = false;
        }
    }

    preencherFormulario() {
        if (!this.userData) return;

        const campos = [
            { campo: 'nome-completo', propriedade: 'nomeCompleto', fallback: 'nome' },
            { campo: 'email', propriedade: 'email' },
            { campo: 'cpf', propriedade: 'cpf' }
        ];

        campos.forEach(({ campo, propriedade, fallback }) => {
            const input = document.getElementById(campo);
            if (input && !input.value) {
                const valor = this.userData[propriedade] || (fallback ? this.userData[fallback] : '');
                if (valor) {
                    input.value = valor;

                    input.dispatchEvent(new Event('input', { bubbles: true }));
                    input.dispatchEvent(new Event('change', { bubbles: true }));

                    if (window.CheckoutForms && window.CheckoutForms.validateField) {
                        window.CheckoutForms.validateField(input);
                    }

                    input.classList.add('auto-filled');
                }
            }
        });

        this.adicionarEstilosVisuais();
    }

    configurarValidacaoEmTempoReal() {
        const campos = ['nome-completo', 'email', 'telefone', 'cpf'];

        campos.forEach(campoId => {
            const input = document.getElementById(campoId);
            if (input) {
                input.addEventListener('input', () => {
                    this.validarCampo(input);
                });

                input.addEventListener('blur', () => {
                    this.validarCampo(input);
                });
            }
        });
    }

    validarCampo(input) {
        if (!input.value.trim()) {
            this.mostrarLogCampoFaltante(this.obterNomeCampo(input.id));
            return false;
        }

        if (window.CheckoutForms && window.CheckoutForms.validateField) {
            const isValid = window.CheckoutForms.validateField(input);

            if (isValid) {
                this.mostrarLogCampoCorreto(this.obterNomeCampo(input.id));
            } else {
                this.mostrarLogCampoIncorreto(this.obterNomeCampo(input.id));
            }

            return isValid;
        }

        return true;
    }

    obterNomeCampo(campoId) {
        const nomes = {
            'nome-completo': 'Nome completo',
            'email': 'E-mail',
            'telefone': 'Telefone',
            'cpf': 'CPF'
        };
        return nomes[campoId] || campoId;
    }

    adicionarEstilosVisuais() {
        if (!document.getElementById('checkout-user-data-styles')) {
            const style = document.createElement('style');
            style.id = 'checkout-user-data-styles';
            style.textContent = `
                .auto-filled {
                    background-color: #f8fff8 !important;
                    border-left: 3px solid #28a745 !important;
                }
                
                .auto-filled:focus {
                    background-color: #ffffff !important;
                    box-shadow: 0 0 0 0.2rem rgba(40, 167, 69, 0.25) !important;
                }
                
                .user-data-log {
                    position: fixed;
                    top: 20px;
                    right: 20px;
                    background: #fff;
                    border-radius: 8px;
                    padding: 12px 16px;
                    box-shadow: 0 4px 12px rgba(0,0,0,0.15);
                    z-index: 9999;
                    max-width: 300px;
                    opacity: 0;
                    transform: translateX(100%);
                    transition: all 0.3s ease;
                }
                
                .user-data-log.show {
                    opacity: 1;
                    transform: translateX(0);
                }
                
                .user-data-log.success {
                    border-left: 4px solid #28a745;
                    background: #d4edda;
                    color: #155724;
                }
                
                .user-data-log.info {
                    border-left: 4px solid #17a2b8;
                    background: #d1ecf1;
                    color: #0c5460;
                }
                
                .user-data-log.warning {
                    border-left: 4px solid #ffc107;
                    background: #fff3cd;
                    color: #856404;
                }
                
                .user-data-log.error {
                    border-left: 4px solid #dc3545;
                    background: #f8d7da;
                    color: #721c24;
                }
            `;
            document.head.appendChild(style);
        }
    }

    mostrarLog(mensagem, tipo = 'info') {
        const log = document.createElement('div');
        log.className = `user-data-log ${tipo}`;
        log.textContent = mensagem;

        document.body.appendChild(log);

        setTimeout(() => log.classList.add('show'), 100);

        setTimeout(() => {
            log.classList.remove('show');
            setTimeout(() => {
                if (log.parentNode) {
                    log.parentNode.removeChild(log);
                }
            }, 300);
        }, 3000);
    }

    mostrarLogSucesso(mensagem) {
        this.mostrarLog(mensagem, 'success');
    }

    mostrarLogInfo(mensagem) {
        this.mostrarLog(mensagem, 'info');
    }

    mostrarLogCampoCorreto(nomeCampo) {
        this.mostrarLog(`${nomeCampo} preenchido corretamente`, 'success');
    }

    mostrarLogCampoIncorreto(nomeCampo) {
        this.mostrarLog(`${nomeCampo} contém erro - verifique o formato`, 'error');
    }

    mostrarLogCampoFaltante(nomeCampo) {
        this.mostrarLog(`${nomeCampo} é obrigatório`, 'warning');
    }

    recarregarDados() {
        this.userData = null;
        return this.carregarDadosUsuario().then(() => {
            this.preencherFormulario();
        });
    }

    limparFormulario() {
        const campos = ['nome-completo', 'email', 'telefone', 'cpf'];

        campos.forEach(campoId => {
            const input = document.getElementById(campoId);
            if (input) {
                input.value = '';
                input.classList.remove('auto-filled', 'is-valid', 'is-invalid');
            }
        });

        this.mostrarLogInfo('Formulário limpo - preencha manualmente');
    }
}

document.addEventListener('DOMContentLoaded', () => {
    if (document.getElementById('form-contato')) {
        window.CheckoutUserData = new CheckoutUserData();
    }
});