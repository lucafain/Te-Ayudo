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
    }

    public IActionResult Index()
    {
        // Slide 13, 14 y 15: Colecciones (Dictionary con ContainsKey, Arrays y Listas)
        Dictionary<string, string> textosHome = new Dictionary<string, string>();
        textosHome.Add("Titulo", "Donde encontrás lo que buscás");
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
        arrayPresentacion[0] = "Jóvenes brindan ayuda que los adultos requieren.";
        arrayPresentacion[1] = "Adultos reciben asistencia a cambio de un monto accesible.";

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
        string? email = HttpContext.Session.GetString("EmailLogueado");
        if (!string.IsNullOrEmpty(email))
        {
            ViewBag.UsuarioActual = BD.ObtenerUsuarioPorEmail(email);
        }
        return View();
    }

    [HttpPost]
    public IActionResult SolicitarTarea(string? Titulo = "", string? Descripcion = "", string? Zona = "", string? Contacto = "")
    {
        if (string.IsNullOrEmpty(Titulo) || string.IsNullOrEmpty(Descripcion))
        {
            ViewBag.ErrorMessage = "Por favor complete el título y la descripción de la ayuda que necesita.";
            return View();
        }

        ViewBag.SuccessMessage = "¡Tu solicitud de ayuda (" + Titulo + ") ha sido recibida! Pronto nos pondremos en contacto.";
        return View();
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
    public IActionResult GuardarRegistroMayor(string? Nombre = "", string? Apellido = "", string? Email = "", string? Provincia = "", string? Calle = "", int Direccion = 0, string? CodigoPostal = "", string? Contrasena = "", IFormFile? FotoPerfil = null, bool AceptaTerminos = true)
    {
        // Validación con métodos string (Slide 12)
        if (string.IsNullOrEmpty(Nombre) || string.IsNullOrEmpty(Apellido) || string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Contrasena))
        {
            ViewBag.ErrorMessage = "Por favor complete los campos obligatorios.";
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

        // Slide 46: Tomar los datos del form y guardarlos en un objeto
        // tipoUsuario = false (0) es Adulto
        Usuarios user = new Usuarios();
        user.Nombre = Nombre;
        user.Apellido = Apellido;
        user.Email = Email.Replace(" ", "").ToLower();
        user.Provincia = Provincia ?? "";
        user.Calle = Calle ?? "";
        user.Direccion = Direccion;
        user.CodigoPostal = CodigoPostal ?? "";
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

    public IActionResult Perfil()
    {
        string? emailSesion = HttpContext.Session.GetString("EmailLogueado");
        if (string.IsNullOrEmpty(emailSesion))
        {
            return RedirectToAction("Login", "Home");
        }

        Usuarios? usuario = BD.ObtenerUsuarioPorEmail(emailSesion);
        if (usuario == null)
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Home");
        }

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

    public IActionResult PrincipalJoven()
    {
        int idJovenLogueado = HttpContext.Session.GetInt32("IdUsuarioLogueado") ?? 1;

        ViewBag.Joven = BD.ObtenerHistorialJoven(idJovenLogueado);
        ViewBag.UltimosAyudados = BD.ObtenerUltimosAyudados(idJovenLogueado);

        return View("PrincipalJoven");
    }

    public IActionResult PrincipalAdulto()
    {
        int idAdultoLogueado = HttpContext.Session.GetInt32("IdUsuarioLogueado") ?? 1;

        ViewBag.Adulto = BD.ObtenerUsuarioPorId(idAdultoLogueado);
        ViewBag.Jovenes = BD.ObtenerJovenesDisponibles();

        return View("PrincipalAdulto");
    }

    public IActionResult Explorar()
    {
        return RedirectToAction("PrincipalJoven", "Home");
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

        string nombreArchivo = archivo.FileName.Replace(" ", "_");
        string fullPath = Path.Combine(folderPath, nombreArchivo);

        using (FileStream stream = System.IO.File.Create(fullPath))
        {
            archivo.CopyTo(stream);
        }

        return "/" + subcarpeta + "/" + nombreArchivo;
    }
}
