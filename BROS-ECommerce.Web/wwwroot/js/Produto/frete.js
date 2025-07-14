document.addEventListener("DOMContentLoaded", function () {
    const cepInput = document.getElementById("cepInput");
    const form = document.getElementById("freteForm");
    const logradouro = document.getElementById("logradouro");
    const bairro = document.getElementById("bairro");
    const localidade = document.getElementById("localidade");
    const uf = document.getElementById("uf");
    const cepFormatado = document.getElementById("cepFormatado");
    const enderecoInfo = document.getElementById("enderecoInfo");

    form.addEventListener("submit", async function (e) {
        e.preventDefault();
        let cep = cepInput.value.replace(/\D/g, "");

        if (cep.length !== 8) {
            alert("Por favor, insira um CEP válido com 8 dígitos.");
            return;
        }

        try {
            const response = await fetch(`https://viacep.com.br/ws/${cep}/json/`);
            const data = await response.json();

            if (data.erro) {
                alert("CEP não encontrado.");
                return;
            }

            logradouro.textContent = data.logradouro || "Não informado";
            bairro.textContent = data.bairro || "Não informado";
            localidade.textContent = data.localidade || "Não informado";
            uf.textContent = data.uf || "Não informado";
            cepFormatado.textContent = data.cep;

            enderecoInfo.style.display = "block";
        } catch (error) {
            console.error("Erro ao buscar CEP:", error);
            alert("Erro ao consultar o CEP. Tente novamente.");
        }
    });
});
