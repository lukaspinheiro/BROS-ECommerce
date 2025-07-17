document.getElementById('libras').style.display = 'none';
$('#btnlibras').on('click', function () {
    if (document.getElementById('libras').style.display == 'none') {
        document.getElementById('libras').style.display = 'block';
    }
    else {
        document.getElementById('libras').style.display = 'none';
    }
});