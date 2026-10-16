namespace TeAyudo.Models;

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
}