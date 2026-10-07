using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TeAyudo.Models;

namespace TeAyudo.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        // Los contenidos de la portada se envían a la vista con un modelo simple.
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
                new ContenidoItem { Titulo = "¿Cómo solicito ayuda?", Texto = "Desde Solicitar tarea. El registro de solicitudes todavía está pendiente de implementación." },
                new ContenidoItem { Titulo = "¿Cómo puedo sumarme?", Texto = "Desde Trabajá con nosotros. El registro de postulaciones todavía está pendiente de implementación." }
            }
        };
        return View(modelo);
    }

    // Cada acción devuelve su vista de Views/Home.
    public IActionResult Ingresar()
    {
        return View();
    }

    public IActionResult SolicitarTarea()
    {
        return View();
    }

    public IActionResult Trabajar()
    {
        return View();
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
