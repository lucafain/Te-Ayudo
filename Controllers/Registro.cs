using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TeAyudo.Models;

namespace TeAyudo.Controllers;

public class RegistroController : Controller
{
    private readonly ILogger<RegistroController> _logger;
    private readonly IWebHostEnvironment _environment;

    public RegistroController(ILogger<RegistroController> logger, IWebHostEnvironment environment)
    {
        _logger = logger;
        _environment = environment;
    }

    public IActionResult Index()
    {
        return RedirectToAction("Index", "Home");
    }

    public IActionResult Login()
    {
        ViewBag.SuccessMessage = TempData["SuccessMessage"] as string;

        string? emailSesion = HttpContext.Session.GetString("EmailLogueado");
        if (!string.IsNullOrEmpty(emailSesion))
        {
            return RedirectToAction("Perfil");
        }

        return View("~/Views/Home/Login.cshtml");
    }

    public IActionResult OlvideContrasena()
    {
        return View("~/Views/Home/olvideContraseña.cshtml");
    }

    [HttpPost]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult OlvideContrasena(string Email)
    {
        if (string.IsNullOrWhiteSpace(Email))
        {
            ViewBag.ErrorMessage = "Debe ingresar un email.";
            return View("~/Views/Home/olvideContraseña.cshtml");
        }

        BD bd = new BD();
        Usuarios? usuario = bd.ObtenerUsuarioPorEmail(Email);

        if (usuario == null)
        {
            ViewBag.ErrorMessage = "No existe un usuario con ese email.";
            return View("~/Views/Home/olvideContraseña.cshtml");
        }

        ViewBag.Contrasena = usuario.Contrasena ?? "La recuperación de contraseña aún no está implementada.";
        return View("~/Views/Home/MostrarContraseña.cshtml");
    }

    [HttpPost]
    public IActionResult Login(string Email, string Contrasena)
    {
        if (string.IsNullOrWhiteSpace(Email))
        {
            ViewBag.ErrorMessage = "Debe completar el email.";
            return View("~/Views/Home/Login.cshtml");
        }

        if (string.IsNullOrWhiteSpace(Contrasena))
        {
            ViewBag.ErrorMessage = "Debe completar la contraseña.";
            return View("~/Views/Home/Login.cshtml");
        }

        BD bd = new BD();
        Usuarios? usuario = bd.ObtenerUsuarioPorCredenciales(Email, Contrasena);

        if (usuario == null)
        {
            ViewBag.ErrorMessage = "Email o contraseña incorrectos.";
            return View("~/Views/Home/Login.cshtml");
        }

        HttpContext.Session.SetString("EmailLogueado", usuario.Email ?? string.Empty);
        return RedirectToAction("Perfil");
    }

    public IActionResult Registro()
    {
        return View("~/Views/Home/Registro.cshtml");
    }

    [HttpPost]
    public IActionResult GuardarRegistro(Usuarios USER, IFormFile? FotoPerfil)
    {
        if (USER == null)
        {
            ViewBag.ErrorMessage = "Debe completar los datos del registro.";
            return View("~/Views/Home/Registro.cshtml");
        }

        USER.TipoUsuario ??= "Adulto mayor";
        USER.FotoPerfilRuta = GuardarArchivo(FotoPerfil, "uploads/perfiles");
        USER.FechaRegistro = DateTime.Now;

        BD bd = new BD();
        bool resultado;

        try
        {
            resultado = bd.GuardarUsuario(USER);
        }
        catch
        {
            ViewBag.ErrorMessage = "No se pudo guardar el registro en la base de datos.";
            return View("~/Views/Home/Registro.cshtml");
        }

        if (resultado)
        {
            TempData["SuccessMessage"] = "Registro exitoso. Ahora puede iniciar sesión.";
            return RedirectToAction("Login");
        }

        bool emailExiste = !string.IsNullOrWhiteSpace(USER.Email) && bd.ObtenerUsuarioPorEmail(USER.Email) != null;
        ViewBag.ErrorMessage = emailExiste
            ? "El email ya existe. Por favor, elige otro."
            : "No se pudo guardar el registro en la base de datos.";
        return View("~/Views/Home/Registro.cshtml");
    }

    public IActionResult Perfil()
    {
        string? emailSesion = HttpContext.Session.GetString("EmailLogueado");
        if (string.IsNullOrEmpty(emailSesion))
        {
            return RedirectToAction("Login");
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
            return RedirectToAction("Login");
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

        return View("~/Views/Home/perfil.cshtml");
    }

    public IActionResult CerrarSesion()
    {
        HttpContext.Session.Remove("EmailLogueado");
        HttpContext.Session.Clear();
        return RedirectToAction("Index");
    }

    public IActionResult Privacy()
    {
        return RedirectToAction("Privacy", "Home");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("~/Views/Shared/Error.cshtml", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    private string? GuardarArchivo(IFormFile? archivo, string subcarpeta)
    {
        if (archivo == null || archivo.Length == 0)
        {
            return null;
        }

        string webRootPath = _environment.WebRootPath;
        string folderPath = Path.Combine(webRootPath, subcarpeta.Replace('/', Path.DirectorySeparatorChar));
        Directory.CreateDirectory(folderPath);

        string extension = Path.GetExtension(archivo.FileName);
        string fileName = $"{Guid.NewGuid():N}{extension}";
        string fullPath = Path.Combine(folderPath, fileName);

        using FileStream stream = System.IO.File.Create(fullPath);
        archivo.CopyTo(stream);

        return $"/{subcarpeta}/{fileName}".Replace("\\", "/");
    }
}