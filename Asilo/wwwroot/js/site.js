// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {
    $('.seccion').show(); // Oculta todas las secciones al principio

    $('#secciones').change(function () { // Detecta cambios en el select
        var seccionSeleccionada = $(this).val(); // Obtiene la opción seleccionada

        if (seccionSeleccionada == 'todas') { // Si se selecciona "todas"
            $('.seccion').show(); // Muestra todas las secciones
        } else { // Si se selecciona una sección específica
            $('.seccion').hide(); // Oculta todas las secciones
            $('.' + seccionSeleccionada).show(); // Muestra la sección correspondiente
        }
    });
});

var preview = document.querySelector('#image-preview');
var fileInput = document.querySelector('input[type="file"]');
fileInput.addEventListener('change', function (e) {
    for (var i = 0; i < e.target.files.length; i++) {
        var file = e.target.files[i];
        var reader = new FileReader();
        reader.onload = function (e) {
            var container = document.createElement('div');
            var img = document.createElement('img');
            img.src = e.target.result;
            img.width = 200;
            img.height = 200;

            container.appendChild(img);
            var button = document.createElement('button');
            button.textContent = 'Eliminar';
            button.className = 'btn btn-md red-plate mt-2 mb-2';
            button.style.display = 'flex';
            button.addEventListener('click', function () {
                preview.removeChild(container);
            });
            container.appendChild(button);

            // Agregar un campo oculto para enviar la imagen
            var hiddenInput = document.createElement('input');
            hiddenInput.type = 'hidden';
            hiddenInput.name = 'ImagePreviews';
            hiddenInput.value = e.target.result;
            container.appendChild(hiddenInput);

            preview.appendChild(container);
        };
        reader.readAsDataURL(file);
    }
    fileInput.value = '';
});