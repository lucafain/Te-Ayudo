let menu = document.querySelector('.menu-toggle');
let navigation = document.querySelector('#navigation');

if (menu != null && navigation != null) {
    menu.addEventListener('click', function () {
        let abierto = menu.getAttribute('aria-expanded') !== 'true';
        menu.setAttribute('aria-expanded', String(abierto));
        menu.setAttribute('aria-label', abierto ? 'Cerrar menú' : 'Abrir menú');
        navigation.classList.toggle('open', abierto);
    });

    let enlaces = navigation.querySelectorAll('a');
    for (let i = 0; i < enlaces.length; i++) {
        enlaces[i].addEventListener('click', function () {
            navigation.classList.remove('open');
            menu.setAttribute('aria-expanded', 'false');
            menu.setAttribute('aria-label', 'Abrir menú');
        });
    }
}

let year = document.getElementById('year');
if (year != null) {
    year.textContent = new Date().getFullYear();
}

let inputsArchivo = document.querySelectorAll('input[type="file"]');
for (let i = 0; i < inputsArchivo.length; i++) {
    let input = inputsArchivo[i];
    let fileName = document.querySelector('[data-file-name="' + input.id + '"]');
    if (fileName != null) {
        input.addEventListener('change', function () {
            if (input.files && input.files.length > 0) {
                fileName.textContent = input.files[0].name;
            } else {
                fileName.textContent = 'Ningún archivo seleccionado';
            }
        });
    }
}

function mostrarErrorFormulario(motivo) {
    let cajaError = document.getElementById('mensajeErrorJs');
    if (cajaError != null) {
        cajaError.textContent = motivo;
        cajaError.style.display = 'block';
    } else {
        alert(motivo);
    }
}

let formLogin = document.getElementById('formLogin');
if (formLogin != null) {
    formLogin.addEventListener('submit', function (evento) {
        let email = document.getElementById('Email').value;
        let contrasena = document.getElementById('Contrasena').value;

        if (email === '' || email.indexOf('@') === -1) {
            evento.preventDefault();
            mostrarErrorFormulario('Debe ingresar un correo electrónico válido.');
            return;
        }

        if (contrasena === '') {
            evento.preventDefault();
            mostrarErrorFormulario('Debe ingresar su contraseña.');
            return;
        }
    });
}

let formRegistro = document.getElementById('formRegistro');
if (formRegistro != null) {
    formRegistro.addEventListener('submit', function (evento) {
        let email = document.getElementById('Email').value;
        let nombreUsuario = document.getElementById('NombreUsuario').value;
        let contrasena = document.getElementById('Contrasena').value;

        if (email === '' || email.indexOf('@') === -1) {
            evento.preventDefault();
            mostrarErrorFormulario('El correo electrónico es obligatorio y debe ser válido.');
            return;
        }

        if (nombreUsuario === '') {
            evento.preventDefault();
            mostrarErrorFormulario('El nombre de usuario es obligatorio.');
            return;
        }

        if (contrasena === '' || contrasena.length < 4) {
            evento.preventDefault();
            mostrarErrorFormulario('La contraseña es obligatoria y debe tener al menos 4 caracteres.');
            return;
        }
    });
}

let formTrabajar = document.getElementById('formTrabajar');
if (formTrabajar != null) {
    formTrabajar.addEventListener('submit', function (evento) {
        let email = document.getElementById('Email').value;
        let ambito = document.getElementById('AmbitoLaboral').value;
        let contrasena = document.getElementById('Contrasena').value;
        let acepta = document.getElementById('AceptaTerminos').checked;

        if (email === '' || email.indexOf('@') === -1) {
            evento.preventDefault();
            mostrarErrorFormulario('Debe ingresar un correo electrónico válido.');
            return;
        }

        if (ambito === '') {
            evento.preventDefault();
            mostrarErrorFormulario('Debe completar el ámbito laboral al que apunta.');
            return;
        }

        if (contrasena === '' || contrasena.length < 4) {
            evento.preventDefault();
            mostrarErrorFormulario('Debe ingresar una contraseña de al menos 4 caracteres.');
            return;
        }

        if (!acepta) {
            evento.preventDefault();
            mostrarErrorFormulario('Debe aceptar los términos y condiciones para registrarse.');
            return;
        }
    });
}

let formOlvide = document.getElementById('formOlvide');
if (formOlvide != null) {
    formOlvide.addEventListener('submit', function (evento) {
        let email = document.getElementById('Email').value;
        if (email === '' || email.indexOf('@') === -1) {
            evento.preventDefault();
            mostrarErrorFormulario('Debe ingresar un correo electrónico válido para buscar su contraseña.');
            return;
        }
    });
}

let formSolicitar = document.getElementById('formSolicitar');
if (formSolicitar != null) {
    formSolicitar.addEventListener('submit', function (evento) {
        let titulo = document.getElementById('Titulo').value;
        let descripcion = document.getElementById('Descripcion').value;

        if (titulo === '' || descripcion === '') {
            evento.preventDefault();
            mostrarErrorFormulario('Por favor complete el título y la descripción de la ayuda que necesita.');
            return;
        }
    });
}

// Petición server-side FETCH (Slides 142-146 del PDF)
function verInfoAyudado(idUsuario) {
    fetch('/Home/VerDetalleAyudado?idUsuario=' + idUsuario)
        .then(function (response) {
            return response.json();
        })
        .then(function (datos) {
            if (datos != null) {
                let contenedor = document.getElementById('contenedorDetalleFetch');
                let nombre = datos.nombre || datos.Nombre || '';
                let apellido = datos.apellido || datos.Apellido || '';
                let nombreUsuario = datos.nombreUsuario || datos.NombreUsuario || '';
                let email = datos.email || datos.Email || 'No especificado';
                let telefono = datos.telefono || datos.Telefono || 'No especificado';
                let tipoUsuario = datos.tipoUsuario || datos.TipoUsuario || 'Adulto mayor';

                let nombreCompleto = (nombre + ' ' + apellido).trim();
                if (nombreCompleto === '') {
                    nombreCompleto = nombreUsuario || 'Adulto mayor';
                }

                document.getElementById('detalleNombre').textContent = 'Información de ' + nombreCompleto;
                document.getElementById('detalleEmail').textContent = 'Email: ' + email;
                document.getElementById('detalleTelefono').textContent = 'Teléfono: ' + telefono;
                document.getElementById('detalleTipo').textContent = 'Tipo de usuario: ' + tipoUsuario;

                contenedor.style.display = 'block';
            }
        })
        .catch(function (error) {
            console.error('Error en fetch:', error);
        });
}
