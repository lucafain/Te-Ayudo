using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using TeAyudo.Models;

namespace TeAyudo.Controllers;

public class HomeController : Controller
{
    private readonly IWebHostEnvironment _environment;

    public HomeController(IWebHostEnvironment environment)
    {
        _environment = environment;

        try
        {
            string rutaDestinoJoven = Path.Combine(_environment.WebRootPath, "img", "hero-joven.png");
            string rutaOrigenJoven = @"wwwroot/img/hero-joven.png";
            if (!System.IO.File.Exists(rutaDestinoJoven) && System.IO.File.Exists(rutaOrigenJoven))
            {
                System.IO.File.Copy(rutaOrigenJoven, rutaDestinoJoven, true);
            }

            string rutaDestinoAdulto = Path.Combine(_environment.WebRootPath, "img", "hero-adulto.png");
            string rutaOrigenAdulto = @"wwwroot/img/hero-adulto.png";
            if (!System.IO.File.Exists(rutaDestinoAdulto) && System.IO.File.Exists(rutaOrigenAdulto))
            {
                System.IO.File.Copy(rutaOrigenAdulto, rutaDestinoAdulto, true);
            }

            string rutaDestinoInicio = Path.Combine(_environment.WebRootPath, "img", "hero-inicio.png");
            string rutaOrigenInicio = @"wwwroot/img/hero-inicio.png";
            if (System.IO.File.Exists(rutaOrigenInicio))
            {
                System.IO.File.Copy(rutaOrigenInicio, rutaDestinoInicio, true);
            }
        }
        catch
        {
        }
    }

    public IActionResult Index()
    {
        // Slide 13, 14 y 15: Colecciones (Dictionary con ContainsKey, Arrays y Listas)
        Dictionary<string, string> textosHome = new Dictionary<string, string>();
        textosHome.Add("Titulo", "Dónde encontrás lo que buscás");
        textosHome.Add("AyudaTitulo", "Recibí la ayuda que buscás");
        textosHome.Add("AyudaTexto", "Seleccionamos a quienes pueden acompañarte.");
        textosHome.Add("TrabajoTitulo", "¿Sos joven y querés ganar experiencia laboral?");
        textosHome.Add("TrabajoTexto", "Formá parte de las historias de Te Ayudo.");

        // Slide 15: Verificar siempre si existe la clave antes de acceder en un Dictionary
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

        // Slide 14: Array (en la definición indicamos cuántos elementos va a tener)
        string[] arrayPresentacion = new string[2];
        arrayPresentacion[0] = "Jóvenes brindan ayuda a adultos que lo requieran a cambio de inclusión laboral";
        arrayPresentacion[1] = "Adultos reciben asistencia a cambio de montos accesibles";

        List<string> listaPresentacion = new List<string>();
        foreach (string item in arrayPresentacion)
        {
            listaPresentacion.Add(item);
        }
        ViewBag.Presentacion = listaPresentacion;

        // Slide 14: Lista (tamaño ilimitado de elementos)
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

    public IActionResult Login(string? mensaje = null)
    {
        ViewBag.SuccessMessage = mensaje;
        return View();
    }

    [HttpPost]
    public IActionResult Login(string? Email = "", string? Contrasena = "")
    {
        // Slide 12: Métodos y propiedades para tipos de datos STRING (.Length, .IndexOf, .Replace, .ToLower)
        if (string.IsNullOrEmpty(Email) || Email.Length == 0)
        {
            ViewBag.ErrorMessage = "Debe completar el email.";
            return View("Login");
        }

        if (Email.IndexOf("@") == -1)
        {
            ViewBag.ErrorMessage = "El email ingresado no es válido.";
            return View("Login");
        }

        if (string.IsNullOrEmpty(Contrasena) || Contrasena.Length == 0)
        {
            ViewBag.ErrorMessage = "Debe completar la contraseña.";
            return View("Login");
        }

        string emailLimpio = Email.Replace(" ", "").ToLower();
        Usuarios? usuario = BD.ObtenerUsuarioPorCredenciales(emailLimpio, Contrasena);

        if (usuario == null)
        {
            ViewBag.ErrorMessage = "Email o contraseña incorrectos.";
            return View("Login");
        }

        // Slide 104 y 105: Sessions (almacenamiento clave-valor en servidor)
        HttpContext.Session.SetString("EmailLogueado", usuario.Email);
        HttpContext.Session.SetInt32("IdUsuarioLogueado", usuario.IdUsuario);
        return RedirectToAction("Perfil", "Home");
    }

    public IActionResult SolicitarTarea()
    {
        Usuarios? usuario = ObtenerUsuarioSesion();
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
    public IActionResult SolicitarTarea(string? Titulo = "", string? Descripcion = "", string? Lugar = "", string? Barrio = "", string? DiaHora = "", string? Aclaracion = "", string? TipoLugar = "Domicilio", double Monto = 0)
    {
        Usuarios? usuarioActual = ObtenerUsuarioSesion();
        if (usuarioActual == null)
        {
            return RedirectToAction("Login", "Home");
        }
        if (usuarioActual.TipoUsuario)
        {
            return RedirectToAction("PrincipalJoven", "Home");
        }

        ViewBag.UsuarioActual = usuarioActual;
        int idSolicitante = usuarioActual.IdUsuario;

        if (string.IsNullOrEmpty(Titulo) || string.IsNullOrEmpty(Descripcion) || string.IsNullOrEmpty(Lugar) || string.IsNullOrEmpty(DiaHora))
        {
            ViewBag.ErrorMessage = "Por favor complete el nombre de la ayuda, la descripción, el lugar y la fecha/hora.";
            return View();
        }

        // Slide 12: Métodos de string (.Length, .Substring) para respetar varchar(50) de la BD
        string tituloLimpio = Titulo.Length > 50 ? Titulo.Substring(0, 50) : Titulo;
        string descripcionLimpia = Descripcion.Length > 500 ? Descripcion.Substring(0, 500) : Descripcion;
        string lugarLimpio = Lugar.Length > 50 ? Lugar.Substring(0, 50) : Lugar;
        string barrioTexto = (Barrio ?? "").Trim();
        string barrioLimpio = barrioTexto.Length > 50 ? barrioTexto.Substring(0, 50) : barrioTexto;
        string tipoLugarTexto = string.IsNullOrEmpty(TipoLugar) ? "Domicilio" : TipoLugar;
        string tipoLugarLimpio = tipoLugarTexto.Length > 50 ? tipoLugarTexto.Substring(0, 50) : tipoLugarTexto;

        DateTime fechaTarea = DateTime.Now;
        TimeSpan horaTarea = new TimeSpan(10, 0, 0);

        if (DateTime.TryParse(DiaHora, out DateTime fechaHoraCompleta))
        {
            fechaTarea = fechaHoraCompleta.Date;
            horaTarea = fechaHoraCompleta.TimeOfDay;
        }

        string aclaracionFinal = Aclaracion ?? "";
        string textoFechaHora = fechaTarea.ToString("dd/MM/yyyy") + " " + horaTarea.ToString(@"hh\:mm");
        aclaracionFinal = string.IsNullOrEmpty(aclaracionFinal)
            ? "Día y hora: " + textoFechaHora
            : aclaracionFinal + " (Día y hora: " + textoFechaHora + ")";

        // Slide 46: Tomar los datos del form y guardarlos en un objeto
        Tareas nuevaTarea = new Tareas();
        nuevaTarea.Titulo = tituloLimpio;
        nuevaTarea.Descripcion = descripcionLimpia;
        nuevaTarea.Estado = true; // true (1) = Tarea activa
        nuevaTarea.FechaCreacion = DateTime.Now;
        nuevaTarea.Lugar = lugarLimpio;
        nuevaTarea.Barrio = barrioLimpio;
        nuevaTarea.DiaTarea = fechaTarea;
        nuevaTarea.HoraTarea = horaTarea;
        nuevaTarea.Aclaracion = aclaracionFinal;
        nuevaTarea.Monto = Monto;
        nuevaTarea.TipoLugar = tipoLugarLimpio;
        nuevaTarea.IdUsuarioSolicitante = idSolicitante;

        bool resultado = false;
        try
        {
            resultado = BD.GuardarTarea(nuevaTarea);
        }
        catch (Exception ex)
        {
            ViewBag.ErrorMessage = "No se pudo guardar la tarea: " + ex.Message;
            return View();
        }

        if (!resultado)
        {
            ViewBag.ErrorMessage = "No se pudo guardar la tarea en la base de datos.";
            return View();
        }

        return RedirectToAction("MisAyudas", "Home", new { mensaje = "¡Tu solicitud de ayuda (" + tituloLimpio + ") ha sido publicada!" });
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
    public IActionResult GuardarRegistroMayor(string? Nombre = "", string? Apellido = "", string? Email = "", string? Provincia = "", string? Calle = "", int Direccion = 0, string? CodigoPostal = "", string? Telefono = "", string? Contrasena = "", IFormFile? FotoPerfil = null, bool AceptaTerminos = true)
    {
        // Validación con métodos string (Slide 12)
        if (string.IsNullOrEmpty(Nombre) || string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Contrasena))
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

        string codigoPostalFinal = !string.IsNullOrEmpty(CodigoPostal) ? CodigoPostal : (Telefono ?? "");
        if (codigoPostalFinal.Length > 8)
        {
            codigoPostalFinal = codigoPostalFinal.Substring(0, 8);
        }

        // Slide 46: Tomar los datos del form y guardarlos en un objeto
        // tipoUsuario = false (0) es Adulto
        Usuarios user = new Usuarios();
        user.Nombre = Nombre;
        user.Apellido = Apellido ?? "";
        user.Email = Email.Replace(" ", "").ToLower();
        user.Provincia = Provincia ?? "";
        user.Calle = Calle ?? "";
        user.Direccion = Direccion;
        user.CodigoPostal = codigoPostalFinal;
        user.FechaRegistro = DateTime.Now;
        user.TipoUsuario = false; // false = Adulto
        user.Contrasena = Contrasena;
        user.AceptaTerminos = AceptaTerminos;
        user.FotoPerfil = GuardarArchivo(FotoPerfil, "uploads/perfiles") ?? "/img/user.svg";

        bool resultado = BD.GuardarUsuario(user);

        if (resultado)
        {
            Usuarios? usuarioGuardado = BD.ObtenerUsuarioPorEmail(user.Email);
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
    public IActionResult GuardarRegistroJoven(string? Nombre = "", string? Apellido = "", string? Email = "", string? Provincia = "", string? Calle = "", int Direccion = 0, string? CodigoPostal = "", string? Contrasena = "", IFormFile? FotoPerfil = null, bool AceptaTerminos = false)
    {
        // Validación en controller
        if (string.IsNullOrEmpty(Nombre) || string.IsNullOrEmpty(Apellido) || string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Contrasena))
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

        // Slide 46: Guardar en objeto
        // tipoUsuario = true (1) es Joven
        Usuarios user = new Usuarios();
        user.Nombre = Nombre;
        user.Apellido = Apellido;
        user.Email = Email.Replace(" ", "").ToLower();
        user.Provincia = Provincia ?? "";
        user.Calle = Calle ?? "";
        user.Direccion = Direccion;
        user.CodigoPostal = CodigoPostal ?? "";
        user.FechaRegistro = DateTime.Now;
        user.TipoUsuario = true; // true = Joven
        user.Contrasena = Contrasena;
        user.AceptaTerminos = AceptaTerminos;
        user.FotoPerfil = GuardarArchivo(FotoPerfil, "uploads/perfiles") ?? "/img/user.svg";

        bool resultado = BD.GuardarUsuario(user);

        if (resultado)
        {
            Usuarios? usuarioGuardado = BD.ObtenerUsuarioPorEmail(user.Email);
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
    public IActionResult OlvideContrasena(string? Email = "")
    {
        if (string.IsNullOrEmpty(Email) || Email.IndexOf("@") == -1)
        {
            ViewBag.ErrorMessage = "Debe ingresar un email válido.";
            return View("olvideContraseña");
        }

        string emailLimpio = Email.Replace(" ", "").ToLower();
        Usuarios? usuario = BD.ObtenerUsuarioPorEmail(emailLimpio);

        if (usuario == null)
        {
            ViewBag.ErrorMessage = "No existe un usuario con ese email.";
            return View("olvideContraseña");
        }

        ViewBag.Contrasena = usuario.Contrasena;
        return View("MostrarContraseña");
    }

    public IActionResult Perfil(string? mensaje = null)
    {
        Usuarios? usuario = ObtenerUsuarioSesion();
        if (usuario == null)
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Home");
        }

        ViewBag.SuccessMessage = mensaje;
        ViewBag.Nombre = usuario.Nombre;
        ViewBag.Apellido = usuario.Apellido;
        ViewBag.Email = usuario.Email;
        ViewBag.Provincia = usuario.Provincia;
        ViewBag.Calle = usuario.Calle;
        ViewBag.Direccion = usuario.Direccion;
        ViewBag.CodigoPostal = usuario.CodigoPostal;
        ViewBag.EsJoven = usuario.TipoUsuario; // true = Joven, false = Adulto
        ViewBag.TipoUsuario = usuario.TipoUsuario ? "Joven" : "Adulto mayor";
        ViewBag.FotoPerfil = usuario.FotoPerfil;

        return View("perfil");
    }

    [HttpPost]
    public IActionResult ActualizarFotoPerfil(IFormFile? FotoPerfil = null)
    {
        Usuarios? usuario = ObtenerUsuarioSesion();
        if (usuario == null)
        {
            return RedirectToAction("Login", "Home");
        }

        string? rutaNueva = GuardarArchivo(FotoPerfil, "uploads/perfiles");
        if (!string.IsNullOrEmpty(rutaNueva))
        {
            BD.ActualizarFotoPerfil(usuario.IdUsuario, rutaNueva);
            return RedirectToAction("Perfil", "Home", new { mensaje = "¡Foto de perfil actualizada y guardada en la base de datos!" });
        }

        return RedirectToAction("Perfil", "Home");
    }

    public IActionResult PrincipalJoven()
    {
        Usuarios? joven = ObtenerUsuarioSesion();
        if (joven == null)
        {
            return RedirectToAction("Login", "Home");
        }
        if (!joven.TipoUsuario)
        {
            return RedirectToAction("PrincipalAdulto", "Home");
        }

        int idJovenLogueado = joven.IdUsuario;

        List<Tareas> tareasDisponibles = BD.ObtenerTareasDisponibles();
        Dictionary<int, Usuarios> solicitantes = new Dictionary<int, Usuarios>();
        foreach (Tareas tarea in tareasDisponibles)
        {
            if (!solicitantes.ContainsKey(tarea.IdUsuarioSolicitante))
            {
                Usuarios? solicitante = BD.ObtenerUsuarioPorId(tarea.IdUsuarioSolicitante);
                if (solicitante != null)
                {
                    solicitantes.Add(tarea.IdUsuarioSolicitante, solicitante);
                }
            }
        }

        ViewBag.Joven = joven;
        ViewBag.UltimosAyudados = BD.ObtenerUltimosAyudados(idJovenLogueado);
        ViewBag.TareasDisponibles = tareasDisponibles;
        ViewBag.Solicitantes = solicitantes;

        return View("PrincipalJoven");
    }

    public IActionResult PrincipalAdulto()
    {
        Usuarios? adulto = ObtenerUsuarioSesion();
        if (adulto == null)
        {
            return RedirectToAction("Login", "Home");
        }
        if (adulto.TipoUsuario)
        {
            return RedirectToAction("PrincipalJoven", "Home");
        }

        int idAdultoLogueado = adulto.IdUsuario;

        ViewBag.Adulto = adulto;
        ViewBag.Jovenes = BD.ObtenerJovenesDisponibles();
        ViewBag.MisTareas = BD.ObtenerTareasPorSolicitante(idAdultoLogueado);

        return View("PrincipalAdulto");
    }

    public IActionResult Explorar()
    {
        Usuarios? usuario = ObtenerUsuarioSesion();
        if (usuario == null)
        {
            return RedirectToAction("Login", "Home");
        }

        int idLogueado = usuario.IdUsuario;

        ViewBag.Joven = usuario;
        ViewBag.UltimosAyudados = BD.ObtenerUltimosAyudados(idLogueado);
        ViewBag.TareasDisponibles = BD.ObtenerTareasDisponibles();

        return View("Explorar");
    }

    public IActionResult AyudasParaRealizar(string? mensaje = null)
    {
        Usuarios? joven = ObtenerUsuarioSesion();
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
            distanciasTareas[t.IdTarea] = km;
        }

        // Ordenar de menor a mayor distancia (más cerca primero) sin lambdas para evitar errores de Hot Reload
        for (int i = 0; i < listaOriginal.Count - 1; i++)
        {
            for (int j = i + 1; j < listaOriginal.Count; j++)
            {
                int distI = distanciasTareas[listaOriginal[i].IdTarea];
                int distJ = distanciasTareas[listaOriginal[j].IdTarea];
                if (distJ < distI || (distJ == distI && listaOriginal[j].IdTarea > listaOriginal[i].IdTarea))
                {
                    Tareas temp = listaOriginal[i];
                    listaOriginal[i] = listaOriginal[j];
                    listaOriginal[j] = temp;
                }
            }
        }

        ViewBag.Joven = joven;
        ViewBag.SuccessMessage = mensaje;
        ViewBag.TareasPendientes = listaOriginal;
        ViewBag.DistanciasTareas = distanciasTareas;
        ViewBag.EstadosPostulacion = BD.ObtenerEstadosPostulacionesJoven(joven.IdUsuario);
        return View("AyudasParaRealizar");
    }

    private int CalcularDistanciaTareaKm(Usuarios? joven, Tareas t)
    {
        Dictionary<string, double[]> coordsBarrios = new Dictionary<string, double[]>
        {
            { "almagro", new double[] { -34.6090, -58.4210 } },
            { "yatay", new double[] { -34.6095, -58.4265 } },
            { "yatai", new double[] { -34.6095, -58.4265 } },
            { "villa crespo", new double[] { -34.5980, -58.4420 } },
            { "araoz", new double[] { -34.5940, -58.4280 } },
            { "caballito", new double[] { -34.6180, -58.4420 } },
            { "avellaneda", new double[] { -34.6240, -58.4580 } },
            { "boyaca", new double[] { -34.6230, -58.4620 } },
            { "flores", new double[] { -34.6280, -58.4630 } },
            { "floresta", new double[] { -34.6310, -58.4830 } },
            { "palermo", new double[] { -34.5830, -58.4250 } },
            { "cordoba", new double[] { -34.5990, -58.3960 } },
            { "corrientes", new double[] { -34.6040, -58.4110 } },
            { "balvanera", new double[] { -34.6090, -58.4010 } },
            { "once", new double[] { -34.6080, -58.4060 } },
            { "abasto", new double[] { -34.6030, -58.4110 } },
            { "recoleta", new double[] { -34.5890, -58.3930 } },
            { "belgrano", new double[] { -34.5620, -58.4560 } },
            { "nuñez", new double[] { -34.5450, -58.4650 } },
            { "nunez", new double[] { -34.5450, -58.4650 } },
            { "colegiales", new double[] { -34.5750, -58.4480 } },
            { "chacarita", new double[] { -34.5880, -58.4540 } },
            { "paternal", new double[] { -34.5990, -58.4680 } },
            { "boedo", new double[] { -34.6260, -58.4170 } },
            { "san cristobal", new double[] { -34.6230, -58.4020 } },
            { "san telmo", new double[] { -34.6210, -58.3730 } },
            { "monserrat", new double[] { -34.6120, -58.3820 } },
            { "microcentro", new double[] { -34.6037, -58.3816 } },
            { "villa urquiza", new double[] { -34.5730, -58.4870 } },
            { "villa devoto", new double[] { -34.6010, -58.5120 } },
            { "villa del parque", new double[] { -34.6040, -58.4910 } },
            { "saavedra", new double[] { -34.5540, -58.4880 } },
            { "coghlan", new double[] { -34.5600, -58.4750 } },
            { "barracas", new double[] { -34.6420, -58.3780 } },
            { "la boca", new double[] { -34.6350, -58.3640 } },
            { "constitucion", new double[] { -34.6280, -58.3840 } },
            { "retiro", new double[] { -34.5910, -58.3780 } },
            { "puerto madero", new double[] { -34.6110, -58.3620 } },
            { "mataderos", new double[] { -34.6580, -58.5010 } },
            { "liniers", new double[] { -34.6430, -58.5210 } },
            { "parque patricios", new double[] { -34.6370, -58.4010 } },
            { "parque chacabuco", new double[] { -34.6350, -58.4380 } }
        };

        string textoJoven = ((joven?.Calle ?? "") + " " + (joven?.Provincia ?? "")).ToLower();
        double latJoven = -34.6095;
        double lonJoven = -58.4265;
        foreach (KeyValuePair<string, double[]> par in coordsBarrios)
        {
            if (textoJoven.Contains(par.Key))
            {
                latJoven = par.Value[0];
                lonJoven = par.Value[1];
                break;
            }
        }

        string textoTarea = ((t.Barrio ?? "") + " " + (t.Lugar ?? "")).ToLower();
        double latTarea = 0;
        double lonTarea = 0;
        foreach (KeyValuePair<string, double[]> par in coordsBarrios)
        {
            if (textoTarea.Contains(par.Key))
            {
                latTarea = par.Value[0];
                lonTarea = par.Value[1];
                break;
            }
        }

        if (latTarea != 0)
        {
            double dLat = (latTarea - latJoven) * 111.0;
            double dLon = (lonTarea - lonJoven) * 91.0;
            double dist = System.Math.Sqrt(dLat * dLat + dLon * dLon) * 1.35;
            int km = (int)System.Math.Round(dist);
            if (km < 1) km = 2;
            if (km > 15) km = 12;
            return km;
        }

        int[] distanciasBase = new int[] { 2, 3, 5, 10, 2, 5, 3, 4, 6 };
        return distanciasBase[System.Math.Abs(t.IdTarea) % distanciasBase.Length];
    }

    public IActionResult DetalleAyuda(int idTarea)
    {
        Usuarios? joven = ObtenerUsuarioSesion();
        if (joven == null)
        {
            return RedirectToAction("Login", "Home");
        }
        if (!joven.TipoUsuario)
        {
            return RedirectToAction("PrincipalAdulto", "Home");
        }

        Tareas? tarea = BD.ObtenerTareaPorId(idTarea);
        if (tarea == null)
        {
            return RedirectToAction("AyudasParaRealizar", "Home");
        }

        Usuarios? solicitante = BD.ObtenerUsuarioPorId(tarea.IdUsuarioSolicitante);
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

        Tareas? tarea = BD.ObtenerTareaPorId(idTarea);
        if (tarea == null)
        {
            return Json(null);
        }

        Usuarios? solicitante = BD.ObtenerUsuarioPorId(tarea.IdUsuarioSolicitante);
        DateTime fechaMostrar = tarea.DiaFecha.Year > 1 ? tarea.DiaFecha : (tarea.DiaTarea.Year > 1 ? tarea.DiaTarea : tarea.FechaCreacion);
        string nombreSolicitante = solicitante != null
            ? ((solicitante.Nombre ?? "") + " " + (solicitante.Apellido ?? "")).Trim()
            : "Adulto mayor";
        if (string.IsNullOrEmpty(nombreSolicitante))
        {
            nombreSolicitante = solicitante?.Email ?? "Adulto mayor";
        }

        string[] diasSemana = new string[] { "Domingo", "Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sábado" };
        string nombreDia = diasSemana[(int)fechaMostrar.DayOfWeek];
        string fechaDiaTexto = nombreDia + " " + fechaMostrar.ToString("dd/MM");

        string aclaracionBruta = tarea.Aclaracion ?? "";
        string aclaracionLimpia = aclaracionBruta;
        int idxParentesis = aclaracionBruta.IndexOf(" (Día y hora: ");
        if (idxParentesis >= 0)
        {
            aclaracionLimpia = aclaracionBruta.Substring(0, idxParentesis).Trim();
        }
        else if (aclaracionBruta.StartsWith("Día y hora: "))
        {
            aclaracionLimpia = "";
        }

        string montoTexto = tarea.Monto > 0 ? tarea.Monto.ToString("0") : "5000";

        return Json(new
        {
            idTarea = tarea.IdTarea,
            titulo = tarea.Titulo,
            descripcion = tarea.Descripcion,
            lugar = tarea.Lugar,
            barrio = tarea.Barrio ?? "",
            monto = montoTexto,
            fecha = fechaMostrar.ToString("dd/MM/yyyy"),
            fechaDia = fechaDiaTexto,
            hora = tarea.HoraTarea.TotalMinutes > 0 ? tarea.HoraTarea.ToString(@"hh\:mm") : fechaMostrar.ToString("HH:mm"),
            aclaracion = aclaracionLimpia,
            nombreSolicitante = nombreSolicitante,
            fotoSolicitante = solicitante?.FotoPerfil ?? "/img/user.svg"
        });
    }

    [HttpPost]
    public IActionResult EliminarTareaRechazada(int idTarea)
    {
        Usuarios? joven = ObtenerUsuarioSesion();
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
        Usuarios? joven = ObtenerUsuarioSesion();
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
        Usuarios? joven = ObtenerUsuarioSesion();
        if (joven == null)
        {
            return RedirectToAction("Login", "Home");
        }
        if (!joven.TipoUsuario)
        {
            return RedirectToAction("PrincipalAdulto", "Home");
        }

        int idJoven = joven.IdUsuario;

        // estado "Pendiente": la postulación espera a que el adulto la acepte o rechace
        Postulaciones nuevaPostulacion = new Postulaciones(idJoven, idTarea, "Pendiente", 0, true);
        BD.GuardarPostulacion(nuevaPostulacion);

        return RedirectToAction("Postulados", "Home", new { idTarea = idTarea });
    }

    [HttpPost]
    public IActionResult PostularseJson(int idTarea)
    {
        Usuarios? joven = ObtenerUsuarioSesion();
        if (joven == null || !joven.TipoUsuario)
        {
            return Json(null);
        }

        int idJoven = joven.IdUsuario;

        // estado "Pendiente": la postulación espera a que el adulto la acepte o rechace
        Postulaciones nuevaPostulacion = new Postulaciones(idJoven, idTarea, "Pendiente", 0, true);
        BD.GuardarPostulacion(nuevaPostulacion);

        Tareas? tarea = BD.ObtenerTareaPorId(idTarea);
        List<Usuarios> postulados = BD.ObtenerPostuladosPorTarea(idTarea);

        return Json(new
        {
            tituloTarea = tarea?.Titulo ?? "esta ayuda",
            postulados = postulados
        });
    }

    public IActionResult Postulados(int idTarea)
    {
        Usuarios? joven = ObtenerUsuarioSesion();
        if (joven == null)
        {
            return RedirectToAction("Login", "Home");
        }
        if (!joven.TipoUsuario)
        {
            return RedirectToAction("PrincipalAdulto", "Home");
        }

        Tareas? tarea = BD.ObtenerTareaPorId(idTarea);
        ViewBag.Tarea = tarea;
        ViewBag.Postulados = BD.ObtenerPostuladosPorTarea(idTarea);
        return View("Postulados");
    }

    [HttpPost]
    public IActionResult AceptarTarea(int idTarea)
    {
        Usuarios? joven = ObtenerUsuarioSesion();
        if (joven == null)
        {
            return RedirectToAction("Login", "Home");
        }
        if (!joven.TipoUsuario)
        {
            return RedirectToAction("PrincipalAdulto", "Home");
        }

        BD.AceptarTarea(idTarea);
        return RedirectToAction("AyudasParaRealizar", "Home", new { mensaje = "¡Aceptaste la ayuda correctamente!" });
    }

    [HttpPost]
    public IActionResult RechazarTarea(int idTarea)
    {
        Usuarios? joven = ObtenerUsuarioSesion();
        if (joven == null || !joven.TipoUsuario)
        {
            return Json(new { ok = false });
        }

        // Se guarda en Postulaciones como "Descartada" (la tarea sigue existiendo para los demás jóvenes)
        bool ok = BD.RechazarTarea(idTarea, joven.IdUsuario);
        return Json(new { ok = ok });
    }

    // ===== AYUDAS PEDIDAS POR EL ADULTO =====

    public IActionResult MisAyudas(string? mensaje = null)
    {
        Usuarios? adulto = ObtenerUsuarioSesion();
        if (adulto == null)
        {
            return RedirectToAction("Login", "Home");
        }
        if (adulto.TipoUsuario)
        {
            return RedirectToAction("PrincipalJoven", "Home");
        }

        int idAdulto = adulto.IdUsuario;
        List<Tareas> misTareas = BD.ObtenerTareasPorSolicitante(idAdulto);

        // Slide 15: Dictionary para saber cuántos postulantes tiene cada tarea y si ya tiene joven asignado
        Dictionary<int, int> cantidadPostulantes = new Dictionary<int, int>();
        Dictionary<int, Usuarios?> jovenesAsignados = new Dictionary<int, Usuarios?>();
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
        ViewBag.SuccessMessage = mensaje;
        ViewBag.MisTareas = misTareas;
        ViewBag.CantidadPostulantes = cantidadPostulantes;
        ViewBag.JovenesAsignados = jovenesAsignados;
        ViewBag.TareasFinalizadas = tareasFinalizadas;
        return View("MisAyudas");
    }

    [HttpPost]
    public IActionResult EliminarTareaFinalizada(int idTarea)
    {
        Usuarios? adulto = ObtenerUsuarioSesion();
        if (adulto == null)
        {
            return RedirectToAction("Login", "Home");
        }
        if (adulto.TipoUsuario)
        {
            return RedirectToAction("PrincipalJoven", "Home");
        }

        BD.EliminarTareaFinalizadaAdulto(idTarea);
        return RedirectToAction("MisAyudas", "Home", new { mensaje = "La tarea finalizada fue eliminada de tu lista." });
    }

    [HttpGet]
    public IActionResult TicketAyuda(int idTarea)
    {
        Usuarios? adulto = ObtenerUsuarioSesion();
        if (adulto == null)
        {
            return RedirectToAction("Login", "Home");
        }
        if (adulto.TipoUsuario)
        {
            return RedirectToAction("PrincipalJoven", "Home");
        }

        Tareas? tarea = BD.ObtenerTareaPorId(idTarea);
        if (tarea == null)
        {
            return RedirectToAction("MisAyudas", "Home");
        }

        Usuarios? jovenAsignado = BD.ObtenerJovenAceptadoPorTarea(idTarea);
        Usuarios? solicitante = BD.ObtenerUsuarioPorId(tarea.IdUsuarioSolicitante) ?? adulto;

        ViewBag.Adulto = adulto;
        ViewBag.Solicitante = solicitante;
        ViewBag.Tarea = tarea;
        ViewBag.JovenAsignado = jovenAsignado;
        return View("TicketAyuda");
    }

    [HttpPost]
    public IActionResult GuardarValoracion(int idTarea, double Calificacion = 5, bool Puntual = true, string? Comentario = "")
    {
        Usuarios? adulto = ObtenerUsuarioSesion();
        if (adulto == null)
        {
            return RedirectToAction("Login", "Home");
        }
        if (adulto.TipoUsuario)
        {
            return RedirectToAction("PrincipalJoven", "Home");
        }

        if (Calificacion < 1) Calificacion = 1;
        if (Calificacion > 5) Calificacion = 5;

        BD.GuardarValoracionJoven(idTarea, Calificacion, Puntual);

        Usuarios? joven = BD.ObtenerJovenAceptadoPorTarea(idTarea);
        string nombreJoven = joven != null ? ((joven.Nombre ?? "") + " " + (joven.Apellido ?? "")).Trim() : "el joven";
        if (string.IsNullOrEmpty(nombreJoven))
        {
            nombreJoven = joven?.Email ?? "el joven";
        }

        return RedirectToAction("MisAyudas", "Home", new { mensaje = "¡Gracias por tu devolución! Se guardó la calificación para " + nombreJoven + "." });
    }

    [HttpPost]
    public IActionResult CancelarTarea(int idTarea)
    {
        Usuarios? adulto = ObtenerUsuarioSesion();
        if (adulto == null)
        {
            return RedirectToAction("Login", "Home");
        }
        if (adulto.TipoUsuario)
        {
            return RedirectToAction("PrincipalJoven", "Home");
        }

        Tareas? tarea = BD.ObtenerTareaPorId(idTarea);

        if (tarea == null)
        {
            return RedirectToAction("MisAyudas", "Home", new { mensaje = "No se encontró la ayuda." });
        }

        bool cancelada = BD.CancelarTarea(idTarea);
        string mensaje = cancelada
            ? "La ayuda fue cancelada."
            : "No se puede cancelar una ayuda que ya tiene postulantes.";
        return RedirectToAction("MisAyudas", "Home", new { mensaje = mensaje });
    }

    [HttpGet]
    public IActionResult ModificarTarea(int idTarea)
    {
        Usuarios? adulto = ObtenerUsuarioSesion();
        if (adulto == null)
        {
            return RedirectToAction("Login", "Home");
        }
        if (adulto.TipoUsuario)
        {
            return RedirectToAction("PrincipalJoven", "Home");
        }

        Tareas? tarea = BD.ObtenerTareaPorId(idTarea);
        if (tarea == null)
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
    public IActionResult ModificarTarea(int IdTarea, string? Titulo = "", string? Descripcion = "", string? Lugar = "", string? Barrio = "", string? TipoLugar = "Casa", string? DiaHora = "", string? Aclaracion = "", string? ConfirmacionModificar = "")
    {
        Usuarios? adulto = ObtenerUsuarioSesion();
        if (adulto == null)
        {
            return RedirectToAction("Login", "Home");
        }
        if (adulto.TipoUsuario)
        {
            return RedirectToAction("PrincipalJoven", "Home");
        }

        Tareas? actual = BD.ObtenerTareaPorId(IdTarea);
        if (actual == null)
        {
            return RedirectToAction("MisAyudas", "Home", new { mensaje = "No se encontró la ayuda." });
        }

        if (!actual.Estado || BD.ContarPostulantes(IdTarea) > 0 || BD.ObtenerJovenAceptadoPorTarea(IdTarea) != null)
        {
            return RedirectToAction("MisAyudas", "Home", new { mensaje = "No se puede modificar una ayuda que ya tiene postulantes o aceptados." });
        }

        string tituloLimpio = (Titulo ?? "").Trim();
        string descripcionLimpia = (Descripcion ?? "").Trim();
        string lugarLimpio = (Lugar ?? "").Trim();
        string barrioLimpio = (Barrio ?? "").Trim();
        string tipoLugarLimpio = string.IsNullOrWhiteSpace(TipoLugar) ? "Casa" : TipoLugar.Trim();
        string diaHoraLimpio = (DiaHora ?? "").Trim();
        string aclaracionLimpia = (Aclaracion ?? "").Trim();
        string confirmacionLimpia = (ConfirmacionModificar ?? "").Trim().ToLower();

        // Actualizar objeto en memoria por si hay error de validación y se vuelve a mostrar el formulario
        actual.Titulo = tituloLimpio;
        actual.Descripcion = descripcionLimpia;
        actual.Lugar = lugarLimpio;
        actual.Barrio = barrioLimpio;
        actual.TipoLugar = tipoLugarLimpio;
        actual.Aclaracion = aclaracionLimpia;
        ViewBag.DiaHoraTexto = diaHoraLimpio;
        ViewBag.Tarea = actual;

        if (string.IsNullOrEmpty(tituloLimpio) || string.IsNullOrEmpty(descripcionLimpia) || string.IsNullOrEmpty(lugarLimpio) || string.IsNullOrEmpty(diaHoraLimpio))
        {
            ViewBag.ErrorMessage = "Por favor completá el título, la descripción, el lugar y el día/hora.";
            return View("ModificarTarea");
        }

        if (confirmacionLimpia != "modificar")
        {
            ViewBag.ErrorMessage = "Debés escribir la palabra \"modificar\" abajo de todo para confirmar los cambios.";
            return View("ModificarTarea");
        }

        if (tituloLimpio.Length > 50) tituloLimpio = tituloLimpio.Substring(0, 50);
        if (descripcionLimpia.Length > 500) descripcionLimpia = descripcionLimpia.Substring(0, 500);
        if (lugarLimpio.Length > 50) lugarLimpio = lugarLimpio.Substring(0, 50);
        if (barrioLimpio.Length > 50) barrioLimpio = barrioLimpio.Substring(0, 50);
        if (tipoLugarLimpio.Length > 50) tipoLugarLimpio = tipoLugarLimpio.Substring(0, 50);

        DateTime fechaTarea = actual.DiaFecha.Year > 1 ? actual.DiaFecha.Date : (actual.DiaTarea.Year > 1 ? actual.DiaTarea.Date : DateTime.Now.Date);
        TimeSpan horaTarea = actual.HoraTarea.TotalMinutes > 0 ? actual.HoraTarea : (actual.DiaFecha.Year > 1 ? actual.DiaFecha.TimeOfDay : new TimeSpan(17, 30, 0));

        string[] formatos = new string[] { "dd/MM/yyyy HH:mm", "d/M/yyyy HH:mm", "dd/MM/yyyy H:mm", "d/M/yyyy H:mm", "dd/MM/yy HH:mm", "d/M/yy HH:mm", "yyyy-MM-ddTHH:mm" };
        if (DateTime.TryParseExact(diaHoraLimpio, formatos, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime fechaExacta))
        {
            fechaTarea = fechaExacta.Date;
            horaTarea = fechaExacta.TimeOfDay;
        }
        else if (DateTime.TryParse(diaHoraLimpio, new System.Globalization.CultureInfo("es-AR"), System.Globalization.DateTimeStyles.None, out DateTime fechaParseada))
        {
            fechaTarea = fechaParseada.Date;
            horaTarea = fechaParseada.TimeOfDay;
        }

        string textoFechaHora = fechaTarea.ToString("dd/MM/yyyy") + " " + horaTarea.ToString(@"hh\:mm");
        string aclaracionFinal = string.IsNullOrEmpty(aclaracionLimpia)
            ? "Día y hora: " + textoFechaHora
            : aclaracionLimpia + " (Día y hora: " + textoFechaHora + ")";
        if (aclaracionFinal.Length > 500)
        {
            aclaracionFinal = aclaracionFinal.Substring(0, 500);
        }

        actual.Titulo = tituloLimpio;
        actual.Descripcion = descripcionLimpia;
        actual.Lugar = lugarLimpio;
        actual.Barrio = barrioLimpio;
        actual.TipoLugar = tipoLugarLimpio;
        actual.DiaTarea = fechaTarea;
        actual.HoraTarea = horaTarea;
        actual.Aclaracion = aclaracionFinal;

        bool modificado = BD.ModificarTarea(actual);
        if (!modificado)
        {
            ViewBag.ErrorMessage = "No se pudo modificar la ayuda en la base de datos.";
            return View("ModificarTarea");
        }

        return RedirectToAction("MisAyudas", "Home", new { mensaje = "¡La ayuda (" + tituloLimpio + ") fue modificada correctamente!" });
    }

    public IActionResult PostulantesTarea(int idTarea)
    {
        Usuarios? adulto = ObtenerUsuarioSesion();
        if (adulto == null)
        {
            return RedirectToAction("Login", "Home");
        }
        if (adulto.TipoUsuario)
        {
            return RedirectToAction("PrincipalJoven", "Home");
        }

        Tareas? tarea = BD.ObtenerTareaPorId(idTarea);
        if (tarea == null)
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

        Usuarios? joven = BD.ObtenerUsuarioPorId(idUsuario);
        if (joven == null)
        {
            return Json(null);
        }

        string nombre = ((joven.Nombre ?? "") + " " + (joven.Apellido ?? "")).Trim();
        if (string.IsNullOrEmpty(nombre))
        {
            nombre = joven.Email;
        }

        return Json(new
        {
            idUsuario = joven.IdUsuario,
            nombre = nombre,
            foto = string.IsNullOrEmpty(joven.FotoPerfil) ? "/img/user.svg" : joven.FotoPerfil,
            ayudasRealizadas = BD.ContarAyudasRealizadas(idUsuario),
            valoracion = BD.ObtenerValoracionPromedio(idUsuario),
            puntual = BD.EsPuntual(idUsuario)
        });
    }

    [HttpPost]
    public IActionResult AceptarPostulante(int idTarea, int idUsuario)
    {
        Usuarios? adulto = ObtenerUsuarioSesion();
        if (adulto == null || adulto.TipoUsuario)
        {
            return Json(new { ok = false });
        }

        bool ok = BD.AceptarPostulante(idTarea, idUsuario);
        return Json(new { ok = ok });
    }

    [HttpPost]
    public IActionResult RechazarPostulante(int idTarea, int idUsuario)
    {
        Usuarios? adulto = ObtenerUsuarioSesion();
        if (adulto == null || adulto.TipoUsuario)
        {
            return Json(new { ok = false });
        }

        bool ok = BD.RechazarPostulante(idTarea, idUsuario);
        return Json(new { ok = ok });
    }

    private Usuarios? ObtenerUsuarioSesion()
    {
        Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
        Response.Headers["Pragma"] = "no-cache";
        Response.Headers["Expires"] = "0";

        int? idSesion = HttpContext.Session.GetInt32("IdUsuarioLogueado");
        if (idSesion != null)
        {
            Usuarios? u = BD.ObtenerUsuarioPorId(idSesion.Value);
            if (u != null)
            {
                ViewBag.UsuarioSesion = u;
                ViewBag.FotoPerfilSesion = string.IsNullOrWhiteSpace(u.FotoPerfil) ? "/img/user.svg" : u.FotoPerfil;
                return u;
            }
        }

        string? emailSesion = HttpContext.Session.GetString("EmailLogueado");
        if (!string.IsNullOrEmpty(emailSesion))
        {
            Usuarios? u = BD.ObtenerUsuarioPorEmail(emailSesion);
            if (u != null)
            {
                ViewBag.UsuarioSesion = u;
                ViewBag.FotoPerfilSesion = string.IsNullOrWhiteSpace(u.FotoPerfil) ? "/img/user.svg" : u.FotoPerfil;
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

    // Endpoint para peticiones server-side FETCH (Slides 142-146 del PDF)
    [HttpGet]
    public IActionResult VerDetalleAyudado(int idUsuario)
    {
        if (ObtenerUsuarioSesion() == null)
        {
            return Json(null);
        }

        Usuarios? usuario = BD.ObtenerUsuarioPorId(idUsuario);
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

    private string? GuardarArchivo(IFormFile? archivo, string subcarpeta)
    {
        if (archivo == null || archivo.Length == 0)
        {
            return null;
        }

        string folderPath = Path.Combine(_environment.WebRootPath, subcarpeta);
        Directory.CreateDirectory(folderPath);

        string ext = Path.GetExtension(archivo.FileName);
        if (string.IsNullOrEmpty(ext))
        {
            ext = ".jpg";
        }
        string baseName = Path.GetFileNameWithoutExtension(archivo.FileName).Replace(" ", "_");
        if (baseName.Length > 12)
        {
            baseName = baseName.Substring(0, 12);
        }
        string nombreArchivo = baseName + "_" + (DateTime.Now.Ticks % 1000000).ToString() + ext;
        string fullPath = Path.Combine(folderPath, nombreArchivo);

        using (FileStream stream = System.IO.File.Create(fullPath))
        {
            archivo.CopyTo(stream);
        }

        return "/" + subcarpeta + "/" + nombreArchivo;
    }
}
