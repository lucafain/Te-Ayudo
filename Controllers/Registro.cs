using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TeAyudo.Models;

namespace TeAyudo.Controllers;

public class RegistroController : Controller
{
    private readonly ILogger<RegistroController> _logger;

    public RegistroController(ILogger<RegistroController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return RedirectToAction("Index", "Home");
    }

    public IActionResult Login()
    {
        string? emailSesion = HttpContext.Session.GetString("EmailLogueado");
        if (!string.IsNullOrEmpty(emailSesion))
        {
            return RedirectToAction("Perfil");
        }

        return View("Login");
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
        Usuarios? usuario = bd.ObtenerUsuarioPorCredenciales(Email, Contrasena);

        if (usuario == null)
        {
            ViewBag.ErrorMessage = "Email o contraseña incorrectos.";
            return View("Login");
        }

        HttpContext.Session.SetString("EmailLogueado", usuario.Email ?? string.Empty);
        return RedirectToAction("Perfil");
    }

    public IActionResult Registro()
    {
        return View("Registro");
    }

    [HttpPost]
    public IActionResult GuardarRegistro(Usuarios USER, IFormFile? FotoPerfil)
    {
        if (USER == null)
        {
            ViewBag.ErrorMessage = "Debe completar los datos del registro.";
            return View("Registro");
        }

        USER.TipoUsuario ??= "Adulto mayor";
        USER.FotoPerfilRuta = FotoPerfil?.FileName;

        BD bd = new BD();
        bool resultado;

        try
        {
            resultado = bd.GuardarUsuario(USER);
        }
        catch
        {
            ViewBag.ErrorMessage = "No se pudo guardar el registro en la base de datos.";
            return View("Registro");
        }

        if (resultado)
        {
            ViewBag.SuccessMessage = "Registro exitoso. Ahora puede iniciar sesión.";
            return View("Login");
        }

        bool emailExiste = !string.IsNullOrWhiteSpace(USER.Email) && bd.ObtenerUsuarioPorEmail(USER.Email) != null;
        ViewBag.ErrorMessage = emailExiste
            ? "El email ya existe. Por favor, elige otro."
            : "No se pudo guardar el registro en la base de datos.";
        return View("Registro");
    }

    public IActionResult Perfil()
    {
        string? emailSesion = HttpContext.Session.GetString("EmailLogueado");
        if (string.IsNullOrEmpty(emailSesion))
        {
            return RedirectToAction("Login");
        }

        BD bd = new BD();
        Usuarios? usuario = bd.ObtenerUsuarioPorEmail(emailSesion);
        if (usuario == null)
        {
            HttpContext.Session.Remove("EmailLogueado");
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        ViewBag.NombreUsuario = usuario.Email;
        ViewBag.Email = usuario.Email;
        ViewBag.Nombre = usuario.Nombre;
        ViewBag.Apellido = usuario.Apellido;
        ViewBag.TipoUsuario = usuario.TipoUsuario;

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
        return RedirectToAction("Privacy", "Home");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("~/Views/Shared/Error.cshtml", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}