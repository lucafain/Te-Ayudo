let menu = document.querySelector('.menu-toggle');
let navigation = document.querySelector('#navigation');

if (menu != null && navigation != null) {
    menu.addEventListener('click', function () {
        let abierto = menu.getAttribute('aria-expanded') !== 'true';
        menu.setAttribute('aria-expanded', String(abierto));
        navigation.classList.toggle('open', abierto);
    });
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
            if (inputPass.type === 'password') {
                inputPass.type = 'text';
            } else {
                inputPass.type = 'password';
            }
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
        let email = document.getElementById('Email').value;
        let contrasena = document.getElementById('Contrasena').value;

        if (email === '' || email.indexOf('@') === -1) {
            evento.preventDefault();
            mostrarErrorFormulario('Debe ingresar un correo electrónico válido.');
            return;
        }

        if (contrasena === '') {
            evento.preventDefault();
            mostrarErrorFormulario('Debe ingresar una contraseña.');
            return;
        }
    });
}

let formRegistro = document.getElementById('formRegistro');
if (formRegistro != null) {
    formRegistro.addEventListener('submit', function (evento) {
        let email = document.getElementById('Email').value;
        let nombre = document.getElementById('Nombre').value;
        let contrasena = document.getElementById('Contrasena').value;

        if (email === '' || email.indexOf('@') === -1) {
            evento.preventDefault();
            mostrarErrorFormulario('El correo electrónico es obligatorio y debe ser válido.');
            return;
        }

        if (nombre === '') {
            evento.preventDefault();
            mostrarErrorFormulario('El nombre de usuario es obligatorio.');
            return;
        }

        if (contrasena === '' || contrasena.length < 4) {
            evento.preventDefault();
            mostrarErrorFormulario('La contraseña debe tener al menos 4 caracteres.');
            return;
        }
    });
}

let formTrabajar = document.getElementById('formTrabajar');
if (formTrabajar != null) {
    formTrabajar.addEventListener('submit', function (evento) {
        let nombre = document.getElementById('Nombre').value;
        let apellido = document.getElementById('Apellido').value;
        let email = document.getElementById('Email').value;
        let contrasena = document.getElementById('Contrasena').value;
        let acepta = document.getElementById('AceptaTerminos').checked;

        if (nombre === '' || apellido === '' || email === '' || contrasena === '') {
            evento.preventDefault();
            mostrarErrorFormulario('Debe completar los datos obligatorios.');
            return;
        }

        if (!acepta) {
            evento.preventDefault();
            mostrarErrorFormulario('Debe aceptar los términos y condiciones.');
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
            mostrarErrorFormulario('Debe ingresar un correo electrónico válido.');
            return;
        }
    });
}

let formSolicitar = document.getElementById('formSolicitar');
if (formSolicitar != null) {
    formSolicitar.addEventListener('submit', function (evento) {
        let titulo = document.getElementById('Titulo').value;
        let descripcion = document.getElementById('Descripcion').value;
        let lugar = document.getElementById('Lugar').value;
        let diaHora = document.getElementById('DiaHora').value;

        if (titulo === '' || descripcion === '' || lugar === '' || diaHora === '') {
            evento.preventDefault();
            mostrarErrorFormulario('Por favor completá el título, la descripción, el lugar y la fecha/hora.');
            return;
        }
    });
}

let formModificarTarea = document.getElementById('formModificarTarea');
if (formModificarTarea != null) {
    formModificarTarea.addEventListener('submit', function (evento) {
        let titulo = document.getElementById('Titulo').value;
        let descripcion = document.getElementById('Descripcion').value;
        let lugar = document.getElementById('Lugar').value;
        let diaHora = document.getElementById('DiaHora').value;
        let confirmacion = document.getElementById('ConfirmacionModificar').value.toLowerCase();

        if (titulo === '' || descripcion === '' || lugar === '' || diaHora === '') {
            evento.preventDefault();
            mostrarErrorFormulario('Por favor completá el título, la descripción, el lugar y el día/hora.');
            return;
        }

        if (confirmacion !== 'modificar') {
            evento.preventDefault();
            mostrarErrorFormulario('Debés escribir "modificar" para confirmar los cambios.');
            return;
        }
    });
}

// ===== FETCH: VER DETALLE DE UN USUARIO (Slides 142-146) =====
function verInfoAyudado(idUsuario) {
    fetch('/Home/VerDetalleAyudado?idUsuario=' + idUsuario)
        .then(function (response) {
            return response.json();
        })
        .then(function (datos) {
            if (datos != null) {
                let contenedor = document.getElementById('contenedorDetalleFetch');
                let nombre = datos.nombre || '';
                let apellido = datos.apellido || '';
                let email = datos.email || 'No especificado';
                let provincia = datos.provincia || '';
                let calle = datos.calle || '';
                let direccion = datos.direccion || '';
                let tipoTexto = datos.tipoUsuario ? 'Joven' : 'Adulto mayor';

                document.getElementById('detalleNombre').textContent = 'Información de ' + nombre + ' ' + apellido;
                document.getElementById('detalleEmail').textContent = 'Email: ' + email;
                document.getElementById('detalleTelefono').textContent = 'Ubicación: ' + calle + ' ' + direccion + ' (' + provincia + ')';
                document.getElementById('detalleTipo').textContent = 'Tipo de usuario: ' + tipoTexto;

                contenedor.style.display = 'block';
            }
        });
}

// ===== REALIZAR AYUDAS (vista del joven) =====
let idTareaActual = 0;
let timerRechazo = null;
let idPendienteRechazo = null;

function mostrarVista(idVista) {
    let vistas = ['vistaListaAyudas', 'vistaDetalleAyuda', 'vistaDetalleAceptada', 'vistaPostuladosAyuda'];
    for (let i = 0; i < vistas.length; i++) {
        let el = document.getElementById(vistas[i]);
        if (el != null) {
            el.hidden = (vistas[i] !== idVista);
        }
    }
}

function abrirDetalle(idTarea) {
    idTareaActual = idTarea;
    fetch('/Home/ObtenerDetalleTareaJson?idTarea=' + idTarea)
        .then(function (response) {
            return response.json();
        })
        .then(function (datos) {
            if (datos != null) {
                document.getElementById('detTitulo').textContent = datos.titulo || 'Detalle de tarea';
                let desc = datos.descripcion || '';
                if (datos.aclaracion && datos.aclaracion !== '') {
                    desc = desc + ' · ' + datos.aclaracion;
                }
                document.getElementById('detDescripcion').textContent = desc;
                document.getElementById('detMonto').textContent = datos.monto || '5000';
                document.getElementById('detDuracion').textContent = '-';
                document.getElementById('detHorario').textContent = (datos.fechaDia || datos.fecha || '') + ' ' + (datos.hora || '') + ' hs';
                document.getElementById('detSolicitante').textContent = datos.nombreSolicitante || 'Adulto mayor';

                let domicilio = datos.lugar || '';
                if (datos.barrio && datos.barrio !== '') {
                    domicilio = domicilio + ' (' + datos.barrio + ')';
                }
                document.getElementById('detDomicilio').textContent = domicilio;
                document.getElementById('detAcompaniamiento').textContent = 'No especificado';

                document.getElementById('btnPostularse').disabled = false;
                mostrarVista('vistaDetalleAyuda');
            }
        });
}

function abrirDetalleAceptada(idTarea, esFinalizada) {
    idTareaActual = idTarea;
    fetch('/Home/ObtenerDetalleTareaJson?idTarea=' + idTarea)
        .then(function (response) {
            return response.json();
        })
        .then(function (datos) {
            if (datos != null) {
                document.getElementById('detAceptadaTitulo').textContent = datos.titulo || 'Detalle de tarea';
                let desc = datos.descripcion || '';
                if (datos.aclaracion && datos.aclaracion !== '') {
                    desc = desc + ' · ' + datos.aclaracion;
                }
                document.getElementById('detAceptadaDescripcion').textContent = desc;
                document.getElementById('detAceptadaHorario').textContent = (datos.fechaDia || datos.fecha || '') + ' ' + (datos.hora || '') + ' hs';
                document.getElementById('detAceptadaDuracion').textContent = '-';
                document.getElementById('detAceptadaMonto').textContent = datos.monto || '5000';
                document.getElementById('detAceptadaSolicitante').textContent = datos.nombreSolicitante || 'Adulto mayor';

                let domicilio = datos.lugar || '';
                if (datos.barrio && datos.barrio !== '') {
                    domicilio = domicilio + ' (' + datos.barrio + ')';
                }
                document.getElementById('detAceptadaDomicilio').textContent = domicilio;

                let btn = document.getElementById('btnFinalizarTarea');
                if (btn != null) {
                    btn.hidden = esFinalizada;
                    btn.disabled = false;
                }
                mostrarVista('vistaDetalleAceptada');
            }
        });
}

function postularse() {
    if (idTareaActual <= 0) {
        return;
    }

    fetch('/Home/PostularseJson?idTarea=' + idTareaActual, { method: 'POST' })
        .then(function (response) {
            return response.json();
        })
        .then(function (datos) {
            if (datos != null) {
                document.getElementById('postuladosNombreTarea').textContent = datos.tituloTarea || '';
                let cont = document.getElementById('listaPostuladosContainer');
                cont.innerHTML = '';

                let lista = datos.postulados || [];
                for (let i = 0; i < lista.length; i++) {
                    let u = lista[i];
                    let idU = u.idUsuario || u.IdUsuario;
                    let nombre = (u.nombre || '') + ' ' + (u.apellido || '');
                    let foto = u.fotoPerfil || '/img/user.svg';

                    cont.innerHTML = cont.innerHTML +
                        '<article class="ar-post-card">' +
                        '<img src="' + foto + '" alt="' + nombre + '" />' +
                        '<div>' +
                        '<h3>' + nombre + '</h3>' +
                        '<button type="button" class="ar-btn ar-btn-ver" onclick="verInfoPostulado(' + idU + ')">Ver más</button>' +
                        '</div>' +
                        '</article>';
                }

                document.getElementById('btnVerPostulados').hidden = false;
                mostrarVista('vistaPostuladosAyuda');
            }
        });
}

function verInfoPostulado(idUsuario) {
    fetch('/Home/VerDetalleAyudado?idUsuario=' + idUsuario)
        .then(function (response) {
            return response.json();
        })
        .then(function (datos) {
            if (datos != null) {
                let nombre = (datos.nombre || '') + ' ' + (datos.apellido || '');
                let email = datos.email || 'No especificado';
                let calle = datos.calle || '';
                let dir = datos.direccion || '';
                let provincia = datos.provincia || '';
                let tipo = datos.tipoUsuario ? 'Joven' : 'Adulto mayor';

                document.getElementById('modalPostuladoNombre').textContent = nombre;
                document.getElementById('modalPostuladoEmail').textContent = 'Email: ' + email;
                document.getElementById('modalPostuladoUbicacion').textContent = 'Ubicación: ' + calle + ' ' + dir + ' (' + provincia + ')';
                document.getElementById('modalPostuladoTipo').textContent = 'Tipo de usuario: ' + tipo;
                document.getElementById('modalPostulado').hidden = false;
            }
        });
}

function cerrarModalPostulado() {
    document.getElementById('modalPostulado').hidden = true;
}

function confirmarRechazo(idTarea) {
    fetch('/Home/RechazarTarea?idTarea=' + idTarea, { method: 'POST' });
}

function rechazarConDeshacer(idTarea) {
    if (timerRechazo != null && idPendienteRechazo != null) {
        clearTimeout(timerRechazo);
        confirmarRechazo(idPendienteRechazo);
    }

    idPendienteRechazo = idTarea;
    let card = document.getElementById('card-tarea-' + idTarea);
    if (card != null) {
        card.style.display = 'none';
    }

    let snack = document.getElementById('snackbarRechazo');
    if (snack != null) {
        snack.style.display = 'flex';
    }

    timerRechazo = setTimeout(function () {
        if (snack != null) {
            snack.style.display = 'none';
        }
        if (idPendienteRechazo != null) {
            confirmarRechazo(idPendienteRechazo);
            idPendienteRechazo = null;
        }
        timerRechazo = null;
    }, 3000);
}

function rechazarDesdeDetalle() {
    mostrarVista('vistaListaAyudas');
    rechazarConDeshacer(idTareaActual);
}

function deshacerRechazo() {
    if (timerRechazo != null) {
        clearTimeout(timerRechazo);
        timerRechazo = null;
    }
    if (idPendienteRechazo != null) {
        let card = document.getElementById('card-tarea-' + idPendienteRechazo);
        if (card != null) {
            card.style.display = '';
        }
        idPendienteRechazo = null;
    }
    let snack = document.getElementById('snackbarRechazo');
    if (snack != null) {
        snack.style.display = 'none';
    }
}

function eliminarTarjeta(idTarea) {
    fetch('/Home/EliminarTareaRechazada?idTarea=' + idTarea, { method: 'POST' })
        .then(function () {
            let card = document.getElementById('card-tarea-' + idTarea);
            if (card != null) {
                card.style.display = 'none';
            }
        });
}

function finalizarTarea() {
    if (idTareaActual <= 0) {
        return;
    }
    fetch('/Home/FinalizarTareaAceptada?idTarea=' + idTareaActual, { method: 'POST' })
        .then(function () {
            window.location.href = '/Home/RealizarAyudas?mensaje=¡Finalizaste la ayuda correctamente!';
        });
}

// ===== VALORACIÓN DEL JOVEN (TicketAyuda.cshtml) =====
function abrirValoracionJoven() {
    let vTicket = document.getElementById('vistaTicketDetalle');
    let vVal = document.getElementById('vistaValoracionJoven');
    if (vTicket != null) {
        vTicket.style.display = 'none';
    }
    if (vVal != null) {
        vVal.style.display = 'block';
    }
}

function seleccionarEstrellaValoracion(valor) {
    let inputCal = document.getElementById('inputCalificacionJoven');
    if (inputCal != null) {
        inputCal.value = String(valor);
    }
    let botones = document.querySelectorAll('#contenedorEstrellasValoracion .estrella-btn');
    for (let i = 0; i < botones.length; i++) {
        let v = Number(botones[i].getAttribute('data-valor'));
        if (v <= valor) {
            botones[i].classList.add('estrella-activa');
        } else {
            botones[i].classList.remove('estrella-activa');
        }
    }
}

function seleccionarPuntualidadJoven(esPuntual) {
    let inputP = document.getElementById('inputPuntualJoven');
    if (inputP != null) {
        inputP.value = esPuntual ? 'true' : 'false';
    }
    let btnSi = document.getElementById('btnPuntualSi');
    let btnNo = document.getElementById('btnPuntualNo');
    if (btnSi != null && btnNo != null) {
        if (esPuntual) {
            btnSi.classList.add('btn-puntual-activo');
            btnNo.classList.remove('btn-puntual-activo');
        } else {
            btnNo.classList.add('btn-puntual-activo');
            btnSi.classList.remove('btn-puntual-activo');
        }
    }
}

// ===== SELECTOR DE FECHA Y HORA (SolicitarTarea.cshtml) =====
let dtDiaSeleccionado = new Date().getDate();
let dtEsPm = false;

function abrirSelectorFechaHora() {
    let popup = document.getElementById('customDateTimePopup');
    if (popup != null) {
        renderizarCalendario();
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
    let totalDiasMes = new Date(anio, mes + 1, 0).getDate();

    grid.innerHTML = '';

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

    let partesHora = selectHora.value.split(':');
    let horaNum = Number(partesHora[0]);
    let minStr = partesHora[1];

    if (dtEsPm && horaNum < 12) {
        horaNum = horaNum + 12;
    } else if (!dtEsPm && horaNum === 12) {
        horaNum = 0;
    }

    let mesStr = mes < 10 ? '0' + mes : String(mes);
    let diaStr = dia < 10 ? '0' + dia : String(dia);
    let horaStr = horaNum < 10 ? '0' + horaNum : String(horaNum);

    let valorIso = anio + '-' + mesStr + '-' + diaStr + 'T' + horaStr + ':' + minStr;
    document.getElementById('DiaHora').value = valorIso;
    let sufijo = dtEsPm ? 'PM' : 'AM';
    document.getElementById('DiaHoraVisible').value = diaStr + '/' + mesStr + '/' + anio + ' · ' + selectHora.value + ' - ' + selectHoraFin.value + ' ' + sufijo;

    cerrarSelectorFechaHora();
}

// ===== POSTULANTES DE UNA TAREA (PostulantesTarea.cshtml) =====
let contenedorPostulantes = document.getElementById('contenedorPostulantes');
let idTareaPostulantes = contenedorPostulantes != null ? contenedorPostulantes.getAttribute('data-id-tarea') : 0;
let idPostulanteModal = 0;
let nombrePostulanteModal = '';

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
        });
}

function cerrarModalPostulante() {
    document.getElementById('modalPostulante').style.display = 'none';
}

function aceptarPostulanteDirecto(idUsuario, nombre) {
    idPostulanteModal = idUsuario;
    nombrePostulanteModal = nombre;
    aceptarPostulanteFetch();
}

function aceptarPostulanteFetch() {
    fetch('/Home/AceptarPostulante?idTarea=' + idTareaPostulantes + '&idUsuario=' + idPostulanteModal, { method: 'POST' })
        .then(function (response) {
            return response.json();
        })
        .then(function () {
            let mensaje = '¡Aceptaste a ' + nombrePostulanteModal + ' para tu ayuda!';
            window.location.href = '/Home/MisAyudas?mensaje=' + mensaje;
        });
}

function rechazarDesdeModal() {
    cerrarModalPostulante();
    rechazarPostulanteConUndo(idPostulanteModal);
}

let timerRechazoPostulante = null;
let idPostulantePendienteRechazo = null;

function confirmarRechazoPostulanteServidor(idUsuario) {
    fetch('/Home/RechazarPostulante?idTarea=' + idTareaPostulantes + '&idUsuario=' + idUsuario, { method: 'POST' });
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
