export function verificarEspacos() {
    const form = document.getElementById("form-cadastrar-usuario");
    const nome = form.querySelector("#Nome");
    const email = form.querySelector("#Email");
    const cpf = form.querySelector("#Cpf");
    const genero = form.querySelector("#Genero");
    const nascimento = form.querySelector("#Nascimento");

    // Para cadastro, também validar senha
    const senha = form.querySelector("#Senha");
    const isEdicao = form.querySelector("#usuario-id-edicao").value !== '';

    let camposObrigatorios = [nome, email, cpf, genero, nascimento];

    // Se for cadastro (não edição), incluir senha na validação
    if (!isEdicao && senha) {
        camposObrigatorios.push(senha);
    }

    const todosValidos = camposObrigatorios.every(campo =>
        campo && campo.value.trim().length > 0
    );
    document.getElementById("btn-disable").disabled = !todosValidos;
}

export function limparFormulario() {
    document.getElementById('form-cadastrar-usuario').reset();
    document.getElementById('usuario-id-edicao').value = '';
    document.querySelector('#modal-cadastrar-usuario .modal-title').textContent = 'Cadastrar Usuário';
    document.querySelector('#modal-cadastrar-usuario .btn-success').textContent = 'ADICIONAR';
    document.getElementById('form-cadastrar-usuario').action = '/Administrativo/Usuario/CadastrarUsuario';

    // Mostrar campo senha para cadastro
    const senhaGroup = document.querySelector('.senha-group');
    if (senhaGroup) {
        senhaGroup.style.display = 'block';
    }

    verificarEspacos();
    window.usuarioEditandoId = null;
}