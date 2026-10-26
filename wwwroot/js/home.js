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

        if (descripcion.length > 500) {
            evento.preventDefault();
            mostrarErrorFormulario('La descripción no puede superar los 500 caracteres.');
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

let formModificarTarea = document.getElementById('formModificarTarea');
if (formModificarTarea != null) {
    formModificarTarea.addEventListener('submit', function (evento) {
        let titulo = document.getElementById('Titulo').value.trim();
        let descripcion = document.getElementById('Descripcion').value.trim();
        let lugar = document.getElementById('Lugar').value.trim();
        let diaHora = document.getElementById('DiaHora').value.trim();
        let confirmacion = document.getElementById('ConfirmacionModificar').value.trim().toLowerCase();

        if (titulo === '' || descripcion === '' || lugar === '' || diaHora === '') {
            evento.preventDefault();
            mostrarErrorFormulario('Por favor completá el título, la descripción, el lugar y el día/hora.');
            return;
        }

        if (confirmacion !== 'modificar') {
            evento.preventDefault();
            mostrarErrorFormulario('Debés escribir exactamente "modificar" abajo de todo para confirmar los cambios.');
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


// Petición FETCH para cargar los datos del detalle de la ayuda (DetalleAyuda.cshtml)
let contenedorDetalleAyuda = document.getElementById('detalleAyudaFetchContainer');
if (contenedorDetalleAyuda != null) {
    let idTarea = contenedorDetalleAyuda.getAttribute('data-id-tarea');
    if (idTarea != null && idTarea !== '0') {
        fetch('/Home/ObtenerDetalleTareaJson?idTarea=' + idTarea)
            .then(function (response) {
                return response.json();
            })
            .then(function (datos) {
                if (datos != null) {
                    let elSolicitante = document.getElementById('detalleTareaSolicitante');
                    let elTitulo = document.getElementById('detalleTareaTitulo');
                    let elLugar = document.getElementById('detalleTareaLugar');
                    let elFechaHora = document.getElementById('detalleTareaFechaHora');
                    let elDescripcion = document.getElementById('detalleTareaDescripcion');
                    let elFoto = document.getElementById('detalleTareaFoto');

                    if (elSolicitante != null && datos.nombreSolicitante) {
                        elSolicitante.textContent = datos.nombreSolicitante;
                    }
                    if (elTitulo != null && datos.titulo) {
                        elTitulo.textContent = datos.titulo;
                    }
                    if (elLugar != null && datos.lugar) {
                        elLugar.textContent = datos.lugar;
                    }
                    if (elFechaHora != null && datos.fecha) {
                        elFechaHora.textContent = datos.fecha + ' ' + (datos.hora || '') + ' hs';
                    }
                    if (elDescripcion != null) {
                        let textoDesc = datos.descripcion || '';
                        if (datos.aclaracion && datos.aclaracion !== '') {
                            textoDesc = textoDesc + ' · ' + datos.aclaracion;
                        }
                        elDescripcion.textContent = textoDesc;
                    }
                    if (elFoto != null && datos.fotoSolicitante) {
                        elFoto.src = datos.fotoSolicitante;
                    }
                }
            })
            .catch(function (error) {
                console.error('Error al cargar detalle de ayuda con fetch:', error);
            });
    }
}


// Snackbar de 3 segundos para rechazar tarea con opción de Deshacer
let timerRechazo = null;
let idTareaPendienteRechazo = null;

function confirmarRechazoServidor(idTarea) {
    fetch('/Home/RechazarTarea?idTarea=' + idTarea, { method: 'POST' })
        .catch(function (error) {
            console.error('Error al rechazar tarea:', error);
        });
}

function rechazarTareaConUndo(idTarea) {
    if (timerRechazo != null && idTareaPendienteRechazo != null) {
        clearTimeout(timerRechazo);
        confirmarRechazoServidor(idTareaPendienteRechazo);
    }

    idTareaPendienteRechazo = idTarea;
    let card = document.getElementById('card-tarea-' + idTarea);
    if (card != null) {
        card.style.display = 'none';
    }

    let snackbar = document.getElementById('snackbarRechazo');
    if (snackbar != null) {
        snackbar.style.display = 'flex';
    }

    timerRechazo = setTimeout(function () {
        if (snackbar != null) {
            snackbar.style.display = 'none';
        }
        if (idTareaPendienteRechazo != null) {
            confirmarRechazoServidor(idTareaPendienteRechazo);
            idTareaPendienteRechazo = null;
        }
        timerRechazo = null;
    }, 3000);
}

function deshacerRechazoTarea() {
    if (timerRechazo != null) {
        clearTimeout(timerRechazo);
        timerRechazo = null;
    }

    if (idTareaPendienteRechazo != null) {
        let card = document.getElementById('card-tarea-' + idTareaPendienteRechazo);
        if (card != null) {
            card.style.display = 'flex';
        }
        idTareaPendienteRechazo = null;
    }

    let snackbar = document.getElementById('snackbarRechazo');
    if (snackbar != null) {
        snackbar.style.display = 'none';
    }
}


// Flujo FETCH en AyudasParaRealizar (Ver más -> Detalle -> Postularse -> Postulados)
let idTareaSeleccionadaFetch = 0;

function abrirDetalleTareaFetch(idTarea) {
    idTareaSeleccionadaFetch = idTarea;
    fetch('/Home/ObtenerDetalleTareaJson?idTarea=' + idTarea)
        .then(function (response) {
            return response.json();
        })
        .then(function (datos) {
            if (datos != null) {
                document.getElementById('detalleTareaSolicitante').textContent = datos.nombreSolicitante || 'Adulto mayor';
                document.getElementById('detalleTareaTitulo').textContent = datos.titulo || '';
                document.getElementById('detalleTareaLugar').textContent = datos.lugar || '';
                document.getElementById('detalleTareaFechaHora').textContent = (datos.fecha || '') + ' ' + (datos.hora || '') + ' hs';

                let desc = datos.descripcion || '';
                if (datos.aclaracion && datos.aclaracion !== '') {
                    desc = desc + ' · ' + datos.aclaracion;
                }
                document.getElementById('detalleTareaDescripcion').textContent = desc;

                let elFoto = document.getElementById('detalleTareaFoto');
                if (elFoto != null && datos.fotoSolicitante) {
                    elFoto.src = datos.fotoSolicitante;
                }

                document.getElementById('vistaGrillaAyudas').style.display = 'none';
                document.getElementById('vistaPostuladosAyuda').style.display = 'none';
                document.getElementById('vistaDetalleAyuda').style.display = 'block';
            }
        })
        .catch(function (error) {
            console.error('Error en abrirDetalleTareaFetch:', error);
        });
}

function volverAGrillaAyudas() {
    document.getElementById('vistaDetalleAyuda').style.display = 'none';
    document.getElementById('vistaPostuladosAyuda').style.display = 'none';
    document.getElementById('vistaGrillaAyudas').style.display = 'block';
}

function postularmeConFetch() {
    if (idTareaSeleccionadaFetch <= 0) {
        return;
    }

    fetch('/Home/PostularseJson?idTarea=' + idTareaSeleccionadaFetch, { method: 'POST' })
        .then(function (response) {
            return response.json();
        })
        .then(function (datos) {
            if (datos != null) {
                let titulo = datos.tituloTarea || 'esta ayuda';
                document.getElementById('tituloPostuladosFetch').textContent = '¡Ya estás postulado para ' + titulo + '!';

                let grilla = document.getElementById('grillaPostuladosFetch');
                grilla.innerHTML = '';

                let lista = datos.postulados || [];
                for (let i = 0; i < lista.length; i++) {
                    let u = lista[i];
                    let idU = u.idUsuario || u.IdUsuario || 1;
                    let nom = ((u.nombre || u.Nombre || '') + ' ' + (u.apellido || u.Apellido || '')).trim();
                    if (nom === '') {
                        nom = u.email || u.Email || 'Joven voluntario';
                    }
                    let foto = u.fotoPerfil || u.FotoPerfil || '/img/user.svg';

                    let card = document.createElement('article');
                    card.className = 'postulado-card';
                    card.innerHTML =
                        '<img src="' + foto + '" alt="' + nom + '" class="postulado-avatar" />' +
                        '<div class="postulado-info">' +
                        '<h3>' + nom + '</h3>' +
                        '<button type="button" class="btn-ayuda-verde" onclick="verInfoAyudado(' + idU + ')">Ver más</button>' +
                        '</div>';
                    grilla.appendChild(card);
                }

                document.getElementById('vistaGrillaAyudas').style.display = 'none';
                document.getElementById('vistaDetalleAyuda').style.display = 'none';
                document.getElementById('vistaPostuladosAyuda').style.display = 'block';
            }
        })
        .catch(function (error) {
            console.error('Error al postularse con fetch:', error);
        });
}

// Selector personalizado de Fecha y Hora (SolicitarTarea.cshtml)
let dtDiaSeleccionado = new Date().getDate();
let dtEsPm = false;

function abrirSelectorFechaHora() {
    let popup = document.getElementById('customDateTimePopup');
    if (popup != null) {
        let ahora = new Date();
        let selectMes = document.getElementById('dtMonthSelect');
        let selectAnio = document.getElementById('dtYearSelect');
        if (selectMes != null && popup.getAttribute('data-init') !== 'true') {
            selectMes.value = String(ahora.getMonth());
            if (selectAnio != null) {
                selectAnio.value = String(ahora.getFullYear());
            }
            dtDiaSeleccionado = ahora.getDate();
            popup.setAttribute('data-init', 'true');
            renderizarCalendario();
        }
        popup.style.display = 'flex';
    }
}

function cerrarSelectorFechaHora() {
    let popup = document.getElementById('customDateTimePopup');
    if (popup != null) {
        popup.style.display = 'none';
    }
}

function alternarAmPm() {
    dtEsPm = !dtEsPm;
    let btn = document.getElementById('dtAmPmBtn');
    if (btn != null) {
        btn.textContent = dtEsPm ? 'PM' : 'AM';
    }
}

function seleccionarDiaCalendario(dia) {
    dtDiaSeleccionado = dia;
    renderizarCalendario();
}

function renderizarCalendario() {
    let grid = document.getElementById('dtDaysGrid');
    let selectMes = document.getElementById('dtMonthSelect');
    let selectAnio = document.getElementById('dtYearSelect');
    if (grid == null || selectMes == null || selectAnio == null) {
        return;
    }

    let mes = Number(selectMes.value);
    let anio = Number(selectAnio.value);
    let primerDiaSemana = new Date(anio, mes, 1).getDay();
    let offsetLunes = (primerDiaSemana + 6) % 7;
    let totalDiasMes = new Date(anio, mes + 1, 0).getDate();
    let diasMesAnterior = new Date(anio, mes, 0).getDate();

    grid.innerHTML = '';

    for (let i = offsetLunes - 1; i >= 0; i--) {
        let spanPrev = document.createElement('span');
        spanPrev.className = 'dt-day dt-day--muted';
        spanPrev.textContent = String(diasMesAnterior - i);
        grid.appendChild(spanPrev);
    }

    for (let d = 1; d <= totalDiasMes; d++) {
        let btnDia = document.createElement('button');
        btnDia.type = 'button';
        btnDia.className = 'dt-day' + (d === dtDiaSeleccionado ? ' dt-day--selected' : '');
        btnDia.textContent = String(d);
        btnDia.onclick = function () {
            seleccionarDiaCalendario(d);
        };
        grid.appendChild(btnDia);
    }
}

function confirmarFechaHora() {
    let selectMes = document.getElementById('dtMonthSelect');
    let selectAnio = document.getElementById('dtYearSelect');
    let selectHora = document.getElementById('dtTimeStart');
    let selectHoraFin = document.getElementById('dtTimeEnd');

    let mes = Number(selectMes.value) + 1;
    let anio = Number(selectAnio.value);
    let dia = dtDiaSeleccionado;

    let partesHora = (selectHora.value || '10:00').split(':');
    let horaNum = Number(partesHora[0]);
    let minStr = partesHora[1] || '00';

    if (dtEsPm && horaNum < 12) {
        horaNum += 12;
    } else if (!dtEsPm && horaNum === 12) {
        horaNum = 0;
    }

    let mesStr = mes < 10 ? '0' + mes : String(mes);
    let diaStr = dia < 10 ? '0' + dia : String(dia);
    let horaStr = horaNum < 10 ? '0' + horaNum : String(horaNum);

    let valorIso = anio + '-' + mesStr + '-' + diaStr + 'T' + horaStr + ':' + minStr;
    let inputOculto = document.getElementById('DiaHora');
    let inputVisible = document.getElementById('DiaHoraVisible');

    if (inputOculto != null) {
        inputOculto.value = valorIso;
    }
    if (inputVisible != null) {
        let sufijo = dtEsPm ? 'PM' : 'AM';
        inputVisible.value = diaStr + '/' + mesStr + '/' + anio + ' · ' + selectHora.value + ' - ' + selectHoraFin.value + ' ' + sufijo;
    }

    cerrarSelectorFechaHora();
}


// ===== POSTULANTES DE UNA TAREA (vista del adulto) =====
let contenedorPostulantes = document.getElementById('contenedorPostulantes');
let idTareaPostulantes = contenedorPostulantes != null ? contenedorPostulantes.getAttribute('data-id-tarea') : 0;
let idPostulanteModal = 0;
let nombrePostulanteModal = '';

// Abre el modal y carga los datos del joven con FETCH
function abrirModalPostulante(idUsuario) {
    fetch('/Home/ObtenerDetallePostulanteJson?idUsuario=' + idUsuario)
        .then(function (response) {
            return response.json();
        })
        .then(function (datos) {
            if (datos != null) {
                idPostulanteModal = datos.idUsuario;
                nombrePostulanteModal = datos.nombre;

                document.getElementById('modalPostulanteNombre').textContent = datos.nombre;
                document.getElementById('modalPostulanteFoto').src = datos.foto;
                document.getElementById('modalPostulanteAyudas').textContent = datos.ayudasRealizadas;

                let valor = Math.round(datos.valoracion);
                let estrellas = '';
                for (let i = 1; i <= 5; i++) {
                    if (i <= valor) {
                        estrellas = estrellas + '★';
                    } else {
                        estrellas = estrellas + '☆';
                    }
                }
                document.getElementById('modalPostulanteEstrellas').textContent = estrellas;
                document.getElementById('modalPostulantePuntual').textContent = datos.puntual ? 'Puntual' : 'No siempre puntual';

                document.getElementById('modalPostulante').style.display = 'flex';
            }
        })
        .catch(function (error) {
            console.error('Error al cargar postulante:', error);
        });
}

function cerrarModalPostulante() {
    document.getElementById('modalPostulante').style.display = 'none';
}

function aceptarPostulanteDirecto(idUsuario, nombre) {
    idPostulanteModal = idUsuario;
    nombrePostulanteModal = nombre || 'este joven';
    aceptarPostulanteFetch();
}

function aceptarPostulanteFetch() {
    fetch('/Home/AceptarPostulante?idTarea=' + idTareaPostulantes + '&idUsuario=' + idPostulanteModal, { method: 'POST' })
        .then(function (response) {
            return response.json();
        })
        .then(function (datos) {
            let mensaje = '¡Aceptaste a ' + nombrePostulanteModal + ' para tu ayuda!';
            window.location.href = '/Home/MisAyudas?mensaje=' + encodeURIComponent(mensaje);
        })
        .catch(function (error) {
            console.error('Error al aceptar postulante:', error);
        });
}

function rechazarDesdeModal() {
    cerrarModalPostulante();
    rechazarPostulanteConUndo(idPostulanteModal);
}

// Snackbar de 3 segundos para rechazar postulante con opción de Deshacer
let timerRechazoPostulante = null;
let idPostulantePendienteRechazo = null;

function confirmarRechazoPostulanteServidor(idUsuario) {
    fetch('/Home/RechazarPostulante?idTarea=' + idTareaPostulantes + '&idUsuario=' + idUsuario, { method: 'POST' })
        .catch(function (error) {
            console.error('Error al rechazar postulante:', error);
        });
}

function rechazarPostulanteConUndo(idUsuario) {
    if (timerRechazoPostulante != null && idPostulantePendienteRechazo != null) {
        clearTimeout(timerRechazoPostulante);
        confirmarRechazoPostulanteServidor(idPostulantePendienteRechazo);
    }

    idPostulantePendienteRechazo = idUsuario;
    let card = document.getElementById('card-postulante-' + idUsuario);
    if (card != null) {
        card.style.display = 'none';
    }

    let snackbar = document.getElementById('snackbarPostulante');
    if (snackbar != null) {
        snackbar.style.display = 'flex';
    }

    timerRechazoPostulante = setTimeout(function () {
        if (snackbar != null) {
            snackbar.style.display = 'none';
        }
        if (idPostulantePendienteRechazo != null) {
            confirmarRechazoPostulanteServidor(idPostulantePendienteRechazo);
            idPostulantePendienteRechazo = null;
        }
        timerRechazoPostulante = null;
    }, 3000);
}

function deshacerRechazoPostulante() {
    if (timerRechazoPostulante != null) {
        clearTimeout(timerRechazoPostulante);
        timerRechazoPostulante = null;
    }

    if (idPostulantePendienteRechazo != null) {
        let card = document.getElementById('card-postulante-' + idPostulantePendienteRechazo);
        if (card != null) {
            card.style.display = 'flex';
        }
        idPostulantePendienteRechazo = null;
    }

    let snackbar = document.getElementById('snackbarPostulante');
    if (snackbar != null) {
        snackbar.style.display = 'none';
    }
}
