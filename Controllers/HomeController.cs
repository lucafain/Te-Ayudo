using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using TeAyudo.Models;

namespace TeAyudo.Controllers;

public class HomeController : Controller
{
    private IWebHostEnvironment _environment;

    public HomeController(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    public IActionResult Index()
    {
        Dictionary<string, string> textosHome = new Dictionary<string, string>();
        textosHome.Add("Titulo", "Dónde encontrás lo que buscás");
        textosHome.Add("AyudaTitulo", "Recibí la ayuda que buscás");
        textosHome.Add("AyudaTexto", "Seleccionamos a quienes pueden acompañarte.");
        textosHome.Add("TrabajoTitulo", "¿Sos joven y querés ganar experiencia laboral?");
        textosHome.Add("TrabajoTexto", "Formá parte de las historias de Te Ayudo.");

        if (textosHome.ContainsKey("Titulo"))
        {
            ViewBag.Titulo = textosHome["Titulo"];
        }
        if (textosHome.ContainsKey("AyudaTitulo"))
        {
            ViewBag.AyudaTitulo = textosHome["AyudaTitulo"];
        }
        if (textosHome.ContainsKey("AyudaTexto"))
        {
            ViewBag.AyudaTexto = textosHome["AyudaTexto"];
        }
        if (textosHome.ContainsKey("TrabajoTitulo"))
        {
            ViewBag.TrabajoTitulo = textosHome["TrabajoTitulo"];
        }
        if (textosHome.ContainsKey("TrabajoTexto"))
        {
            ViewBag.TrabajoTexto = textosHome["TrabajoTexto"];
        }

        string[] arrayPresentacion = new string[2];
        arrayPresentacion[0] = "Jóvenes brindan ayuda a adultos que lo requieran a cambio de inclusión laboral";
        arrayPresentacion[1] = "Adultos reciben asistencia a cambio de montos accesibles";

        List<string> listaPresentacion = new List<string>();
        foreach (string item in arrayPresentacion)
        {
            listaPresentacion.Add(item);
        }
        ViewBag.Presentacion = listaPresentacion;

        List<string> listaSobreNosotros = new List<string>();
        listaSobreNosotros.Add("En Te Ayudo creemos que ayudarnos entre generaciones puede cambiar la forma en la que vivimos.");
        listaSobreNosotros.Add("Creamos un espacio que conecta a jóvenes universitarios con adultos para que puedan ayudarse en tareas cotidianas.");
        listaSobreNosotros.Add("Los jóvenes tienen la posibilidad de ganar experiencia y aprender, mientras que los adultos encuentran una forma sencilla y segura de recibir la ayuda que necesitan.");
        listaSobreNosotros.Add("Buscamos acompañar a quien lo necesita y a quien puede ayudar, con el cuidado que las personas de nuestra comunidad merecen.");
        ViewBag.SobreNosotros = listaSobreNosotros;

        List<ContenidoItem> listaPasos = new List<ContenidoItem>();
        listaPasos.Add(new ContenidoItem("Publicá", "Publicás la ayuda que precisás y elegís un momento.", "pen.png"));
        listaPasos.Add(new ContenidoItem("Encontrá", "Buscás a alguien que pueda ayudarte.", "alarm.png"));
        listaPasos.Add(new ContenidoItem("Conectate", "Coordinás los detalles entre ambas partes.", "circle-check.png"));
        ViewBag.Pasos = listaPasos;

        List<ContenidoItem> listaValores = new List<ContenidoItem>();
        listaValores.Add(new ContenidoItem("Seguridad", "Tu tranquilidad es nuestra prioridad. Promovemos el respeto y el cuidado entre las personas de nuestra comunidad.", "circle-check.png"));
        listaValores.Add(new ContenidoItem("Confianza", "Creemos en la confianza entre las personas. Cada encuentro es una oportunidad para conocerse y construir una buena relación.", "pen.png"));
        listaValores.Add(new ContenidoItem("Comunicación", "Todo es más fácil cuando nos entendemos. Conversá para coordinar la ayuda, acordar los detalles y resolver cualquier duda.", "alarm.png"));
        ViewBag.Valores = listaValores;

        List<ContenidoItem> listaPreguntas = new List<ContenidoItem>();
        listaPreguntas.Add(new ContenidoItem("¿Qué es Te Ayudo?", "Un espacio que conecta a jóvenes con adultos que necesitan ayuda con tareas cotidianas."));
        listaPreguntas.Add(new ContenidoItem("¿Cómo solicito ayuda?", "Desde Solicitar ayuda podés acceder al formulario de solicitud o al registro para adultos mayores."));
        listaPreguntas.Add(new ContenidoItem("¿Cómo puedo sumarme?", "Desde Trabajá con nosotros podés acceder al registro para jóvenes."));
        ViewBag.Preguntas = listaPreguntas;

        return View();
    }

    public IActionResult Ingresar()
    {
        return RedirectToAction("Login", "Home");
    }

    public IActionResult Login(string mensaje = "")
    {
        if (mensaje != "")
        {
            ViewBag.SuccessMessage = mensaje;
        }
        return View();
    }

    [HttpPost]
    public IActionResult Login(string Email = "", string Contrasena = "")
    {
        if (Email == null || Email.Length == 0)
        {
            ViewBag.ErrorMessage = "Debe completar el email.";
            return View("Login");
        }

        if (Email.IndexOf("@") == -1)
        {
            ViewBag.ErrorMessage = "El email ingresado no es válido.";
            return View("Login");
        }

        if (Contrasena == null || Contrasena.Length == 0)
        {
            ViewBag.ErrorMessage = "Debe completar la contraseña.";
            return View("Login");
        }

        string emailLimpio = Email.Replace(" ", "").ToLower();
        Usuarios usuario = BD.ObtenerUsuarioPorCredenciales(emailLimpio, Contrasena);

        if (usuario == null)
        {
            ViewBag.ErrorMessage = "Email o contraseña incorrectos.";
            return View("Login");
        }

        HttpContext.Session.SetString("EmailLogueado", usuario.Email);
        HttpContext.Session.SetInt32("IdUsuarioLogueado", usuario.IdUsuario);
        return RedirectToAction("Perfil", "Home");
    }

    public IActionResult SolicitarTarea()
    {
        Usuarios usuario = ObtenerUsuarioSesion();
        if (usuario == null)
        {
            return RedirectToAction("Login", "Home");
        }
        if (usuario.TipoUsuario)
        {
            return RedirectToAction("PrincipalJoven", "Home");
        }

        ViewBag.UsuarioActual = usuario;
        return View();
    }

    [HttpPost]
    public IActionResult SolicitarTarea(string Titulo = "", string Descripcion = "", string Lugar = "", string Barrio = "", string DiaHora = "", string Aclaracion = "", string TipoLugar = "Domicilio", double Monto = 0)
    {
        Usuarios usuarioActual = ObtenerUsuarioSesion();
        if (usuarioActual == null)
        {
            return RedirectToAction("Login", "Home");
        }
        if (usuarioActual.TipoUsuario)
        {
            return RedirectToAction("PrincipalJoven", "Home");
        }

        ViewBag.UsuarioActual = usuarioActual;

        if (Titulo == null) Titulo = "";
        if (Descripcion == null) Descripcion = "";
        if (Lugar == null) Lugar = "";
        if (Barrio == null) Barrio = "";
        if (DiaHora == null) DiaHora = "";
        if (Aclaracion == null) Aclaracion = "";
        if (TipoLugar == null || TipoLugar == "") TipoLugar = "Domicilio";

        if (Titulo == "" || Descripcion == "" || Lugar == "" || DiaHora == "")
        {
            ViewBag.ErrorMessage = "Por favor complete el nombre de la ayuda, la descripción, el lugar y la fecha/hora.";
            return View();
        }

        if (Titulo.Length > 50) Titulo = Titulo.Substring(0, 50);
        if (Descripcion.Length > 500) Descripcion = Descripcion.Substring(0, 500);
        if (Lugar.Length > 50) Lugar = Lugar.Substring(0, 50);
        if (Barrio.Length > 50) Barrio = Barrio.Substring(0, 50);
        if (TipoLugar.Length > 50) TipoLugar = TipoLugar.Substring(0, 50);

        DateTime fechaTarea = DateTime.Now;
        TimeSpan horaTarea = new TimeSpan(10, 0, 0);

        if (DateTime.TryParse(DiaHora, out DateTime fechaHoraCompleta))
        {
            fechaTarea = fechaHoraCompleta.Date;
            horaTarea = fechaHoraCompleta.TimeOfDay;
        }

        string textoFechaHora = fechaTarea.ToString("dd/MM/yyyy") + " " + horaTarea.ToString(@"hh\:mm");
        string aclaracionFinal = "";
        if (Aclaracion == "")
        {
            aclaracionFinal = "Día y hora: " + textoFechaHora;
        }
        else
        {
            aclaracionFinal = Aclaracion + " (Día y hora: " + textoFechaHora + ")";
        }

        Tareas nuevaTarea = new Tareas();
        nuevaTarea.Titulo = Titulo;
        nuevaTarea.Descripcion = Descripcion;
        nuevaTarea.Estado = true;
        nuevaTarea.FechaCreacion = DateTime.Now;
        nuevaTarea.Lugar = Lugar;
        nuevaTarea.Barrio = Barrio;
        nuevaTarea.DiaTarea = fechaTarea;
        nuevaTarea.HoraTarea = horaTarea;
        nuevaTarea.Aclaracion = aclaracionFinal;
        nuevaTarea.Monto = Monto;
        nuevaTarea.TipoLugar = TipoLugar;
        nuevaTarea.IdUsuarioSolicitante = usuarioActual.IdUsuario;

        bool resultado = BD.GuardarTarea(nuevaTarea);
        if (!resultado)
        {
            ViewBag.ErrorMessage = "No se pudo guardar la tarea en la base de datos.";
            return View();
        }

        return RedirectToAction("MisAyudas", "Home", new { mensaje = "¡Tu solicitud de ayuda (" + Titulo + ") ha sido publicada!" });
    }

    public IActionResult Trabajar()
    {
        return View();
    }

    public IActionResult RegistroJoven()
    {
        ViewBag.Title = "Registro para jóvenes";
        return View("Trabajar");
    }

    public IActionResult RegistroMayor()
    {
        ViewBag.Title = "Registro para solicitar ayuda";
        return View("Registro");
    }

    [HttpPost]
    public IActionResult GuardarRegistroMayor(string Nombre = "", string Apellido = "", string Email = "", string Provincia = "", string Calle = "", int Direccion = 0, string CodigoPostal = "", string Telefono = "", string Contrasena = "", IFormFile FotoPerfil = null, bool AceptaTerminos = true)
    {
        if (Nombre == null) Nombre = "";
        if (Apellido == null) Apellido = "";
        if (Email == null) Email = "";
        if (Provincia == null) Provincia = "";
        if (Calle == null) Calle = "";
        if (CodigoPostal == null) CodigoPostal = "";
        if (Telefono == null) Telefono = "";
        if (Contrasena == null) Contrasena = "";

        if (Nombre == "" || Email == "" || Contrasena == "")
        {
            ViewBag.ErrorMessage = "Por favor complete los campos obligatorios (*).";
            return View("Registro");
        }

        if (Email.IndexOf("@") == -1)
        {
            ViewBag.ErrorMessage = "El correo electrónico no es válido.";
            return View("Registro");
        }

        if (Contrasena.Length < 4)
        {
            ViewBag.ErrorMessage = "La contraseña debe tener al menos 4 caracteres.";
            return View("Registro");
        }

        string codigoPostalFinal = CodigoPostal;
        if (codigoPostalFinal == "")
        {
            codigoPostalFinal = Telefono;
        }
        if (codigoPostalFinal.Length > 8)
        {
            codigoPostalFinal = codigoPostalFinal.Substring(0, 8);
        }

        string rutaFoto = GuardarArchivo(FotoPerfil, "uploads/perfiles");
        if (rutaFoto == "")
        {
            rutaFoto = "/img/user.svg";
        }

        Usuarios user = new Usuarios();
        user.Nombre = Nombre;
        user.Apellido = Apellido;
        user.Email = Email.Replace(" ", "").ToLower();
        user.Provincia = Provincia;
        user.Calle = Calle;
        user.Direccion = Direccion;
        user.CodigoPostal = codigoPostalFinal;
        user.FechaRegistro = DateTime.Now;
        user.TipoUsuario = false;
        user.Contrasena = Contrasena;
        user.AceptaTerminos = AceptaTerminos;
        user.FotoPerfil = rutaFoto;

        bool resultado = BD.GuardarUsuario(user);
        if (resultado)
        {
            Usuarios usuarioGuardado = BD.ObtenerUsuarioPorEmail(user.Email);
            if (usuarioGuardado != null)
            {
                HttpContext.Session.SetString("EmailLogueado", usuarioGuardado.Email);
                HttpContext.Session.SetInt32("IdUsuarioLogueado", usuarioGuardado.IdUsuario);
            }
            return RedirectToAction("Perfil", "Home");
        }

        ViewBag.ErrorMessage = "El email ya existe o no se pudo guardar el registro.";
        return View("Registro");
    }

    [HttpPost]
    public IActionResult GuardarRegistroJoven(string Nombre = "", string Apellido = "", string Email = "", string Provincia = "", string Calle = "", int Direccion = 0, string CodigoPostal = "", string Contrasena = "", IFormFile FotoPerfil = null, bool AceptaTerminos = false)
    {
        if (Nombre == null) Nombre = "";
        if (Apellido == null) Apellido = "";
        if (Email == null) Email = "";
        if (Provincia == null) Provincia = "";
        if (Calle == null) Calle = "";
        if (CodigoPostal == null) CodigoPostal = "";
        if (Contrasena == null) Contrasena = "";

        if (Nombre == "" || Apellido == "" || Email == "" || Contrasena == "")
        {
            ViewBag.ErrorMessage = "Debe completar los datos obligatorios del registro.";
            return View("Trabajar");
        }

        if (Email.IndexOf("@") == -1)
        {
            ViewBag.ErrorMessage = "Debe ingresar un correo electrónico válido.";
            return View("Trabajar");
        }

        if (Contrasena.Length < 4)
        {
            ViewBag.ErrorMessage = "Debe ingresar una contraseña de al menos 4 caracteres.";
            return View("Trabajar");
        }

        if (!AceptaTerminos)
        {
            ViewBag.ErrorMessage = "Debe aceptar los términos y condiciones.";
            return View("Trabajar");
        }

        string rutaFoto = GuardarArchivo(FotoPerfil, "uploads/perfiles");
        if (rutaFoto == "")
        {
            rutaFoto = "/img/user.svg";
        }

        Usuarios user = new Usuarios();
        user.Nombre = Nombre;
        user.Apellido = Apellido;
        user.Email = Email.Replace(" ", "").ToLower();
        user.Provincia = Provincia;
        user.Calle = Calle;
        user.Direccion = Direccion;
        user.CodigoPostal = CodigoPostal;
        user.FechaRegistro = DateTime.Now;
        user.TipoUsuario = true;
        user.Contrasena = Contrasena;
        user.AceptaTerminos = AceptaTerminos;
        user.FotoPerfil = rutaFoto;

        bool resultado = BD.GuardarUsuario(user);
        if (resultado)
        {
            Usuarios usuarioGuardado = BD.ObtenerUsuarioPorEmail(user.Email);
            if (usuarioGuardado != null)
            {
                HttpContext.Session.SetString("EmailLogueado", usuarioGuardado.Email);
                HttpContext.Session.SetInt32("IdUsuarioLogueado", usuarioGuardado.IdUsuario);
            }
            return RedirectToAction("Perfil", "Home");
        }

        ViewBag.ErrorMessage = "El email ya existe. Por favor, elegí otro.";
        return View("Trabajar");
    }

    public IActionResult OlvideContrasena()
    {
        return View("olvideContraseña");
    }

    [HttpPost]
    public IActionResult OlvideContrasena(string Email = "")
    {
        if (Email == null || Email == "" || Email.IndexOf("@") == -1)
        {
            ViewBag.ErrorMessage = "Debe ingresar un email válido.";
            return View("olvideContraseña");
        }

        string emailLimpio = Email.Replace(" ", "").ToLower();
        Usuarios usuario = BD.ObtenerUsuarioPorEmail(emailLimpio);

        if (usuario == null)
        {
            ViewBag.ErrorMessage = "No existe un usuario con ese email.";
            return View("olvideContraseña");
        }

        ViewBag.Contrasena = usuario.Contrasena;
        return View("MostrarContraseña");
    }

    public IActionResult Perfil(string mensaje = "")
    {
        Usuarios usuario = ObtenerUsuarioSesion();
        if (usuario == null)
        {
            return RedirectToAction("Login", "Home");
        }

        if (mensaje != "")
        {
            ViewBag.SuccessMessage = mensaje;
        }
        ViewBag.Nombre = usuario.Nombre;
        ViewBag.Apellido = usuario.Apellido;
        ViewBag.Email = usuario.Email;
        ViewBag.Provincia = usuario.Provincia;
        ViewBag.Calle = usuario.Calle;
        ViewBag.Direccion = usuario.Direccion;
        ViewBag.CodigoPostal = usuario.CodigoPostal;
        ViewBag.EsJoven = usuario.TipoUsuario;
        if (usuario.TipoUsuario)
        {
            ViewBag.TipoUsuario = "Joven";
        }
        else
        {
            ViewBag.TipoUsuario = "Adulto mayor";
        }
        ViewBag.FotoPerfil = usuario.FotoPerfil;

        return View("perfil");
    }

    [HttpPost]
    public IActionResult ActualizarFotoPerfil(IFormFile FotoPerfil = null)
    {
        Usuarios usuario = ObtenerUsuarioSesion();
        if (usuario == null)
        {
            return RedirectToAction("Login", "Home");
        }

        string rutaNueva = GuardarArchivo(FotoPerfil, "uploads/perfiles");
        if (rutaNueva != "")
        {
            BD.ActualizarFotoPerfil(usuario.IdUsuario, rutaNueva);
            return RedirectToAction("Perfil", "Home", new { mensaje = "¡Foto de perfil actualizada y guardada en la base de datos!" });
        }

        return RedirectToAction("Perfil", "Home");
    }

    public IActionResult PrincipalJoven()
    {
        Usuarios joven = ObtenerUsuarioSesion();
        if (joven == null)
        {
            return RedirectToAction("Login", "Home");
        }
        if (!joven.TipoUsuario)
        {
            return RedirectToAction("PrincipalAdulto", "Home");
        }

        List<Tareas> tareasRealizadas = BD.ObtenerTareasRealizadasPorJoven(joven.IdUsuario);
        Dictionary<int, Usuarios> solicitantes = new Dictionary<int, Usuarios>();
        foreach (Tareas tarea in tareasRealizadas)
        {
            if (!solicitantes.ContainsKey(tarea.IdUsuarioSolicitante))
            {
                Usuarios solicitante = BD.ObtenerUsuarioPorId(tarea.IdUsuarioSolicitante);
                if (solicitante != null)
                {
                    solicitantes.Add(tarea.IdUsuarioSolicitante, solicitante);
                }
            }
        }

        ViewBag.Joven = joven;
        ViewBag.UltimosAyudados = BD.ObtenerUltimosAyudados(joven.IdUsuario);
        ViewBag.TareasDisponibles = tareasRealizadas;
        ViewBag.Solicitantes = solicitantes;

        return View("PrincipalJoven");
    }

    public IActionResult PrincipalAdulto()
    {
        Usuarios adulto = ObtenerUsuarioSesion();
        if (adulto == null)
        {
            return RedirectToAction("Login", "Home");
        }
        if (adulto.TipoUsuario)
        {
            return RedirectToAction("PrincipalJoven", "Home");
        }

        List<Tareas> misTareas = BD.ObtenerTareasPorSolicitante(adulto.IdUsuario);
        List<Tareas> tareasConJoven = new List<Tareas>();
        Dictionary<int, Usuarios> jovenesAsignados = new Dictionary<int, Usuarios>();

        foreach (Tareas tarea in misTareas)
        {
            Usuarios jovenAsignado = BD.ObtenerJovenAceptadoPorTarea(tarea.IdTarea);
            if (jovenAsignado != null)
            {
                if (!jovenesAsignados.ContainsKey(tarea.IdTarea))
                {
                    jovenesAsignados.Add(tarea.IdTarea, jovenAsignado);
                }
                tareasConJoven.Add(tarea);
            }
        }

        ViewBag.Adulto = adulto;
        ViewBag.Jovenes = BD.ObtenerJovenesDisponibles();
        ViewBag.MisTareas = misTareas;
        ViewBag.TareasConJoven = tareasConJoven;
        ViewBag.JovenesAsignados = jovenesAsignados;
        ViewBag.PersonasQueAyudaron = BD.ObtenerPersonasQueAyudaron(adulto.IdUsuario);

        return View("PrincipalAdulto");
    }

    public IActionResult Explorar()
    {
        Usuarios usuario = ObtenerUsuarioSesion();
        if (usuario == null)
        {
            return RedirectToAction("Login", "Home");
        }

        ViewBag.Joven = usuario;
        if (usuario.TipoUsuario)
        {
            ViewBag.UltimosAyudados = BD.ObtenerUltimosAyudados(usuario.IdUsuario);
            ViewBag.CantidadAyudas = BD.ContarAyudasRealizadas(usuario.IdUsuario);
            ViewBag.TareasDisponibles = BD.ObtenerTareasDisponibles();
        }
        else
        {
            ViewBag.UltimosAyudados = BD.ObtenerPersonasQueAyudaron(usuario.IdUsuario);
            List<Tareas> misTareas = BD.ObtenerTareasPorSolicitante(usuario.IdUsuario);
            ViewBag.CantidadAyudas = misTareas.Count;
            ViewBag.TareasDisponibles = misTareas;
        }

        return View("Explorar");
    }

    public IActionResult RealizarAyudas(string mensaje = "")
    {
        Usuarios joven = ObtenerUsuarioSesion();
        if (joven == null)
        {
            return RedirectToAction("Login", "Home");
        }
        if (!joven.TipoUsuario)
        {
            return RedirectToAction("PrincipalAdulto", "Home");
        }

        List<Tareas> listaOriginal = BD.ObtenerTareasPendientes(joven.IdUsuario);
        Dictionary<int, int> distanciasTareas = new Dictionary<int, int>();

        foreach (Tareas t in listaOriginal)
        {
            int km = CalcularDistanciaTareaKm(joven, t);
            distanciasTareas.Add(t.IdTarea, km);
        }

        for (int i = 0; i < listaOriginal.Count - 1; i++)
        {
            for (int j = i + 1; j < listaOriginal.Count; j++)
            {
                int distI = distanciasTareas[listaOriginal[i].IdTarea];
                int distJ = distanciasTareas[listaOriginal[j].IdTarea];
                if (distJ < distI)
                {
                    Tareas temp = listaOriginal[i];
                    listaOriginal[i] = listaOriginal[j];
                    listaOriginal[j] = temp;
                }
            }
        }

        ViewBag.Joven = joven;
        if (mensaje != "")
        {
            ViewBag.SuccessMessage = mensaje;
        }
        ViewBag.TareasPendientes = listaOriginal;
        ViewBag.DistanciasTareas = distanciasTareas;
        ViewBag.EstadosPostulacion = BD.ObtenerEstadosPostulacionesJoven(joven.IdUsuario);
        return View("RealizarAyudas");
    }

    private int CalcularDistanciaTareaKm(Usuarios joven, Tareas t)
    {
        string barrioJoven = "";
        if (joven != null && joven.Barrio != null)
        {
            barrioJoven = joven.Barrio.ToLower();
        }
        string barrioTarea = "";
        if (t.Barrio != null)
        {
            barrioTarea = t.Barrio.ToLower();
        }

        if (barrioJoven != "" && barrioTarea != "" && barrioJoven == barrioTarea)
        {
            return 2;
        }

        int resto = t.IdTarea % 10;
        if (resto < 0)
        {
            resto = resto * -1;
        }
        return 3 + resto;
    }

    public IActionResult DetalleAyuda(int idTarea)
    {
        Usuarios joven = ObtenerUsuarioSesion();
        if (joven == null)
        {
            return RedirectToAction("Login", "Home");
        }
        if (!joven.TipoUsuario)
        {
            return RedirectToAction("PrincipalAdulto", "Home");
        }

        Tareas tarea = BD.ObtenerTareaPorId(idTarea);
        if (tarea == null)
        {
            return RedirectToAction("RealizarAyudas", "Home");
        }

        Usuarios solicitante = BD.ObtenerUsuarioPorId(tarea.IdUsuarioSolicitante);
        ViewBag.Tarea = tarea;
        ViewBag.Solicitante = solicitante;
        return View("DetalleAyuda");
    }

    [HttpGet]
    public IActionResult ObtenerDetalleTareaJson(int idTarea)
    {
        if (ObtenerUsuarioSesion() == null)
        {
            return Json(null);
        }

        Tareas tarea = BD.ObtenerTareaPorId(idTarea);
        if (tarea == null)
        {
            return Json(null);
        }

        Usuarios solicitante = BD.ObtenerUsuarioPorId(tarea.IdUsuarioSolicitante);
        DateTime fechaMostrar = tarea.FechaCreacion;
        if (tarea.DiaTarea.Year > 1)
        {
            fechaMostrar = tarea.DiaTarea;
        }

        string nombreSolicitante = "Adulto mayor";
        string fotoSolicitante = "/img/user.svg";
        if (solicitante != null)
        {
            nombreSolicitante = solicitante.Nombre + " " + solicitante.Apellido;
            if (nombreSolicitante.Replace(" ", "") == "")
            {
                nombreSolicitante = solicitante.Email;
            }
            if (solicitante.FotoPerfil != null && solicitante.FotoPerfil != "")
            {
                fotoSolicitante = solicitante.FotoPerfil;
            }
        }

        string[] diasSemana = new string[7];
        diasSemana[0] = "Domingo";
        diasSemana[1] = "Lunes";
        diasSemana[2] = "Martes";
        diasSemana[3] = "Miércoles";
        diasSemana[4] = "Jueves";
        diasSemana[5] = "Viernes";
        diasSemana[6] = "Sábado";

        string nombreDia = diasSemana[(int)fechaMostrar.DayOfWeek];
        string fechaDiaTexto = nombreDia + " " + fechaMostrar.ToString("dd/MM");

        string aclaracionBruta = "";
        if (tarea.Aclaracion != null)
        {
            aclaracionBruta = tarea.Aclaracion;
        }
        string aclaracionLimpia = aclaracionBruta;
        int idxParentesis = aclaracionBruta.IndexOf(" (Día y hora: ");
        if (idxParentesis >= 0)
        {
            aclaracionLimpia = aclaracionBruta.Substring(0, idxParentesis);
        }
        else if (aclaracionBruta.IndexOf("Día y hora: ") == 0)
        {
            aclaracionLimpia = "";
        }

        string montoTexto = "5000";
        if (tarea.Monto > 0)
        {
            montoTexto = tarea.Monto.ToString("0");
        }

        string horaTexto = fechaMostrar.ToString("HH:mm");
        if (tarea.HoraTarea.TotalMinutes > 0)
        {
            horaTexto = tarea.HoraTarea.ToString(@"hh\:mm");
        }

        string barrioTexto = "";
        if (tarea.Barrio != null)
        {
            barrioTexto = tarea.Barrio;
        }

        return Json(new
        {
            idTarea = tarea.IdTarea,
            titulo = tarea.Titulo,
            descripcion = tarea.Descripcion,
            lugar = tarea.Lugar,
            barrio = barrioTexto,
            monto = montoTexto,
            fecha = fechaMostrar.ToString("dd/MM/yyyy"),
            fechaDia = fechaDiaTexto,
            hora = horaTexto,
            aclaracion = aclaracionLimpia,
            nombreSolicitante = nombreSolicitante,
            fotoSolicitante = fotoSolicitante
        });
    }

    [HttpPost]
    public IActionResult EliminarTareaRechazada(int idTarea)
    {
        Usuarios joven = ObtenerUsuarioSesion();
        if (joven == null || !joven.TipoUsuario)
        {
            return Json(new { ok = false });
        }

        bool ok = BD.RechazarTarea(idTarea, joven.IdUsuario);
        return Json(new { ok = ok });
    }

    [HttpPost]
    public IActionResult FinalizarTareaAceptada(int idTarea)
    {
        Usuarios joven = ObtenerUsuarioSesion();
        if (joven == null || !joven.TipoUsuario)
        {
            return Json(new { ok = false });
        }

        bool ok = BD.FinalizarTareaAceptada(idTarea, joven.IdUsuario);
        return Json(new { ok = ok });
    }

    [HttpPost]
    public IActionResult Postularse(int idTarea)
    {
        Usuarios joven = ObtenerUsuarioSesion();
        if (joven == null)
        {
            return RedirectToAction("Login", "Home");
        }
        if (!joven.TipoUsuario)
        {
            return RedirectToAction("PrincipalAdulto", "Home");
        }

        Postulaciones nuevaPostulacion = new Postulaciones(joven.IdUsuario, idTarea, "Pendiente", 0, true);
        BD.GuardarPostulacion(nuevaPostulacion);

        return RedirectToAction("Postulados", "Home", new { idTarea = idTarea });
    }

    [HttpPost]
    public IActionResult PostularseJson(int idTarea)
    {
        Usuarios joven = ObtenerUsuarioSesion();
        if (joven == null || !joven.TipoUsuario)
        {
            return Json(null);
        }

        Postulaciones nuevaPostulacion = new Postulaciones(joven.IdUsuario, idTarea, "Pendiente", 0, true);
        BD.GuardarPostulacion(nuevaPostulacion);

        Tareas tarea = BD.ObtenerTareaPorId(idTarea);
        List<Usuarios> postulados = BD.ObtenerPostuladosPorTarea(idTarea);

        string titulo = "esta ayuda";
        if (tarea != null)
        {
            titulo = tarea.Titulo;
        }

        return Json(new
        {
            tituloTarea = titulo,
            postulados = postulados
        });
    }

    public IActionResult Postulados(int idTarea)
    {
        Usuarios joven = ObtenerUsuarioSesion();
        if (joven == null)
        {
            return RedirectToAction("Login", "Home");
        }
        if (!joven.TipoUsuario)
        {
            return RedirectToAction("PrincipalAdulto", "Home");
        }

        Tareas tarea = BD.ObtenerTareaPorId(idTarea);
        ViewBag.Tarea = tarea;
        ViewBag.Postulados = BD.ObtenerPostuladosPorTarea(idTarea);
        return View("Postulados");
    }

    [HttpPost]
    public IActionResult AceptarTarea(int idTarea)
    {
        Usuarios joven = ObtenerUsuarioSesion();
        if (joven == null)
        {
            return RedirectToAction("Login", "Home");
        }
        if (!joven.TipoUsuario)
        {
            return RedirectToAction("PrincipalAdulto", "Home");
        }

        BD.AceptarTarea(idTarea);
        return RedirectToAction("RealizarAyudas", "Home", new { mensaje = "¡Aceptaste la ayuda correctamente!" });
    }

    [HttpPost]
    public IActionResult RechazarTarea(int idTarea)
    {
        Usuarios joven = ObtenerUsuarioSesion();
        if (joven == null || !joven.TipoUsuario)
        {
            return Json(new { ok = false });
        }

        bool ok = BD.RechazarTarea(idTarea, joven.IdUsuario);
        return Json(new { ok = ok });
    }

    public IActionResult MisAyudas(string mensaje = "")
    {
        Usuarios adulto = ObtenerUsuarioSesion();
        if (adulto == null)
        {
            return RedirectToAction("Login", "Home");
        }
        if (adulto.TipoUsuario)
        {
            return RedirectToAction("PrincipalJoven", "Home");
        }

        List<Tareas> misTareas = BD.ObtenerTareasPorSolicitante(adulto.IdUsuario);

        Dictionary<int, int> cantidadPostulantes = new Dictionary<int, int>();
        Dictionary<int, Usuarios> jovenesAsignados = new Dictionary<int, Usuarios>();
        Dictionary<int, bool> tareasFinalizadas = new Dictionary<int, bool>();

        foreach (Tareas tarea in misTareas)
        {
            if (!cantidadPostulantes.ContainsKey(tarea.IdTarea))
            {
                cantidadPostulantes.Add(tarea.IdTarea, BD.ContarPostulantes(tarea.IdTarea));
            }
            if (!jovenesAsignados.ContainsKey(tarea.IdTarea))
            {
                jovenesAsignados.Add(tarea.IdTarea, BD.ObtenerJovenAceptadoPorTarea(tarea.IdTarea));
            }
            if (!tareasFinalizadas.ContainsKey(tarea.IdTarea))
            {
                tareasFinalizadas.Add(tarea.IdTarea, BD.EsTareaFinalizada(tarea.IdTarea));
            }
        }

        ViewBag.Adulto = adulto;
        if (mensaje != "")
        {
            ViewBag.SuccessMessage = mensaje;
        }
        ViewBag.MisTareas = misTareas;
        ViewBag.CantidadPostulantes = cantidadPostulantes;
        ViewBag.JovenesAsignados = jovenesAsignados;
        ViewBag.TareasFinalizadas = tareasFinalizadas;
        return View("MisAyudas");
    }

    [HttpPost]
    public IActionResult EliminarTareaFinalizada(int idTarea)
    {
        Usuarios adulto = ObtenerUsuarioSesion();
        if (adulto == null)
        {
            return RedirectToAction("Login", "Home");
        }
        if (adulto.TipoUsuario)
        {
            return RedirectToAction("PrincipalJoven", "Home");
        }

        Tareas tarea = BD.ObtenerTareaPorId(idTarea);
        if (tarea == null || tarea.IdUsuarioSolicitante != adulto.IdUsuario)
        {
            return RedirectToAction("MisAyudas", "Home");
        }

        BD.EliminarTareaFinalizadaAdulto(idTarea);
        return RedirectToAction("MisAyudas", "Home", new { mensaje = "La tarea finalizada fue eliminada de tu lista." });
    }

    [HttpGet]
    public IActionResult TicketAyuda(int idTarea)
    {
        Usuarios adulto = ObtenerUsuarioSesion();
        if (adulto == null)
        {
            return RedirectToAction("Login", "Home");
        }
        if (adulto.TipoUsuario)
        {
            return RedirectToAction("PrincipalJoven", "Home");
        }

        Tareas tarea = BD.ObtenerTareaPorId(idTarea);
        if (tarea == null || tarea.IdUsuarioSolicitante != adulto.IdUsuario)
        {
            return RedirectToAction("MisAyudas", "Home");
        }

        Usuarios jovenAsignado = BD.ObtenerJovenAceptadoPorTarea(idTarea);
        Usuarios solicitante = BD.ObtenerUsuarioPorId(tarea.IdUsuarioSolicitante);
        if (solicitante == null)
        {
            solicitante = adulto;
        }

        ViewBag.Adulto = adulto;
        ViewBag.Solicitante = solicitante;
        ViewBag.Tarea = tarea;
        ViewBag.JovenAsignado = jovenAsignado;
        return View("TicketAyuda");
    }

    [HttpPost]
    public IActionResult GuardarValoracion(int idTarea, double Calificacion = 5, bool Puntual = true, string Comentario = "")
    {
        Usuarios adulto = ObtenerUsuarioSesion();
        if (adulto == null)
        {
            return RedirectToAction("Login", "Home");
        }
        if (adulto.TipoUsuario)
        {
            return RedirectToAction("PrincipalJoven", "Home");
        }

        Tareas tarea = BD.ObtenerTareaPorId(idTarea);
        if (tarea == null || tarea.IdUsuarioSolicitante != adulto.IdUsuario)
        {
            return RedirectToAction("MisAyudas", "Home");
        }

        if (Calificacion < 1) Calificacion = 1;
        if (Calificacion > 5) Calificacion = 5;

        BD.GuardarValoracionJoven(idTarea, Calificacion, Puntual);

        Usuarios joven = BD.ObtenerJovenAceptadoPorTarea(idTarea);
        string nombreJoven = "el joven";
        if (joven != null)
        {
            nombreJoven = joven.Nombre + " " + joven.Apellido;
            if (nombreJoven.Replace(" ", "") == "")
            {
                nombreJoven = joven.Email;
            }
        }

        return RedirectToAction("MisAyudas", "Home", new { mensaje = "¡Gracias por tu devolución! Se guardó la calificación para " + nombreJoven + "." });
    }

    [HttpPost]
    public IActionResult CancelarTarea(int idTarea)
    {
        Usuarios adulto = ObtenerUsuarioSesion();
        if (adulto == null)
        {
            return RedirectToAction("Login", "Home");
        }
        if (adulto.TipoUsuario)
        {
            return RedirectToAction("PrincipalJoven", "Home");
        }

        Tareas tarea = BD.ObtenerTareaPorId(idTarea);
        if (tarea == null || tarea.IdUsuarioSolicitante != adulto.IdUsuario)
        {
            return RedirectToAction("MisAyudas", "Home", new { mensaje = "No se encontró la ayuda." });
        }

        bool cancelada = BD.CancelarTarea(idTarea);
        string mensaje = "No se puede cancelar una ayuda que ya tiene postulantes.";
        if (cancelada)
        {
            mensaje = "La ayuda fue cancelada.";
        }
        return RedirectToAction("MisAyudas", "Home", new { mensaje = mensaje });
    }

    [HttpGet]
    public IActionResult ModificarTarea(int idTarea)
    {
        Usuarios adulto = ObtenerUsuarioSesion();
        if (adulto == null)
        {
            return RedirectToAction("Login", "Home");
        }
        if (adulto.TipoUsuario)
        {
            return RedirectToAction("PrincipalJoven", "Home");
        }

        Tareas tarea = BD.ObtenerTareaPorId(idTarea);
        if (tarea == null || tarea.IdUsuarioSolicitante != adulto.IdUsuario)
        {
            return RedirectToAction("MisAyudas", "Home", new { mensaje = "No se encontró la ayuda." });
        }

        if (!tarea.Estado || BD.ContarPostulantes(idTarea) > 0 || BD.ObtenerJovenAceptadoPorTarea(idTarea) != null)
        {
            return RedirectToAction("MisAyudas", "Home", new { mensaje = "No se puede modificar una ayuda que ya tiene postulantes o aceptados." });
        }

        ViewBag.Tarea = tarea;
        return View("ModificarTarea");
    }

    [HttpPost]
    public IActionResult ModificarTarea(int IdTarea, string Titulo = "", string Descripcion = "", string Lugar = "", string Barrio = "", string TipoLugar = "Casa", string DiaHora = "", string Aclaracion = "", string ConfirmacionModificar = "")
    {
        Usuarios adulto = ObtenerUsuarioSesion();
        if (adulto == null)
        {
            return RedirectToAction("Login", "Home");
        }
        if (adulto.TipoUsuario)
        {
            return RedirectToAction("PrincipalJoven", "Home");
        }

        Tareas actual = BD.ObtenerTareaPorId(IdTarea);
        if (actual == null || actual.IdUsuarioSolicitante != adulto.IdUsuario)
        {
            return RedirectToAction("MisAyudas", "Home", new { mensaje = "No se encontró la ayuda." });
        }

        if (!actual.Estado || BD.ContarPostulantes(IdTarea) > 0 || BD.ObtenerJovenAceptadoPorTarea(IdTarea) != null)
        {
            return RedirectToAction("MisAyudas", "Home", new { mensaje = "No se puede modificar una ayuda que ya tiene postulantes o aceptados." });
        }

        if (Titulo == null) Titulo = "";
        if (Descripcion == null) Descripcion = "";
        if (Lugar == null) Lugar = "";
        if (Barrio == null) Barrio = "";
        if (TipoLugar == null || TipoLugar == "") TipoLugar = "Casa";
        if (DiaHora == null) DiaHora = "";
        if (Aclaracion == null) Aclaracion = "";
        if (ConfirmacionModificar == null) ConfirmacionModificar = "";

        string confirmacionLimpia = ConfirmacionModificar.Replace(" ", "").ToLower();

        actual.Titulo = Titulo;
        actual.Descripcion = Descripcion;
        actual.Lugar = Lugar;
        actual.Barrio = Barrio;
        actual.TipoLugar = TipoLugar;
        actual.Aclaracion = Aclaracion;
        ViewBag.DiaHoraTexto = DiaHora;
        ViewBag.Tarea = actual;

        if (Titulo == "" || Descripcion == "" || Lugar == "" || DiaHora == "")
        {
            ViewBag.ErrorMessage = "Por favor completá el título, la descripción, el lugar y el día/hora.";
            return View("ModificarTarea");
        }

        if (confirmacionLimpia != "modificar")
        {
            ViewBag.ErrorMessage = "Debés escribir la palabra \"modificar\" abajo de todo para confirmar los cambios.";
            return View("ModificarTarea");
        }

        if (Titulo.Length > 50) Titulo = Titulo.Substring(0, 50);
        if (Descripcion.Length > 500) Descripcion = Descripcion.Substring(0, 500);
        if (Lugar.Length > 50) Lugar = Lugar.Substring(0, 50);
        if (Barrio.Length > 50) Barrio = Barrio.Substring(0, 50);
        if (TipoLugar.Length > 50) TipoLugar = TipoLugar.Substring(0, 50);

        DateTime fechaTarea = DateTime.Now.Date;
        if (actual.DiaTarea.Year > 1)
        {
            fechaTarea = actual.DiaTarea.Date;
        }
        TimeSpan horaTarea = new TimeSpan(17, 30, 0);
        if (actual.HoraTarea.TotalMinutes > 0)
        {
            horaTarea = actual.HoraTarea;
        }

        if (DateTime.TryParse(DiaHora, out DateTime fechaParseada))
        {
            fechaTarea = fechaParseada.Date;
            horaTarea = fechaParseada.TimeOfDay;
        }

        string textoFechaHora = fechaTarea.ToString("dd/MM/yyyy") + " " + horaTarea.ToString(@"hh\:mm");
        string aclaracionFinal = "";
        if (Aclaracion == "")
        {
            aclaracionFinal = "Día y hora: " + textoFechaHora;
        }
        else
        {
            aclaracionFinal = Aclaracion + " (Día y hora: " + textoFechaHora + ")";
        }
        if (aclaracionFinal.Length > 500)
        {
            aclaracionFinal = aclaracionFinal.Substring(0, 500);
        }

        actual.Titulo = Titulo;
        actual.Descripcion = Descripcion;
        actual.Lugar = Lugar;
        actual.Barrio = Barrio;
        actual.TipoLugar = TipoLugar;
        actual.DiaTarea = fechaTarea;
        actual.HoraTarea = horaTarea;
        actual.Aclaracion = aclaracionFinal;

        bool modificado = BD.ModificarTarea(actual);
        if (!modificado)
        {
            ViewBag.ErrorMessage = "No se pudo modificar la ayuda en la base de datos.";
            return View("ModificarTarea");
        }

        return RedirectToAction("MisAyudas", "Home", new { mensaje = "¡La ayuda (" + Titulo + ") fue modificada correctamente!" });
    }

    public IActionResult PostulantesTarea(int idTarea)
    {
        Usuarios adulto = ObtenerUsuarioSesion();
        if (adulto == null)
        {
            return RedirectToAction("Login", "Home");
        }
        if (adulto.TipoUsuario)
        {
            return RedirectToAction("PrincipalJoven", "Home");
        }

        Tareas tarea = BD.ObtenerTareaPorId(idTarea);
        if (tarea == null || tarea.IdUsuarioSolicitante != adulto.IdUsuario)
        {
            return RedirectToAction("MisAyudas", "Home");
        }

        ViewBag.Tarea = tarea;
        ViewBag.Postulantes = BD.ObtenerPostuladosPorTarea(idTarea);
        return View("PostulantesTarea");
    }

    [HttpGet]
    public IActionResult ObtenerDetallePostulanteJson(int idUsuario)
    {
        if (ObtenerUsuarioSesion() == null)
        {
            return Json(null);
        }

        Usuarios joven = BD.ObtenerUsuarioPorId(idUsuario);
        if (joven == null)
        {
            return Json(null);
        }

        string nombre = joven.Nombre + " " + joven.Apellido;
        if (nombre.Replace(" ", "") == "")
        {
            nombre = joven.Email;
        }

        string foto = "/img/user.svg";
        if (joven.FotoPerfil != null && joven.FotoPerfil != "")
        {
            foto = joven.FotoPerfil;
        }

        return Json(new
        {
            idUsuario = joven.IdUsuario,
            nombre = nombre,
            foto = foto,
            ayudasRealizadas = BD.ContarAyudasRealizadas(idUsuario),
            valoracion = BD.ObtenerValoracionPromedio(idUsuario),
            puntual = BD.EsPuntual(idUsuario)
        });
    }

    [HttpPost]
    public IActionResult AceptarPostulante(int idTarea, int idUsuario)
    {
        Usuarios adulto = ObtenerUsuarioSesion();
        if (adulto == null || adulto.TipoUsuario)
        {
            return Json(new { ok = false });
        }

        Tareas tarea = BD.ObtenerTareaPorId(idTarea);
        if (tarea == null || tarea.IdUsuarioSolicitante != adulto.IdUsuario)
        {
            return Json(new { ok = false });
        }

        bool ok = BD.AceptarPostulante(idTarea, idUsuario);
        return Json(new { ok = ok });
    }

    [HttpPost]
    public IActionResult RechazarPostulante(int idTarea, int idUsuario)
    {
        Usuarios adulto = ObtenerUsuarioSesion();
        if (adulto == null || adulto.TipoUsuario)
        {
            return Json(new { ok = false });
        }

        Tareas tarea = BD.ObtenerTareaPorId(idTarea);
        if (tarea == null || tarea.IdUsuarioSolicitante != adulto.IdUsuario)
        {
            return Json(new { ok = false });
        }

        bool ok = BD.RechazarPostulante(idTarea, idUsuario);
        return Json(new { ok = ok });
    }

    private Usuarios ObtenerUsuarioSesion()
    {
        int? idSesion = HttpContext.Session.GetInt32("IdUsuarioLogueado");
        if (idSesion != null)
        {
            Usuarios u = BD.ObtenerUsuarioPorId(idSesion.Value);
            if (u != null)
            {
                ViewBag.UsuarioSesion = u;
                if (u.FotoPerfil != null && u.FotoPerfil != "")
                {
                    ViewBag.FotoPerfilSesion = u.FotoPerfil;
                }
                else
                {
                    ViewBag.FotoPerfilSesion = "/img/user.svg";
                }
                return u;
            }
        }

        string emailSesion = HttpContext.Session.GetString("EmailLogueado");
        if (emailSesion != null && emailSesion != "")
        {
            Usuarios u = BD.ObtenerUsuarioPorEmail(emailSesion);
            if (u != null)
            {
                ViewBag.UsuarioSesion = u;
                if (u.FotoPerfil != null && u.FotoPerfil != "")
                {
                    ViewBag.FotoPerfilSesion = u.FotoPerfil;
                }
                else
                {
                    ViewBag.FotoPerfilSesion = "/img/user.svg";
                }
                return u;
            }
        }

        return null;
    }

    public IActionResult CerrarSesion()
    {
        HttpContext.Session.Remove("EmailLogueado");
        HttpContext.Session.Remove("IdUsuarioLogueado");
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult VerDetalleAyudado(int idUsuario)
    {
        if (ObtenerUsuarioSesion() == null)
        {
            return Json(null);
        }

        Usuarios usuario = BD.ObtenerUsuarioPorId(idUsuario);
        return Json(usuario);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    private string GuardarArchivo(IFormFile archivo, string subcarpeta)
    {
        if (archivo == null || archivo.Length == 0)
        {
            return "";
        }

        string folderPath = Path.Combine(_environment.WebRootPath, subcarpeta);
        Directory.CreateDirectory(folderPath);

        string nombreArchivo = archivo.FileName.Replace(" ", "_");
        string fullPath = Path.Combine(folderPath, nombreArchivo);

        using (FileStream stream = System.IO.File.Create(fullPath))
        {
            archivo.CopyTo(stream);
        }

        return "/" + subcarpeta + "/" + nombreArchivo;
    }
}
