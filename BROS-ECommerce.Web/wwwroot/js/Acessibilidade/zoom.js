$(document).ready(function () {

    $("#zoomMaior").click(function () {
        if (parseFloat($("header .bg-light").css('font-size')) < 21)
            changeFontSize(1);
    });

    $("#zoomMenor").click(function () {
        if (parseFloat($("header .bg-light").css('font-size')) > 15)
            changeFontSize(2);
    });

    $("#zoomReset").click(function () {
        if (parseFloat($("header .bg-light").css('font-size')) != 18)
            changeFontSize(3);

    });
});

function changeFontSize(modeZoom) {
    $('div h1, div h2, div h3, div h4, div h5, div h6, div p, div span, div a, div').each(function (i, v) {        
        switch (modeZoom) {
            case 1:
                $(this).css('font-size', (parseFloat($(this).css('font-size')) + (0.6)) + 'px');
                break;
        
            case 2:
                $(this).css('font-size', (parseFloat($(this).css('font-size')) - (0.6)) + 'px');
                break;

            case 3:
                $(this).css('font-size', "");
                break;
        }
    });
}