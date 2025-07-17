document.addEventListener('DOMContentLoaded', function () {
    console.log('Carrinho Page - JavaScript inicializado');
    inicializarCarrinho();


    if (window.location.pathname.includes('/Carrinho')) {
        desabilitarEventosSidebar();
    }
});

function desabilitarEventosSidebar() {

    const carrinhoIcon = document.querySelector('.fa-cart-shopping');
    if (carrinhoIcon) {
        carrinhoIcon.removeEventListener('click', function () { });
    }
}

function inicializarCarrinho() {
    const carrinhoItems = document.querySelectorAll('.carrinho-item');
    console.log(`Carrinho inicializado com ${carrinhoItems.length} itens`);

    carrinhoItems.forEach(item => {
        const produtoId = item.dataset.produtoId;
        console.log(`Item inicializado: ${produtoId}`);
    });
}

let processandoRequest = false;

async function alterarQuantidadeSegura(produtoId, operacao) {
    if (processandoRequest) {
        console.log('Já está processando uma requisição, aguarde...');
        return;
    }

    processandoRequest = true;

    try {

        const itemElement = document.querySelector(`[data-produto-id="${produtoId}"]`);
        const quantidadeAtual = parseInt(itemElement?.querySelector('.quantidade-valor')?.textContent || '0');

        let novaQuantidade;
        if (operacao === 'mais') {
            novaQuantidade = quantidadeAtual + 1;
        } else if (operacao === 'menos') {
            novaQuantidade = quantidadeAtual - 1;
        } else {

            novaQuantidade = parseInt(operacao);
        }

        console.log(`Operação: ${operacao}, Quantidade atual: ${quantidadeAtual}, Nova quantidade: ${novaQuantidade}`);

        await alterarQuantidade(produtoId, novaQuantidade);
    } finally {
        processandoRequest = false;
    }
}

async function alterarQuantidade(produtoId, novaQuantidade) {
    if (!produtoId) {
        console.error('ID do produto não informado');
        mostrarFeedback('Erro: ID do produto não informado', 'error');
        return;
    }

    if (novaQuantidade < 0) {
        console.warn('Quantidade não pode ser negativa');
        return;
    }

    if (novaQuantidade === 0) {
        await removerItem(produtoId);
        return;
    }

    try {
        console.log(`Alterando quantidade do produto ${produtoId} para ${novaQuantidade}`);

        mostrarLoading(produtoId);

        const formData = new FormData();
        formData.append('produtoId', produtoId);
        formData.append('quantidade', novaQuantidade);

        const token = document.querySelector('input[name="__RequestVerificationToken"]')?.value;
        if (token) {
            formData.append('__RequestVerificationToken', token);
        }

        const response = await fetch('/Carrinho/AtualizarQuantidadePage', {
            method: 'POST',
            body: formData
        });

        const result = await response.json();

        if (result.sucesso) {
            console.log('Quantidade alterada com sucesso:', result);


            const itemAtualizado = result.carrinho.itens.find(i => i.idProduto === produtoId);
            if (itemAtualizado) {
                atualizarItemInterface(produtoId, itemAtualizado.quantidade, itemAtualizado.subtotal);
            }


            atualizarTotaisCompletos(result.carrinho);

            mostrarFeedback('Quantidade atualizada!', 'success');
        } else {
            console.error('Erro ao alterar quantidade:', result.mensagem);
            mostrarFeedback(result.mensagem || 'Erro ao alterar quantidade', 'error');
        }
    } catch (error) {
        console.error('Erro na requisição:', error);
        mostrarFeedback('Erro de conexão. Tente novamente.', 'error');
    } finally {
        ocultarLoading(produtoId);
    }
}

async function removerItem(produtoId) {
    if (!produtoId) {
        console.error('ID do produto não informado');
        mostrarFeedback('Erro: ID do produto não informado', 'error');
        return;
    }

    if (!confirm('Tem certeza que deseja remover este item do carrinho?')) {
        return;
    }

    try {
        console.log(`Removendo produto ${produtoId} do carrinho`);

        mostrarLoading(produtoId);

        const formData = new FormData();
        formData.append('produtoId', produtoId);

        const token = document.querySelector('input[name="__RequestVerificationToken"]')?.value;
        if (token) {
            formData.append('__RequestVerificationToken', token);
        }

        const response = await fetch('/Carrinho/RemoverProdutoPage', {
            method: 'POST',
            body: formData
        });

        const result = await response.json();

        if (result.sucesso) {
            console.log('Item removido com sucesso:', result);

            removerItemDaInterface(produtoId);

            if (result.carrinho && result.carrinho.itens.length > 0) {
                atualizarTotaisCompletos(result.carrinho);
            } else {

                setTimeout(() => {
                    location.reload();
                }, 500);
            }

            mostrarFeedback('Item removido do carrinho!', 'success');
        } else {
            console.error('Erro ao remover item:', result.mensagem);
            mostrarFeedback(result.mensagem || 'Erro ao remover item', 'error');
        }
    } catch (error) {
        console.error('Erro na requisição:', error);
        mostrarFeedback('Erro de conexão. Tente novamente.', 'error');
    } finally {
        ocultarLoading(produtoId);
    }
}

function atualizarItemInterface(produtoId, novaQuantidade, novoSubtotal) {
    const itemElement = document.querySelector(`[data-produto-id="${produtoId}"]`);
    if (!itemElement) return;

    const quantidadeElement = itemElement.querySelector('.quantidade-valor');
    const subtotalElement = itemElement.querySelector('.item-subtotal');
    const btnMenos = itemElement.querySelector('.btn-menos');

    if (quantidadeElement) {
        quantidadeElement.textContent = novaQuantidade;
    }

    if (subtotalElement && novoSubtotal) {
        subtotalElement.textContent = `R$ ${novoSubtotal.toFixed(2)}`;
    }

    if (btnMenos) {
        btnMenos.disabled = novaQuantidade <= 1;
    }

    itemElement.style.animation = 'fadeIn 0.3s ease-out';
}

function removerItemDaInterface(produtoId) {
    const itemElement = document.querySelector(`[data-produto-id="${produtoId}"]`);
    if (itemElement) {
        itemElement.style.animation = 'slideOutLeft 0.3s ease-in';
        setTimeout(() => {
            itemElement.remove();
        }, 300);
    }
}

function atualizarTotaisCompletos(carrinho) {
    const resumoQuantidade = document.querySelector('#resumo-quantidade');
    const resumoTotal = document.querySelector('#resumo-total');
    const carrinhoTitulo = document.querySelector('.carrinho-titulo');

    // Encontrar o elemento do subtotal no resumo
    const resumoSubtotal = document.querySelector('#resumo-subtotal');

    if (resumoQuantidade) {
        resumoQuantidade.textContent = carrinho.quantidadeTotal;
    }

    if (resumoTotal) {
        resumoTotal.textContent = `R$ ${carrinho.total.toFixed(2)}`;
    }

    // Atualizar o subtotal no resumo (mesmo valor que o total)
    if (resumoSubtotal) {
        resumoSubtotal.textContent = `R$ ${carrinho.total.toFixed(2)}`;
    }

    if (carrinhoTitulo) {
        carrinhoTitulo.innerHTML = `<i class="fa-solid fa-cart-shopping me-3"></i>CARRINHO (${carrinho.quantidadeTotal})`;
    }


    carrinho.itens.forEach(item => {
        const itemElement = document.querySelector(`[data-produto-id="${item.idProduto}"]`);
        if (itemElement) {
            const subtotalElement = itemElement.querySelector('.item-subtotal');
            if (subtotalElement) {
                subtotalElement.textContent = `R$ ${item.subtotal.toFixed(2)}`;
            }
        }
    });
}

function mostrarLoading(produtoId) {
    const itemElement = document.querySelector(`[data-produto-id="${produtoId}"]`);
    if (itemElement) {
        itemElement.style.opacity = '0.6';
        itemElement.style.pointerEvents = 'none';


        const botoes = itemElement.querySelectorAll('.btn-quantidade');
        botoes.forEach(btn => {
            btn.disabled = true;
            btn.style.cursor = 'not-allowed';
        });

        let loadingSpinner = itemElement.querySelector('.loading-spinner');
        if (!loadingSpinner) {
            loadingSpinner = document.createElement('div');
            loadingSpinner.className = 'loading-spinner';
            loadingSpinner.innerHTML = '<i class="fa-solid fa-spinner fa-spin"></i>';
            loadingSpinner.style.cssText = `
                position: absolute;
                top: 50%;
                left: 50%;
                transform: translate(-50%, -50%);
                z-index: 1000;
                color: #FF4E4E;
                font-size: 1.5rem;
                background: rgba(255, 255, 255, 0.8);
                padding: 1rem;
                border-radius: 50%;
            `;

            itemElement.style.position = 'relative';
            itemElement.appendChild(loadingSpinner);
        }
    }
}

function ocultarLoading(produtoId) {
    const itemElement = document.querySelector(`[data-produto-id="${produtoId}"]`);
    if (itemElement) {
        itemElement.style.opacity = '1';
        itemElement.style.pointerEvents = 'auto';


        const botoes = itemElement.querySelectorAll('.btn-quantidade');
        botoes.forEach(btn => {
            btn.disabled = false;
            btn.style.cursor = 'pointer';
        });


        const quantidadeAtual = parseInt(itemElement.querySelector('.quantidade-valor')?.textContent || '0');
        const btnMenos = itemElement.querySelector('.btn-menos');
        if (btnMenos && quantidadeAtual <= 1) {
            btnMenos.disabled = true;
        }

        const loadingSpinner = itemElement.querySelector('.loading-spinner');
        if (loadingSpinner) {
            loadingSpinner.remove();
        }
    }
}

function mostrarFeedback(mensagem, tipo = 'info') {
    const existingToast = document.querySelector('.toast-feedback');
    if (existingToast) {
        existingToast.remove();
    }

    const toast = document.createElement('div');
    toast.className = `toast-feedback toast-${tipo}`;
    toast.innerHTML = `
        <div class="toast-content">
            <i class="fa-solid ${tipo === 'success' ? 'fa-check-circle' : 'fa-exclamation-circle'}"></i>
            <span>${mensagem}</span>
        </div>
    `;

    const backgroundColor = tipo === 'success' ? '#28a745' : '#dc3545';
    toast.style.cssText = `
        position: fixed;
        top: 20px;
        right: 20px;
        background: ${backgroundColor};
        color: white;
        padding: 1rem 1.5rem;
        border-radius: 10px;
        box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
        z-index: 10001;
        display: flex;
        align-items: center;
        gap: 0.5rem;
        transform: translateX(100%);
        transition: all 0.3s ease;
        font-weight: 600;
    `;

    document.body.appendChild(toast);

    setTimeout(() => {
        toast.style.transform = 'translateX(0)';
    }, 100);

    setTimeout(() => {
        toast.style.transform = 'translateX(100%)';
        setTimeout(() => {
            toast.remove();
        }, 300);
    }, 3000);
}

function finalizarCompra() {
    const resumoQuantidade = document.querySelector('#resumo-quantidade');
    if (!resumoQuantidade || resumoQuantidade.textContent === '0') {
        mostrarFeedback('Seu carrinho está vazio!', 'error');
        return;
    }


    window.location.href = '/Carrinho/Checkout';
}

async function verificarEstoque() {
    try {
        const response = await fetch('/Carrinho/ValidarEstoqueCompleto', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]')?.value
            }
        });

        const result = await response.json();

        if (!result.sucesso) {
            mostrarFeedback(result.mensagem, 'error');
            if (result.carrinho) {
                setTimeout(() => {
                    location.reload();
                }, 2000);
            }
        }

        return result.sucesso;
    } catch (error) {
        console.error('Erro ao verificar estoque:', error);
        return false;
    }
}