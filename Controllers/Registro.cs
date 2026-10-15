using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TeAyudo.Models;

namespace TeAyudo.Controllers;

public class RegistroController : Controller
{
    private readonly ILogger<RegistroController> _logger;

    public RegistroController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Login()
    {
        string? emailSesion = HttpContext.Session.GetString("EmailLogueado");
        if (!string.IsNullOrEmpty(emailSesion))
        {
            return RedirectToAction("Perfil");
        }

        return View();
    }

    public IActionResult OlvideContrasena()
    {
        return View();
    }

    [HttpPost]
    public IActionResult OlvideContrasena(string Email)
    {
        if (string.IsNullOrWhiteSpace(Email))
        {
            ViewBag.ErrorMessage = "Debe ingresar un email.";
            return View();
        }

        BD bd = new BD();
        Usuarios? usuario = bd.ObtenerUsuarioPorEmail(Email);

        if (usuario == null)
        {
            ViewBag.ErrorMessage = "No existe un usuario con ese email.";
            return View();
        }

        ViewBag.SuccessMessage = "Usuario verificado. (Falta columna Contraseña en BD)";
        return View("MostrarContrasena");
    }

    [HttpPost]
    public IActionResult Login(string Email)
    {
        if (string.IsNullOrWhiteSpace(Email))
        {
            ViewBag.ErrorMessage = "Debe completar el email.";
            return View();
        }

        BD bd = new BD();
        Usuarios? usuario = bd.ObtenerUsuarioPorEmail(Email);

        if (usuario == null)
        {
            ViewBag.ErrorMessage = "Usuario incorrecto.";
            return View();
        }

        HttpContext.Session.SetString("EmailLogueado", usuario.Email);
        return RedirectToAction("Perfil");
    }

    public IActionResult Registro()
    {
        return View();
    }

    [HttpPost]
    public IActionResult GuardarRegistro(Usuarios USER)
    {
        if (USER == null)
        {
            ViewBag.ErrorMessage = "Debe completar los datos del registro.";
            return View("Registro");
        }

        BD bd = new BD();
        bool resultado = bd.GuardarUsuario(USER);

        if (resultado)
        {
            ViewBag.SuccessMessage = "Registro exitoso. Ahora puede iniciar sesión.";
            return View("Login");
        }

        ViewBag.ErrorMessage = "El email ya existe. Por favor, elige otro.";
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

        ViewBag.Email = usuario.Email;
        ViewBag.Nombre = usuario.Nombre;
        ViewBag.Apellido = usuario.Apellido;
        ViewBag.TipoUsuario = usuario.TipoUsuario;

        return View();
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
}