namespace TeAyudo.Models;

// Modelo Usuarios con getters y setters automáticos (Slides 60-63 del PDF)
public class Usuarios
{
    public int IdUsuario { get; set; }
    public string? NombreUsuario { get; set; }
    public string? Nombre { get; set; }
    public string? Apellido { get; set; }
    public string? Email { get; set; }
    public string? Telefono { get; set; }
    public string? Provincia { get; set; }
    public string? Direccion { get; set; }
    public string? Calle { get; set; }
    public string? CodigoPostal { get; set; }
    public DateTime FechaRegistro { get; set; }
    public string? TipoUsuario { get; set; }
    public string? Contrasena { get; set; }
    public string? FotoPerfilRuta { get; set; }
    public string? AmbitoLaboral { get; set; }
    public string? LinkedIn { get; set; }
    public string? CvRuta { get; set; }
    public bool AceptaTerminos { get; set; }

    // Constructor sin parámetros para Dapper y deserialización
    public Usuarios()
    {
    }

    // Constructor con parámetros principales
    public Usuarios(string email, string contrasena, string tipoUsuario, string? nombreUsuario = null)
    {
        Email = email;
        Contrasena = contrasena;
        TipoUsuario = tipoUsuario;
        NombreUsuario = nombreUsuario;
        FechaRegistro = DateTime.Now;
    }
}
