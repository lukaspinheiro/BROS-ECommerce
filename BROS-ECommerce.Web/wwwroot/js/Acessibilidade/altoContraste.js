$(document)
    .scroll(function () {
        if ($(document).scrollTop() >= 60) {
            if (!$('header').hasClass('header-flutuante'))
                $('header').addClass('header-flutuante');

        } else {
            $('header').removeClass('header-flutuante');
        }
    });
function getCookie(cname) {
    let name = cname + "=";
    let ca = document.cookie.split(';');
    for (let i = 0; i < ca.length; i++) {
        let c = ca[i];
        while (c.charAt(0) === ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) === 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}
let tagExcluida;
let tagPai;
$('#altoContraste').on('click', function () {

    if (getCookie("altoContraste") == "true") {

        document.cookie = "altoContraste=; expires=Thu, 01 Jan 1970 00:00:00 UTC";
        $("body").removeClass("altoContraste");

        //adiciona as classes que foram removidas            
        $("div.cardheaderinfo").addClass("card-header-info");
        $("div").removeClass("cardheaderinfo");
        $("div.cardheadersuccess").addClass("card-header-success");
        $("div").removeClass("cardheadersuccess");
        $("a.textdark").addClass("text-dark");
        $("a.text-dark").removeClass("textdark");
        $("div.cardheaderprimary").addClass("card-header-primary");
        $("div").removeClass("cardheaderprimary");

        document.cookie = "permanecerAtivo=false"
        document.cookie = "altoContraste=false";

    } else {

        document.cookie = "altoContraste=true";
        document.cookie = "permanecerAtivo=true"
        $("body").addClass("altoContraste");

        //remover classes para ajustar alto contraste
        $("div.card-header-info").addClass("cardheaderinfo");
        $("div").removeClass("card-header-info");
        $("div.card-header-success").addClass("cardheadersuccess");
        $("div").removeClass("card-header-success");
        $("a.text-dark").addClass("textdark");
        $("a").removeClass("text-dark");
        $("div.card-header-primary").addClass("cardheaderprimary");
        $("div").removeClass("card-header-primary");

    }
});

//quando mudar de página verifica se o alto contraste está ativo e permanece
if (getCookie("permanecerAtivo") == "true") {
    document.cookie = "altoContraste=true";
    $("body").addClass("altoContraste");

    //remover classes para ajustar alto contraste
    $("div.card-header-info").addClass("cardheaderinfo");
    $("div").removeClass("card-header-info");
    $("div.card-header-success").addClass("cardheadersuccess");
    $("div").removeClass("card-header-success");
    $("a.text-dark").addClass("textdark");
    $("a").removeClass("text-dark");
    $("div.card-header-primary").addClass("cardheaderprimary");
    $("div").removeClass("card-header-primary");

}


//efeito mouseenter e mouseout nos card-img da pagina inicial
$('.list-group-item').on('mouseenter', function () {
    if (getCookie("permanecerAtivo") == "true") {
        this.setAttribute('style', 'background-color: #c3b7b7 !important;');
    }
});
$('.list-group-item').on('mouseout', function () {
    this.removeAttribute('style');
});

//efeito mouseenter e mouseout nos dopdown-item
$('.dropdown-item').on('mouseenter', function () {
    if (getCookie("permanecerAtivo") == "true") {
        this.setAttribute('style', 'background-color: #6d6c6c !important;');
    }
})
$('.dropdown-item').on('mouseout', function () {
    this.removeAttribute('style');
})

//troca a cor dos botões ao passar o mouse
$('.btn').on('mouseenter', function () {
    this.style.borderColor = '#808080';
});
$('.btn').on('mouseout', function () {
    this.removeAttribute('style');
});





