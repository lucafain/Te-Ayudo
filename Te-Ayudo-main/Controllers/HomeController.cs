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
        ViewBag.Titulo = "Donde encontrás lo que buscás";

        List<string> listaPresentacion = new List<string>();
        listaPresentacion.Add("Jóvenes brindan ayuda que los adultos requieren.");
        listaPresentacion.Add("Adultos reciben asistencia a cambio de un monto accesible.");
        ViewBag.Presentacion = listaPresentacion;

        List<string> listaSobreNosotros = new List<string>();
        listaSobreNosotros.Add("En Te Ayudo creemos que ayudarnos entre generaciones puede cambiar la forma en la que vivimos.");
        listaSobreNosotros.Add("Creamos un espacio que conecta a jóvenes universitarios con adultos para que puedan ayudarse en tareas cotidianas.");
        listaSobreNosotros.Add("Los jóvenes tienen la posibilidad de ganar experiencia y aprender, mientras que los adultos encuentran una forma sencilla y segura de recibir la ayuda que necesitan.");
        listaSobreNosotros.Add("Buscamos acompañar a quien lo necesita y a quien puede ayudar, con el cuidado que las personas de nuestra comunidad merecen.");
        ViewBag.SobreNosotros = listaSobreNosotros;

        ViewBag.AyudaTitulo = "Recibí la ayuda que buscás";
        ViewBag.AyudaTexto = "Seleccionamos a quienes pueden acompañarte.";
        ViewBag.TrabajoTitulo = "¿Sos joven y querés ganar experiencia laboral?";
        ViewBag.TrabajoTexto = "Formá parte de las historias de Te Ayudo.";

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
        listaPreguntas.Add(new ContenidoItem("¿Cómo puedo sumarme?", "Desde Trabajá con nosotros podés acceder al registro para jóvenes y subir tu CV."));
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
    public IActionResult Login(string Email, string Contrasena)
    {
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

        HttpContext.Session.SetString("EmailLogueado", usuario.Email ?? "");
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
    public IActionResult SolicitarTarea(string Titulo, string Descripcion, string Zona, string Contacto)
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
    public IActionResult GuardarRegistroMayor(string Email, string NombreUsuario, string Contrasena, string Telefono, IFormFile? FotoPerfil)
    {
        // Validación con métodos string (Slide 12)
        if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(NombreUsuario) || string.IsNullOrEmpty(Contrasena))
        {
            ViewBag.ErrorMessage = "Por favor complete todos los campos obligatorios.";
            return View("Registro");
        }

        if (Email.IndexOf("@") == -1)
        {
            ViewBag.ErrorMessage = "El correo electrónico no es válido.";
            return View("Registro");
        }

        // Slide 46: Tomar los datos del form y guardarlos en un objeto
        Usuarios user = new Usuarios();
        user.Email = Email.Replace(" ", "").ToLower();
        user.NombreUsuario = NombreUsuario;
        user.Contrasena = Contrasena;
        user.Telefono = Telefono;
        user.TipoUsuario = "Adulto mayor";
        user.FechaRegistro = DateTime.Now;
        user.FotoPerfilRuta = GuardarArchivo(FotoPerfil, "uploads/perfiles");

        bool resultado = BD.GuardarUsuario(user);

        if (resultado)
        {
            // Slide 33 y 50: RedirectToAction con paso de parámetros anónimos
            return RedirectToAction("Login", "Home", new { mensaje = "Registro exitoso. Ahora puede iniciar sesión." });
        }

        ViewBag.ErrorMessage = "El email ya existe o no se pudo guardar el registro.";
        return View("Registro");
    }

    [HttpPost]
    public IActionResult GuardarRegistroJoven(string Email, string Nombre, string Apellido, string AmbitoLaboral, string Contrasena, string LinkedIn, string Telefono, IFormFile? FotoPerfil, IFormFile? CvArchivo, bool AceptaTerminos = false)
    {
        // Validación en controller
        if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Contrasena) || string.IsNullOrEmpty(AmbitoLaboral))
        {
            ViewBag.ErrorMessage = "Debe completar los datos obligatorios del registro.";
            return View("Trabajar");
        }

        if (!AceptaTerminos)
        {
            ViewBag.ErrorMessage = "Debe aceptar los términos y condiciones.";
            return View("Trabajar");
        }

        // Slide 46: Guardar en objeto
        Usuarios user = new Usuarios();
        user.Email = Email.Replace(" ", "").ToLower();
        user.Nombre = Nombre;
        user.Apellido = Apellido;
        user.AmbitoLaboral = AmbitoLaboral;
        user.Contrasena = Contrasena;
        user.LinkedIn = LinkedIn;
        user.Telefono = Telefono;

        // Slide 12: Métodos para tipos string (IndexOf, Substring)
        int posArroba = user.Email.IndexOf("@");
        if (posArroba > 0)
        {
            user.NombreUsuario = user.Email.Substring(0, posArroba);
        }
        else
        {
            user.NombreUsuario = user.Email;
        }

        user.TipoUsuario = "Adolescente";
        user.AceptaTerminos = AceptaTerminos;
        user.FotoPerfilRuta = GuardarArchivo(FotoPerfil, "uploads/perfiles");
        user.CvRuta = GuardarArchivo(CvArchivo, "uploads/cv");
        user.FechaRegistro = DateTime.Now;

        bool resultado = BD.GuardarUsuario(user);

        if (resultado)
        {
            // Slide 33 y 50: RedirectToAction pasando parámetro
            return RedirectToAction("Login", "Home", new { mensaje = "Registro exitoso. Ahora puede iniciar sesión." });
        }

        ViewBag.ErrorMessage = "El email ya existe. Por favor, elegí otro.";
        return View("Trabajar");
    }

    public IActionResult OlvideContrasena()
    {
        return View("olvideContraseña");
    }

    [HttpPost]
    public IActionResult OlvideContrasena(string Email)
    {
        if (string.IsNullOrEmpty(Email))
        {
            ViewBag.ErrorMessage = "Debe ingresar un email.";
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

        ViewBag.NombreUsuario = usuario.NombreUsuario ?? usuario.Email;
        ViewBag.Email = usuario.Email;
        ViewBag.Nombre = usuario.Nombre;
        ViewBag.Apellido = usuario.Apellido;
        ViewBag.TipoUsuario = usuario.TipoUsuario;
        ViewBag.FotoPerfilRuta = usuario.FotoPerfilRuta;
        ViewBag.Telefono = usuario.Telefono;
        ViewBag.AmbitoLaboral = usuario.AmbitoLaboral;
        ViewBag.LinkedIn = usuario.LinkedIn;

        return View("perfil");
    }

    public IActionResult CerrarSesion()
    {
        HttpContext.Session.Remove("EmailLogueado");
        HttpContext.Session.Remove("IdUsuarioLogueado");
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Home");
    }

    public IActionResult Explorar()
    {
        int idJovenLogueado = HttpContext.Session.GetInt32("IdUsuarioLogueado") ?? 1;

        ViewBag.Joven = BD.ObtenerHistorialJoven(idJovenLogueado);
        ViewBag.UltimosAyudados = BD.ObtenerUltimosAyudados(idJovenLogueado);

        return View();
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
