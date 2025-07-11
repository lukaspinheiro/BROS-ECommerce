

async function alterarQuantidade(carrinhoId, produtoId, novaQuantidade) {
    if (novaQuantidade < 0) return;

    try {
        const token = document.querySelector('input[name="__RequestVerificationToken"]')?.value;

        const response = await fetch('/Carrinho/AtualizarQuantidade', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded',
            },
            body: `carrinhoId=${carrinhoId}&produtoId=${produtoId}&quantidade=${novaQuantidade}` +
                (token ? `&__RequestVerificationToken=${token}` : '')
        });

        const result = await response.json();

        if (result.sucesso) {
            if (novaQuantidade === 0) {
                location.reload();
            } else {
                atualizarInterfaceCarrinho(produtoId, novaQuantidade, result);
            }
        } else {
            mostrarMensagem('Erro ao atualizar quantidade: ' + result.mensagem, 'erro');
        }
    } catch (error) {
        console.error('Erro ao alterar quantidade:', error);
        mostrarMensagem('Erro ao atualizar quantidade', 'erro');
    }
}

async function removerProduto(carrinhoId, produtoId) {
    if (!confirm('Tem certeza que deseja remover este produto do carrinho?')) {
        return;
    }

    try {
        const token = document.querySelector('input[name="__RequestVerificationToken"]')?.value;

        const response = await fetch('/Carrinho/RemoverProduto', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded',
            },
            body: `carrinhoId=${carrinhoId}&produtoId=${produtoId}` +
                (token ? `&__RequestVerificationToken=${token}` : '')
        });

        const result = await response.json();

        if (result.sucesso) {
            mostrarMensagem('Produto removido do carrinho', 'sucesso');

            if (!result.temItens) {
                setTimeout(() => location.reload(), 1000);
            } else {
                removerItemDaInterface(produtoId);
                atualizarTotais(result.quantidadeItens, result.valorTotal);
            }
        } else {
            mostrarMensagem('Erro ao remover produto: ' + result.mensagem, 'erro');
        }
    } catch (error) {
        console.error('Erro ao remover produto:', error);
        mostrarMensagem('Erro ao remover produto', 'erro');
    }
}

function atualizarInterfaceCarrinho(produtoId, novaQuantidade, result) {
    const itemElement = document.querySelector(`[data-produto-id="${produtoId}"]`);
    if (itemElement) {
        const quantidadeElement = itemElement.querySelector('.quantidade-valor');
        if (quantidadeElement) {
            quantidadeElement.textContent = novaQuantidade;
        }

        const precoElement = itemElement.querySelector('.item-preco');
        const subtotalElement = itemElement.querySelector('.item-subtotal');

        if (precoElement && subtotalElement) {
            const preco = parseFloat(precoElement.textContent.replace(

            /**
             * 
             * @param {string} carrinhoId 
             * @param {string} produtoId
             * @param {number} novaQuantidade 
             */
            async function alterarQuantidade(carrinhoId, produtoId, novaQuantidade) {
                if (novaQuantidade < 0) return;

                try {
                    const token = document.querySelector('input[name="__RequestVerificationToken"]')?.value;

                    const response = await fetch('/Carrinho/AtualizarQuantidade', {
                        method: 'POST',
                        headers: {
                            'Content-Type': 'application/x-www-form-urlencoded',
                        },
                        body: `carrinhoId=${carrinhoId}&produtoId=${produtoId}&quantidade=${novaQuantidade}` +
                            (token ? `&__RequestVerificationToken=${token}` : '')
                    });

                    const result = await response.json();

                    if (result.sucesso) {
                        if (novaQuantidade === 0) {
                            location.reload();
                        } else {
                            atualizarInterfaceCarrinho(produtoId, novaQuantidade, result);
                        }
                    } else {
                        mostrarMensagem('Erro ao atualizar quantidade: ' + result.mensagem, 'erro');
                    }
                } catch (error) {
                    console.error('Erro ao alterar quantidade:', error);
                    mostrarMensagem('Erro ao atualizar quantidade', 'erro');
                }
            }

            /**
             * 
             * @param {string} carrinhoId 
             * @param {string} produtoId 
             */
            async function removerProduto(carrinhoId, produtoId) {
                if (!confirm('Tem certeza que deseja remover este produto do carrinho?')) {
                    return;
                }

                try {
                    const token = document.querySelector('input[name="__RequestVerificationToken"]')?.value;

                    const response = await fetch('/Carrinho/RemoverProduto', {
                        method: 'POST',
                        headers: {
                            'Content-Type': 'application/x-www-form-urlencoded',
                        },
                        body: `carrinhoId=${carrinhoId}&produtoId=${produtoId}` +
                            (token ? `&__RequestVerificationToken=${token}` : '')
                    });

                    const result = await response.json();

                    if (result.sucesso) {
                        mostrarMensagem('Produto removido do carrinho', 'sucesso');

                        
                        if (!result.temItens) {
                            setTimeout(() => location.reload(), 1000);
                        } else {
                            
                            removerItemDaInterface(produtoId);
                            atualizarTotais(result.quantidadeItens, result.valorTotal);
                        }
                    } else {
                        mostrarMensagem('Erro ao remover produto: ' + result.mensagem, 'erro');
                    }
                } catch (error) {
                    console.error('Erro ao remover produto:', error);
                    mostrarMensagem('Erro ao remover produto', 'erro');
                }
            }

            /**
             * 
             * @param {string} produtoId 
             * @param {number} novaQuantidade 
             * @param {object} result 
             */
            function atualizarInterfaceCarrinho(produtoId, novaQuantidade, result) {
               
                const itemElement = document.querySelector(`[data-produto-id="${produtoId}"]`);
                if (itemElement) {
                    const quantidadeElement = itemElement.querySelector('.quantidade-valor');
                    if (quantidadeElement) {
                        quantidadeElement.textContent = novaQuantidade;
                    }

                    
                    const precoElement = itemElement.querySelector('.item-preco');
                    const subtotalElement = itemElement.querySelector('.item-subtotal');

                    if (precoElement && subtotalElement) {
                        const preco = parseFloat(precoElement.textContent.replace('R$', '').replace(',', '.'));
                        const subtotal = preco * novaQuantidade;
                        subtotalElement.textContent = `R$ ${subtotal.toFixed(2).replace('.', ',')}`;
                    }
                }

                
                atualizarTotais(result.quantidadeItens, result.valorTotal);
            }

            /**
             * 
             * @param {string} produtoId 
             */
            function removerItemDaInterface(produtoId) {
                const itemElement = document.querySelector(`[data-produto-id="${produtoId}"]`);
                if (itemElement) {
                    itemElement.style.animation = 'slideOutLeft 0.3s ease-in';
                    setTimeout(() => {
                        itemElement.remove();
                    }, 300);
                }
            }

            /**
             * 
             * @param {number} quantidade 
             * @param {string} valorTotal 
             */
            function atualizarTotais(quantidade, valorTotal) {
                const resumoQuantidade = document.querySelector('#resumo-quantidade');
                const resumoTotal = document.querySelector('#resumo-total');

                if (resumoQuantidade) {
                    resumoQuantidade.textContent = quantidade;
                }

                if (resumoTotal) {
                    resumoTotal.textContent = valorTotal;
                }

                
                const carrinhoTitulo = document.querySelector('.carrinho-titulo');
                if (carrinhoTitulo) {
                    carrinhoTitulo, '').replace(',', '.'));
                    const subtotal = preco * novaQuantidade;
                    subtotalElement.textContent = `R$ ${subtotal.toFixed(2).replace('.', ',')}`;
                }
            }

            atualizarTotais(result.quantidadeItens, result.valorTotal);
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

        function atualizarTotais(quantidade, valorTotal) {
            const resumoQuantidade = document.querySelector(

            /**
             *
             * @param {string} carrinhoId
             * @param {string} produtoId 
             * @param {number} novaQuantidade 
             */
            async function alterarQuantidade(carrinhoId, produtoId, novaQuantidade) {
                if (novaQuantidade < 0) return;

                try {
                    const token = document.querySelector('input[name="__RequestVerificationToken"]')?.value;

                    const response = await fetch('/Carrinho/AtualizarQuantidade', {
                        method: 'POST',
                        headers: {
                            'Content-Type': 'application/x-www-form-urlencoded',
                        },
                        body: `carrinhoId=${carrinhoId}&produtoId=${produtoId}&quantidade=${novaQuantidade}` +
                            (token ? `&__RequestVerificationToken=${token}` : '')
                    });

                    const result = await response.json();

                    if (result.sucesso) {
                        if (novaQuantidade === 0) {
                            
                            location.reload();
                        } else {
                            
                            atualizarInterfaceCarrinho(produtoId, novaQuantidade, result);
                        }
                    } else {
                        mostrarMensagem('Erro ao atualizar quantidade: ' + result.mensagem, 'erro');
                    }
                } catch (error) {
                    console.error('Erro ao alterar quantidade:', error);
                    mostrarMensagem('Erro ao atualizar quantidade', 'erro');
                }
            }

            /**
             * 
             * @param {string} carrinhoId 
             * @param {string} produtoId 
             */
            async function removerProduto(carrinhoId, produtoId) {
                if (!confirm('Tem certeza que deseja remover este produto do carrinho?')) {
                    return;
                }

                try {
                    const token = document.querySelector('input[name="__RequestVerificationToken"]')?.value;

                    const response = await fetch('/Carrinho/RemoverProduto', {
                        method: 'POST',
                        headers: {
                            'Content-Type': 'application/x-www-form-urlencoded',
                        },
                        body: `carrinhoId=${carrinhoId}&produtoId=${produtoId}` +
                            (token ? `&__RequestVerificationToken=${token}` : '')
                    });

                    const result = await response.json();

                    if (result.sucesso) {
                        mostrarMensagem('Produto removido do carrinho', 'sucesso');

                        
                        if (!result.temItens) {
                            setTimeout(() => location.reload(), 1000);
                        } else {
                            
                            removerItemDaInterface(produtoId);
                            atualizarTotais(result.quantidadeItens, result.valorTotal);
                        }
                    } else {
                        mostrarMensagem('Erro ao remover produto: ' + result.mensagem, 'erro');
                    }
                } catch (error) {
                    console.error('Erro ao remover produto:', error);
                    mostrarMensagem('Erro ao remover produto', 'erro');
                }
            }

            /**
             * 
             * @param {string} produtoId 
             * @param {number} novaQuantidade 
             * @param {object} result 
             */
            function atualizarInterfaceCarrinho(produtoId, novaQuantidade, result) {
                
                const itemElement = document.querySelector(`[data-produto-id="${produtoId}"]`);
                if (itemElement) {
                    const quantidadeElement = itemElement.querySelector('.quantidade-valor');
                    if (quantidadeElement) {
                        quantidadeElement.textContent = novaQuantidade;
                    }

                    
                    const precoElement = itemElement.querySelector('.item-preco');
                    const subtotalElement = itemElement.querySelector('.item-subtotal');

                    if (precoElement && subtotalElement) {
                        const preco = parseFloat(precoElement.textContent.replace('R$', '').replace(',', '.'));
                        const subtotal = preco * novaQuantidade;
                        subtotalElement.textContent = `R$ ${subtotal.toFixed(2).replace('.', ',')}`;
                    }
                }

                
                atualizarTotais(result.quantidadeItens, result.valorTotal);
            }

            /**
             * 
             * @param {string} produtoId 
             */
            function removerItemDaInterface(produtoId) {
                const itemElement = document.querySelector(`[data-produto-id="${produtoId}"]`);
                if (itemElement) {
                    itemElement.style.animation = 'slideOutLeft 0.3s ease-in';
                    setTimeout(() => {
                        itemElement.remove();
                    }, 300);
                }
            }

            /**
             * 
             * @param {number} quantidade
             * @param {string} valorTotal 
             */
            function atualizarTotais(quantidade, valorTotal) {
                
                const resumoQuantidade = document.querySelector('#resumo-quantidade');
                const resumoTotal = document.querySelector('#resumo-total');

                if (resumoQuantidade) {
                    resumoQuantidade.textContent = quantidade;
                }

                if (resumoTotal) {
                    resumoTotal.textContent = valorTotal;
                }

                
                const carrinhoTitulo = document.querySelector('.carrinho-titulo');
                if (carrinhoTitulo) {
                    carrinhoTitulo
