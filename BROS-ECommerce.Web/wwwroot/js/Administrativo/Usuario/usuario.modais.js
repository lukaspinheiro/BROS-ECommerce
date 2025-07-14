import { limparFormulario, verificarEspacos } from './usuario.formulario.js';

export function abrirModalCadastro() {
    limparFormulario();
    document.querySelector('#modal-cadastrar-usuario .modal-title').textContent = 'Cadastrar Usuário';
    document.querySelector('#modal-cadastrar-usuario .btn-success').textContent = 'ADICIONAR';
    document.getElementById('form-cadastrar-usuario').action = '/Administrativo/Usuario/Cadastrar';
    document.getElementById('usuario-id-edicao').value = '';

    // Mostrar campo senha para cadastro
    const senhaGroup = document.querySelector('.senha-group');
    if (senhaGroup) {
        senhaGroup.style.display = 'block';
    }

    $('#modal-cadastrar-usuario').modal('show');
}

export function editarUsuario(id, nome, email, cpf, genero, nascimento, ativo) {
    window.usuarioEditandoId = id;
    document.getElementById('Nome').value = nome;
    document.getElementById('Email').value = email;
    document.getElementById('Cpf').value = cpf;
    document.getElementById('Genero').value = genero;
    document.getElementById('Nascimento').value = nascimento;
    document.getElementById('usuario-id-edicao').value = id;

    // Proteção: só preenche se o campo "Ativo" existir
    const ativoField = document.getElementById('Ativo');
    if (ativoField) {
        ativoField.value = ativo; // ou .checked = ativo === 'True', se for checkbox
    }

    document.querySelector('#modal-cadastrar-usuario .modal-title').textContent = 'Editar Usuário';
    document.querySelector('#modal-cadastrar-usuario .btn-success').textContent = 'ATUALIZAR';
    document.getElementById('form-cadastrar-usuario').action = '/Administrativo/Usuario/Editar';

    // Esconde o campo de senha ao editar
    const senhaGroup = document.querySelector('.senha-group');
    const senhaInput = document.getElementById('Senha');

    if (senhaGroup && senhaInput) {
        senhaGroup.style.display = 'none';
        senhaInput.removeAttribute('required'); // 👈 Remove o required
    }


    document.getElementById('btn-disable').disabled = false;
    $('#modal-cadastrar-usuario').modal('show');
}


export function confirmarExclusao(id, nome) {
    document.getElementById('usuario-excluir-nome').textContent = nome;
    document.getElementById('btn-confirmar-exclusao').setAttribute('data-id', id);
    $('#modal-confirmar-exclusao').modal('show');
}

export function fecharModalCadastrar() {
    $('#modal-cadastrar-usuario').modal('hide');
    limparFormulario();
}

export function fecharModalExclusao() {
    $('#modal-confirmar-exclusao').modal('hide');
}