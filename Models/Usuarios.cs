namespace TeAyudo.Models;

public class Usuarios
{
    public int IdUsuario { get; set; }
    public string Nombre { get; set; }
    public string Apellido { get; set; }
    public string Email { get; set; }
    public string Provincia { get; set; }
    public int Direccion { get; set; }
    public string Calle { get; set; }
    public string CodigoPostal { get; set; }
    public DateTime FechaRegistro { get; set; }
    public bool TipoUsuario { get; set; }
}