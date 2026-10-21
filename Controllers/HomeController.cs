using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TeAyudo.Models;

namespace TeAyudo.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IWebHostEnvironment _environment;

    public HomeController(ILogger<HomeController> logger, IWebHostEnvironment environment)
    {
        _logger = logger;
        _environment = environment;
    }
    
    public IActionResult Index()
    {
        var modelo = new HomeViewModel
        {
            Titulo = "Donde encontrás lo que buscás",
            Presentacion = new[] {
                "Jóvenes brindan ayuda que los adultos requieren.",
                "Adultos reciben asistencia a cambio de un monto accesible."
            },
            SobreNosotros = new[] {
                "En Te Ayudo creemos que ayudarnos entre generaciones puede cambiar la forma en la que vivimos.",
                "Creamos un espacio que conecta a jóvenes universitarios con adultos para que puedan ayudarse en tareas cotidianas.",
                "Los jóvenes tienen la posibilidad de ganar experiencia y aprender, mientras que los adultos encuentran una forma sencilla y segura de recibir la ayuda que necesitan.",
                "Buscamos acompañar a quien lo necesita y a quien puede ayudar, con el cuidado que las personas de nuestra comunidad merecen."
            },
            AyudaTitulo = "Recibí la ayuda que buscás",
            AyudaTexto = "Seleccionamos a quienes pueden acompañarte.",
            TrabajoTitulo = "¿Sos joven y querés ganar experiencia laboral?",
            TrabajoTexto = "Formá parte de las historias de Te Ayudo.",
            Pasos = new[] {
                new ContenidoItem { Titulo = "Publicá", Texto = "Publicás la ayuda que precisás y elegís un momento.", Icono = "pen.png" },
                new ContenidoItem { Titulo = "Encontrá", Texto = "Buscás a alguien que pueda ayudarte.", Icono = "alarm.png" },
                new ContenidoItem { Titulo = "Conectate", Texto = "Coordinás los detalles entre ambas partes.", Icono = "circle-check.png" }
            },
            Valores = new[] {
                new ContenidoItem { Titulo = "Seguridad", Texto = "Tu tranquilidad es nuestra prioridad. Promovemos el respeto y el cuidado entre las personas de nuestra comunidad.", Icono = "circle-check.png" },
                new ContenidoItem { Titulo = "Confianza", Texto = "Creemos en la confianza entre las personas. Cada encuentro es una oportunidad para conocerse y construir una buena relación.", Icono = "pen.png" },
                new ContenidoItem { Titulo = "Comunicación", Texto = "Todo es más fácil cuando nos entendemos. Conversá para coordinar la ayuda, acordar los detalles y resolver cualquier duda.", Icono = "alarm.png" }
            },
            Preguntas = new[] {
                new ContenidoItem { Titulo = "¿Qué es Te Ayudo?", Texto = "Un espacio que conecta a jóvenes con adultos que necesitan ayuda con tareas cotidianas." },
                new ContenidoItem { Titulo = "¿Cómo solicito ayuda?", Texto = "Desde Solicitar ayuda podés acceder a la sección de registro para adultos mayores. El formulario todavía no está habilitado." },
                new ContenidoItem { Titulo = "¿Cómo puedo sumarme?", Texto = "Desde Trabajá con nosotros podés acceder a la sección de registro para jóvenes. El formulario todavía no está habilitado." }
            }
        };
        return View(modelo);
    }

   

    public IActionResult Ingresar()
    {
        return RedirectToAction(nameof(Login));
    }

    [HttpGet]
    public IActionResult Login()
    {
        ViewBag.SuccessMessage = TempData["SuccessMessage"] as string;
        return View();
    }

    [HttpPost]
    public IActionResult Login(string Email, string Contrasena)
    {
        if (string.IsNullOrWhiteSpace(Email))
        {
            ViewBag.ErrorMessage = "Debe completar el email.";
            return View("Login");
        }

        if (string.IsNullOrWhiteSpace(Contrasena))
        {
            ViewBag.ErrorMessage = "Debe completar la contraseña.";
            return View("Login");
        }

        BD bd = new BD();
        Usuarios? usuario;

        try
        {
            usuario = bd.ObtenerUsuarioPorCredenciales(Email, Contrasena);
        }
        catch
        {
            ViewBag.ErrorMessage = "No se pudo acceder a la base de datos.";
            return View("Login");
        }

        if (usuario == null)
        {
            ViewBag.ErrorMessage = "Email o contraseña incorrectos.";
            return View("Login");
        }

        HttpContext.Session.SetString("EmailLogueado", usuario.Email ?? string.Empty);
        return RedirectToAction("Perfil");
    }

    public IActionResult SolicitarTarea()
    {
        return View();
    }

    public IActionResult Trabajar()
    {
        return View();
    }

    public IActionResult RegistroJoven()
    {
        ViewData["Title"] = "Registro para jóvenes";
        ViewData["Descripcion"] = "Este será el acceso al registro para jóvenes que quieran trabajar en Te Ayudo.";
        return View("Trabajar");
    }

    public IActionResult RegistroMayor()
    {
        ViewData["Title"] = "Registro para solicitar ayuda";
        ViewData["Descripcion"] = "Este será el acceso al registro para adultos mayores que necesiten ayuda.";
        return View("Registro");
    }

    [HttpPost]
[ValidateAntiForgeryToken]
public IActionResult GuardarRegistroMayor(Usuarios user, IFormFile? FotoPerfil)
{
    if (user == null)
    {
        ViewBag.ErrorMessage = "Debe completar los datos del registro.";
        return View("Registro");
    }

    ModelState.Remove("IdUsuario");
    ModelState.Remove("TipoUsuario");
    ModelState.Remove("FechaRegistro");
    ModelState.Remove("FotoPerfilRuta");
    ModelState.Remove("AceptaTerminos"); 

    if (!ModelState.IsValid)
    {
        ViewBag.ErrorMessage = "Por favor complete los campos requeridos correctamente.";
        return View("Registro");
    }

    user.TipoUsuario = "Adulto mayor";
    user.FechaRegistro = DateTime.Now;

    try
    {
        user.FotoPerfilRuta = GuardarArchivo(FotoPerfil, "uploads/perfiles");
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error al guardar la foto de perfil para {Email}", user.Email);
        ViewBag.ErrorMessage = "No se pudo guardar la foto de perfil.";
        return View("Registro");
    }

    BD bd = new BD();
    bool resultado = false;

    try
    {
        resultado = bd.GuardarUsuario(user);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error al guardar registro en BD para {Email}", user.Email);
        ViewBag.ErrorMessage = "Error interno al guardar en la base de datos.";
        return View("Registro");
    }

    if (resultado)
    {
        TempData["SuccessMessage"] = "Registro exitoso. Ahora puede iniciar sesión.";
        return RedirectToAction(nameof(Login)); // Retorno de éxito
    }

    ViewBag.ErrorMessage = "El email ya existe o no se pudo guardar el registro.";
    return View("Registro"); // Retorno final por defecto si falla el guardado
}


    [HttpPost]
    public IActionResult GuardarRegistroJoven(Usuarios user, IFormFile? FotoPerfil, IFormFile? CvArchivo, bool AceptaTerminos = false)
    {
        if (user == null)
        {
            ViewBag.ErrorMessage = "Debe completar los datos del registro.";
            return View("Trabajar");
        }

        if (!AceptaTerminos)
        {
            ViewBag.ErrorMessage = "Debe aceptar los términos y condiciones.";
            return View("Trabajar");
        }

        user.TipoUsuario = "Adolecente";
        user.AceptaTerminos = AceptaTerminos;
        user.FotoPerfilRuta = GuardarArchivo(FotoPerfil, "uploads/perfiles");
        user.CvRuta = GuardarArchivo(CvArchivo, "uploads/cv");
        user.FechaRegistro = DateTime.Now;

        BD bd = new BD();
        bool resultado;

        try
        {
            resultado = bd.GuardarUsuario(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al guardar registro de joven para {Email}", user.Email);
            ViewBag.ErrorMessage = "No se pudo guardar el registro en la base de datos.";
            return View("Trabajar");
        }

        if (resultado)
        {
            TempData["SuccessMessage"] = "Registro exitoso. Ahora puede iniciar sesión.";
            return RedirectToAction(nameof(Login));
        }

        bool emailExiste = !string.IsNullOrWhiteSpace(user.Email) && bd.ObtenerUsuarioPorEmail(user.Email) != null;
        ViewBag.ErrorMessage = emailExiste
            ? "El email ya existe. Por favor, elige otro."
            : "No se pudo guardar el registro en la base de datos.";
        return View("Trabajar");
    }

    public IActionResult OlvideContrasena()
    {
        return View("olvideContraseña");
    }

    [HttpPost]
    public IActionResult OlvideContrasena(string Email)
    {
        if (string.IsNullOrWhiteSpace(Email))
        {
            ViewBag.ErrorMessage = "Debe ingresar un email.";
            return View("olvideContraseña");
        }

        BD bd = new BD();
        Usuarios? usuario = bd.ObtenerUsuarioPorEmail(Email);

        if (usuario == null)
        {
            ViewBag.ErrorMessage = "No existe un usuario con ese email.";
            return View("olvideContraseña");
        }

        ViewBag.Contrasena = usuario.Contrasena ?? "La recuperación de contraseña aún no está implementada.";
        return View("MostrarContraseña");
    }

    public IActionResult Perfil()
    {
        string? emailSesion = HttpContext.Session.GetString("EmailLogueado");
        if (string.IsNullOrEmpty(emailSesion))
        {
            return RedirectToAction(nameof(Login));
        }

        BD bd = new BD();
        Usuarios? usuario;

        try
        {
            usuario = bd.ObtenerUsuarioPorEmail(emailSesion);
        }
        catch
        {
            HttpContext.Session.Remove("EmailLogueado");
            HttpContext.Session.Clear();
            ViewBag.ErrorMessage = "No se pudo acceder a la base de datos.";
            return View("Login");
        }
        if (usuario == null)
        {
            HttpContext.Session.Remove("EmailLogueado");
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
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
        HttpContext.Session.Clear();
        return RedirectToAction("Index");
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
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

        string webRootPath = _environment.WebRootPath;
        string folderPath = Path.Combine(webRootPath, subcarpeta.Replace('/', Path.DirectorySeparatorChar));
        try
        {
            Directory.CreateDirectory(folderPath);

            string extension = Path.GetExtension(archivo.FileName);
            string fileName = $"{Guid.NewGuid():N}{extension}";
            string fullPath = Path.Combine(folderPath, fileName);

            using FileStream stream = System.IO.File.Create(fullPath);
            archivo.CopyTo(stream);

            return $"/{subcarpeta}/{fileName}".Replace("\\", "/");
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Error al guardar archivo en {Folder}", folderPath);
            return null;
        }
    }

    public IActionResult Explorar()
    {
        int idJovenLogueado = 1; // ID de ejemplo o tomado de Session

        ViewBag.Joven = BD.ObtenerHistorialJoven(idJovenLogueado);
        ViewBag.UltimosAyudados = BD.ObtenerUltimosAyudados(idJovenLogueado);

        return View();
    }
}
