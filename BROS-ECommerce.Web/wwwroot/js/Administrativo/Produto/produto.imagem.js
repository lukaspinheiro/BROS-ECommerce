import { verificarEspacos, marcarTentativaEnvio } from './produto.formulario.js';

let imagensSelecionadas = [];

export function adicionarImagens(files) {
    imagensSelecionadas.push(...files);
    document.getElementById('Imagens').value = '';
    atualizarPreview();
    verificarEspacos();
    const indicePrincipal = document.getElementById('IndiceImagemPrincipal');
    const imagemPrincipalErro = document.getElementById('imagem-principal-error');

    if (imagemPrincipalErro) {
        imagemPrincipalErro.style.display = imagensSelecionadas.length > 0 && !indicePrincipal.value ? "block" : "none";
    }

}

function atualizarPreview() {
    const preview = document.getElementById('preview-imagens');
    const nomesArquivos = document.getElementById('nomes-arquivos');
    const indicePrincipal = document.getElementById('IndiceImagemPrincipal');

    preview.innerHTML = '';
    nomesArquivos.value = imagensSelecionadas.map(f => f.name).join(', ');

    imagensSelecionadas.forEach((file, i) => {
        const reader = new FileReader();
        reader.onload = e => {
            const container = document.createElement('div');
            container.className = 'position-relative d-inline-block m-1';

            const img = document.createElement('img');
            img.src = e.target.result;
            img.alt = file.name;
            img.className = 'img-thumbnail';
            img.style.cursor = 'pointer';
            img.style.maxHeight = '100px';
            img.dataset.index = i;

            img.onclick = () => {
                preview.querySelectorAll('img').forEach(im => im.classList.remove('border-primary'));
                img.classList.add('border-primary');
                indicePrincipal.value = i.toString();
                verificarEspacos();
            };

            const btnRemover = document.createElement('button');
            btnRemover.type = 'button';
            btnRemover.className = 'btn btn-sm btn-danger position-absolute top-0 end-0 rounded-circle p-1';
            btnRemover.style.zIndex = '10';
            btnRemover.style.width = '22px';
            btnRemover.style.height = '22px';
            btnRemover.style.fontSize = '16px';
            btnRemover.style.lineHeight = '1';
            btnRemover.innerHTML = '&times;';
            btnRemover.title = 'Remover imagem';
            btnRemover.onclick = () => {
                const indicePrincipal = document.getElementById('IndiceImagemPrincipal');

                const removendoPrincipal = indicePrincipal.value === i.toString();
                if (removendoPrincipal) {
                    indicePrincipal.value = '';
                }

                imagensSelecionadas.splice(i, 1);
                atualizarPreview();
                verificarEspacos(); 
            };




            container.appendChild(img);
            container.appendChild(btnRemover);
            preview.appendChild(container);
        };
        reader.readAsDataURL(file);
    });
}

export function inicializarImagemProduto() {
    const form = document.getElementById('form-cadastrar-produto');
    window.adicionarImagens = adicionarImagens;

    form.addEventListener('submit', e => {
        e.preventDefault();

        marcarTentativaEnvio();

        const indicePrincipal = document.getElementById('IndiceImagemPrincipal');
        if (!indicePrincipal.value) {
            verificarEspacos();
            return;
        }

        const formData = new FormData(form);
        formData.delete('cadastrarProdutoViewModel.Arquivos');
        imagensSelecionadas.forEach(file => formData.append('cadastrarProdutoViewModel.Arquivos', file));

        fetch(form.action, {
            method: form.method,
            body: formData
        })
            .then(res => res.ok ? location.reload() : alert('Erro ao enviar o formulário'))
            .catch(() => alert('Erro ao enviar o formulário'));
    });
}
