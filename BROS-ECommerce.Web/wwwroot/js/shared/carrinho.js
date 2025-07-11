
/**
 * 
 * @param {string} produtoId 
 * @param {number} quantidade 
 */
async function adicionarAoCarrinho(produtoId, quantidade = 1) {
    try {
        const token = document.querySelector('input[name="__RequestVerificationToken"]')?.value;

        const response = await fetch('/Carrinho/AdicionarProduto', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded',
            },
            body: `produtoId=${produtoId}&quantidade=${quantidade}` + (token ? `&__RequestVerificationToken=${token}` : '')
        });

        const result = await response.json();

        if (result.sucesso) {
            mostrarMensagem(result.mensagem, 'sucesso');

            atualizarContadorCarrinho(result.quantidadeItens);

            setTimeout(() => {
                if (confirm('Produto adicionado com sucesso! Deseja ir para o carrinho?')) {
                    window.location.href = result.redirect;
                }
            }, 1000);
        } else {
            mostrarMensagem('Erro: ' + result.mensagem, 'erro');
        }
    } catch (error) {
        console.error('Erro ao adicionar produto:', error);
        mostrarMensagem('Erro ao adicionar produto ao carrinho', 'erro');
    }
}

/**
 * 
 * @param {number} quantidade 
 */
function atualizarContadorCarrinho(quantidade) {
=    const contadorCarrinho = document.querySelector('#contador-carrinho');
    if (contadorCarrinho) {
        contadorCarrinho.textContent = quantidade;
        contadorCarrinho.style.display = quantidade > 0 ? 'inline-block' : 'none';
    }

    const iconeCarrinho = document.querySelector('.fa-cart-shopping');
    if (iconeCarrinho) {
        if (quantidade > 0) {
            iconeCarrinho.style.color = '#FF4E4E';
            iconeCarrinho.classList.add('carrinho-com-itens');
        } else {
            iconeCarrinho.style.color = '';
            iconeCarrinho.classList.remove('carrinho-com-itens');
        }
    }
}

/**
 * 
 * @param {string} mensagem 
 * @param {string} tipo 
 */
function mostrarMensagem(mensagem, tipo) {
   
    const alertsAnteriores = document.querySelectorAll('.alert-carrinho');
    alertsAnteriores.forEach(alert => alert.remove());

    
    const alertDiv = document.createElement('div');
    alertDiv.className = `alert alert-${tipo === 'sucesso' ? 'success' : 'danger'} alert-dismissible fade show position-fixed alert-carrinho`;
    alertDiv.style.cssText = `
        top: 20px; 
        right: 20px; 
        z-index: 9999; 
        min-width: 300px;
        max-width: 400px;
        box-shadow: 0 4px 6px rgba(0,0,0,0.1);
        border-radius: 8px;
        animation: slideInRight 0.3s ease-out;
    `;

    alertDiv.innerHTML = `
        <div class="d-flex align-items-center">
            <i class="fas fa-${tipo === 'sucesso' ? 'check-circle' : 'exclamation-circle'} me-2"></i> 
            <span>${mensagem}</span>
            <button type="button" class="btn-close ms-auto" onclick="this.parentElement.parentElement.remove()"></button>
        </div>
    `;

    document.body.appendChild(alertDiv);

    
    setTimeout(() => {
        if (alertDiv.parentNode) {
            alertDiv.style.animation = 'slideOutRight 0.3s ease-in';
            setTimeout(() => alertDiv.remove(), 300);
        }
    }, 4000);
}


async function carregarQuantidadeCarrinho() {
    try {
        const response = await fetch('/Carrinho/QuantidadeItens');
        const result = await response.json();
        atualizarContadorCarrinho(result.quantidade || 0);
    } catch (error) {
        console.error('Erro ao carregar quantidade do carrinho:', error);
    }
}

if (!document.querySelector('#carrinho-animations')) {
    const style = document.createElement('style');
    style.id = 'carrinho-animations';
    style.textContent = `
        @keyframes slideInRight {
            from { transform: translateX(100%); opacity: 0; }
            to { transform: translateX(0); opacity: 1; }
        }
        @keyframes slideOutRight {
            from { transform: translateX(0); opacity: 1; }
            to { transform: translateX(100%); opacity: 0; }
        }
        .carrinho-com-itens {
            animation: pulse 2s infinite;
        }
        @keyframes pulse {
            0% { transform: scale(1); }
            50% { transform: scale(1.1); }
            100% { transform: scale(1); }
        }
    `;
    document.head.appendChild(style);
}

document.addEventListener('DOMContentLoaded', carregarQuantidadeCarrinho);