

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
            atualizarContadorCarrinho(result.quantidadeItens);

            if (window.carrinhoSidebar) {
                await window.carrinhoSidebar.abrir();
                window.carrinhoSidebar.mostrarToast('Produto adicionado com sucesso!', 'success');
            } else {
                mostrarMensagem('Produto adicionado com sucesso!', 'sucesso');

                setTimeout(() => {
                    if (confirm('Produto adicionado com sucesso! Deseja ir para o carrinho?')) {
                        window.location.href = '/Carrinho';
                    }
                }, 1000);
            }
        } else {
            if (window.carrinhoSidebar) {
                window.carrinhoSidebar.mostrarToast('Erro: ' + result.mensagem, 'error');
            } else {
                mostrarMensagem('Erro: ' + result.mensagem, 'erro');
            }
        }
    } catch (error) {
        console.error('Erro ao adicionar produto:', error);
        if (window.carrinhoSidebar) {
            window.carrinhoSidebar.mostrarToast('Erro ao adicionar produto ao carrinho', 'error');
        } else {
            mostrarMensagem('Erro ao adicionar produto ao carrinho', 'erro');
        }
    }
}

/**
 * 
 * @param {number} quantidade 
 */
function atualizarContadorCarrinho(quantidade) {
    const contadorCarrinho = document.querySelector('#contador-carrinho');
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
            alertDiv.style.animation = 'slideOutRight 0.3s ease-in forwards';
            setTimeout(() => alertDiv.remove(), 300);
        }
    }, 4000);
}


async function carregarContadorInicial() {
    try {
        const response = await fetch('/Carrinho/QuantidadeItens');
        const data = await response.json();
        atualizarContadorCarrinho(data.quantidade);
    } catch (error) {
        console.log('Erro ao carregar contador inicial:', error);
    }
}

document.addEventListener('DOMContentLoaded', function () {
    carregarContadorInicial();

    if (!document.getElementById('carrinho-animations-fallback')) {
        const style = document.createElement('style');
        style.id = 'carrinho-animations-fallback';
        style.textContent = `
            @keyframes slideInRight {
                from { opacity: 0; transform: translateX(20px); }
                to { opacity: 1; transform: translateX(0); }
            }
            @keyframes slideOutRight {
                from { opacity: 1; transform: translateX(0); }
                to { opacity: 0; transform: translateX(20px); }
            }
        `;
        document.head.appendChild(style);
    }
});

window.adicionarAoCarrinho = adicionarAoCarrinho;
window.atualizarContadorCarrinho = atualizarContadorCarrinho;
window.mostrarMensagem = mostrarMensagem;