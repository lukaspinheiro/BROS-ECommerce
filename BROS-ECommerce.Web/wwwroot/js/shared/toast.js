window.ToastManager = {
    mostrarFeedback: function (mensagem, tipo = 'info') {
        const existingToast = document.querySelector('.toast-feedback');
        if (existingToast) {
            existingToast.remove();
        }

        const toast = document.createElement('div');
        toast.className = `toast-feedback toast-${tipo}`;
        toast.innerHTML = `
            <div class="toast-content">
                <i class="fa-solid ${this.obterIcone(tipo)}"></i>
                <span>${mensagem}</span>
            </div>
        `;

        document.body.appendChild(toast);

        setTimeout(() => {
            toast.classList.add('show');
        }, 100);

        setTimeout(() => {
            toast.classList.remove('show');
            setTimeout(() => {
                if (toast.parentNode) {
                    toast.remove();
                }
            }, 300);
        }, 4000);
    },

    obterIcone: function (tipo) {
        switch (tipo) {
            case 'success': return 'fa-check-circle';
            case 'error': return 'fa-exclamation-circle';
            case 'warning': return 'fa-exclamation-triangle';
            case 'info': return 'fa-info-circle';
            default: return 'fa-info-circle';
        }
    }
};