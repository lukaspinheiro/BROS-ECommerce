function desabilitarBotao() {
    document.getElementById('btn-disable').disabled = true;
}

function desabilitarBotoesEmPartials(form) {
    const botoes = form.querySelectorAll('.btn-disable');
    botoes.forEach(botao => botao.disabled = true);
}
