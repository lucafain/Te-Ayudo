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

let btnTogglePassword = document.getElementById('btnTogglePassword');
if (btnTogglePassword != null) {
    btnTogglePassword.addEventListener('click', function () {
        let inputPass = document.getElementById('Contrasena');
        if (inputPass != null) {
            inputPass.type = inputPass.type === 'password' ? 'text' : 'password';
        }
    });
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
        let email = document.getElementById('Email').value.trim();
        let contrasena = document.getElementById('Contrasena').value;

        if (email === '' || email.indexOf('@') === -1 || email.indexOf('.') === -1) {
            evento.preventDefault();
            mostrarErrorFormulario('Debe ingresar un correo electrónico válido.');
            return;
        }

        if (email.length > 50) {
            evento.preventDefault();
            mostrarErrorFormulario('El correo electrónico no puede superar los 50 caracteres.');
            return;
        }

        if (contrasena === '' || contrasena.length > 50) {
            evento.preventDefault();
            mostrarErrorFormulario('Debe ingresar una contraseña válida (máximo 50 caracteres).');
            return;
        }
    });
}

let formRegistro = document.getElementById('formRegistro');
if (formRegistro != null) {
    formRegistro.addEventListener('submit', function (evento) {
        let email = document.getElementById('Email').value.trim();
        let nombre = document.getElementById('Nombre').value.trim();
        let contrasena = document.getElementById('Contrasena').value;
        let inputTelefono = document.getElementById('Telefono');
        let telefono = inputTelefono != null ? inputTelefono.value.trim() : '';

        if (email === '' || email.indexOf('@') === -1 || email.indexOf('.') === -1) {
            evento.preventDefault();
            mostrarErrorFormulario('El correo electrónico es obligatorio y debe tener un formato válido.');
            return;
        }

        if (email.length > 50) {
            evento.preventDefault();
            mostrarErrorFormulario('El correo electrónico no puede superar los 50 caracteres.');
            return;
        }

        if (nombre === '') {
            evento.preventDefault();
            mostrarErrorFormulario('El nombre de usuario es obligatorio.');
            return;
        }

        if (nombre.length > 50) {
            evento.preventDefault();
            mostrarErrorFormulario('El nombre de usuario no puede superar los 50 caracteres.');
            return;
        }

        if (contrasena === '' || contrasena.length < 4) {
            evento.preventDefault();
            mostrarErrorFormulario('La contraseña es obligatoria y debe tener al menos 4 caracteres.');
            return;
        }

        if (contrasena.length > 50) {
            evento.preventDefault();
            mostrarErrorFormulario('La contraseña no puede superar los 50 caracteres.');
            return;
        }

        if (telefono !== '' && isNaN(Number(telefono.replace(/[-+ ]/g, '')))) {
            evento.preventDefault();
            mostrarErrorFormulario('El número telefónico solo puede contener números.');
            return;
        }
    });
}

let formTrabajar = document.getElementById('formTrabajar');
if (formTrabajar != null) {
    formTrabajar.addEventListener('submit', function (evento) {
        let nombre = document.getElementById('Nombre').value.trim();
        let apellido = document.getElementById('Apellido').value.trim();
        let email = document.getElementById('Email').value.trim();
        let contrasena = document.getElementById('Contrasena').value;
        let provincia = document.getElementById('Provincia').value.trim();
        let calle = document.getElementById('Calle').value.trim();
        let direccion = document.getElementById('Direccion').value.trim();
        let codigoPostal = document.getElementById('CodigoPostal').value.trim();
        let acepta = document.getElementById('AceptaTerminos').checked;

        if (nombre === '' || apellido === '') {
            evento.preventDefault();
            mostrarErrorFormulario('Debe completar su nombre y apellido.');
            return;
        }

        if (nombre.length > 50 || apellido.length > 50) {
            evento.preventDefault();
            mostrarErrorFormulario('El nombre y el apellido no pueden superar los 50 caracteres.');
            return;
        }

        if (email === '' || email.indexOf('@') === -1 || email.indexOf('.') === -1 || email.length > 50) {
            evento.preventDefault();
            mostrarErrorFormulario('Debe ingresar un correo electrónico válido (máximo 50 caracteres).');
            return;
        }

        if (contrasena === '' || contrasena.length < 4 || contrasena.length > 50) {
            evento.preventDefault();
            mostrarErrorFormulario('La contraseña debe tener entre 4 y 50 caracteres.');
            return;
        }

        if (provincia === '' || provincia.length > 50) {
            evento.preventDefault();
            mostrarErrorFormulario('Debe ingresar su provincia (máximo 50 caracteres).');
            return;
        }

        if (calle === '' || calle.length > 100) {
            evento.preventDefault();
            mostrarErrorFormulario('Debe ingresar su calle (máximo 100 caracteres).');
            return;
        }

        if (direccion === '' || isNaN(Number(direccion)) || Number(direccion) < 0) {
            evento.preventDefault();
            mostrarErrorFormulario('Debe ingresar una altura/número de dirección válido.');
            return;
        }

        if (codigoPostal === '' || codigoPostal.length > 8) {
            evento.preventDefault();
            mostrarErrorFormulario('Debe ingresar un código postal válido (máximo 8 caracteres).');
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
        let email = document.getElementById('Email').value.trim();
        if (email === '' || email.indexOf('@') === -1 || email.length > 50) {
            evento.preventDefault();
            mostrarErrorFormulario('Debe ingresar un correo electrónico válido para buscar su contraseña.');
            return;
        }
    });
}

let formSolicitar = document.getElementById('formSolicitar');
if (formSolicitar != null) {
    formSolicitar.addEventListener('submit', function (evento) {
        let titulo = document.getElementById('Titulo').value.trim();
        let descripcion = document.getElementById('Descripcion').value.trim();
        let lugar = document.getElementById('Lugar').value.trim();
        let diaHora = document.getElementById('DiaHora').value.trim();

        if (titulo === '') {
            evento.preventDefault();
            mostrarErrorFormulario('Por favor ingresá el nombre de la ayuda que necesitás.');
            return;
        }

        if (titulo.length > 50) {
            evento.preventDefault();
            mostrarErrorFormulario('El nombre de la ayuda no puede superar los 50 caracteres.');
            return;
        }

        if (descripcion === '') {
            evento.preventDefault();
            mostrarErrorFormulario('Por favor completá la descripción de la ayuda.');
            return;
        }

        if (descripcion.length > 50) {
            evento.preventDefault();
            mostrarErrorFormulario('La descripción no puede superar los 50 caracteres.');
            return;
        }

        if (lugar === '') {
            evento.preventDefault();
            mostrarErrorFormulario('Por favor indicá el lugar donde necesitás la ayuda.');
            return;
        }

        if (lugar.length > 50) {
            evento.preventDefault();
            mostrarErrorFormulario('El lugar no puede superar los 50 caracteres.');
            return;
        }

        if (diaHora === '') {
            evento.preventDefault();
            mostrarErrorFormulario('Por favor seleccioná el día y la hora en el calendario.');
            return;
        }

        let fechaElegida = new Date(diaHora);
        if (isNaN(fechaElegida.getTime())) {
            evento.preventDefault();
            mostrarErrorFormulario('La fecha y hora elegidas no son válidas.');
            return;
        }

        if (fechaElegida < new Date()) {
            evento.preventDefault();
            mostrarErrorFormulario('La fecha y hora de la ayuda no pueden ser anteriores a hoy.');
            return;
        }

        let aclaracion = document.getElementById('Aclaracion').value.trim();
        if (aclaracion.length > 500) {
            evento.preventDefault();
            mostrarErrorFormulario('Las aclaraciones no pueden superar los 500 caracteres.');
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
                let email = datos.email || datos.Email || 'No especificado';
                let provincia = datos.provincia || datos.Provincia || '';
                let calle = datos.calle || datos.Calle || '';
                let direccion = datos.direccion || datos.Direccion || '';
                let tipoBool = datos.tipoUsuario !== undefined ? datos.tipoUsuario : datos.TipoUsuario;
                let tipoTexto = tipoBool ? 'Joven' : 'Adulto mayor';

                let nombreCompleto = (nombre + ' ' + apellido).trim();
                if (nombreCompleto === '') {
                    nombreCompleto = email;
                }

                document.getElementById('detalleNombre').textContent = 'Información de ' + nombreCompleto;
                document.getElementById('detalleEmail').textContent = 'Email: ' + email;
                document.getElementById('detalleTelefono').textContent = 'Ubicación: ' + calle + ' ' + direccion + ' (' + provincia + ')';
                document.getElementById('detalleTipo').textContent = 'Tipo de usuario: ' + tipoTexto;

                contenedor.style.display = 'block';
            }
        })
        .catch(function (error) {
            console.error('Error en fetch:', error);
        });
}
