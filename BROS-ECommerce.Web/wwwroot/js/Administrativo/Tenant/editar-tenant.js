document.addEventListener("DOMContentLoaded", function () {

    $(document).on("click", ".btn-editar", function () {

        const id = $(this).data("id");
        const nome = $(this).data("nome");
        const email = $(this).data("email");
        const telefone = $(this).data("telefone");
        let ativoRaw = $(this).data("ativo");
        const ativoBool = (function(v){
            if (v === true || v === 1) return true;
            if (v === false || v === 0) return false;
            if (typeof v === "string") {
                const s = v.trim().toLowerCase();
                if (s === "1" || s === "true" || s === "yes" ) return true;
                if (s === "0" || s === "false" || s === "no") return false;
            }
            return false;
        })(ativoRaw);

        $("#tenant-id-original").val(id); 
        $("#tenant-dominio").val(id);
        $("#tenant-nome").val(nome);
        $("#tenant-email").val(email);
        $("#tenant-telefone").val(telefone);
        const selectAtivo = $("select[name='CadastrarTenant.Ativo']");
        if (selectAtivo.length) {
            selectAtivo.val(ativoBool ? "true" : "false").trigger("change");
        } else {
            const checkbox = $("#tenant-ativo");
            if (checkbox.length && (checkbox.attr("type") === "checkbox")) {
                checkbox.prop("checked", ativoBool);
            }
        }
        $("#modal-editar-tenant .modal-title").text("Editar Tenant");
        $("#btn-disable").text("SALVAR");
        $("#form-cadastrar-tenant")
            .attr("action", "/Administrativo/Tenant/EditarTenant");

    });

});