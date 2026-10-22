namespace TeAyudo.Models;

// Modelo Usuarios con getters y setters automáticos (Slides 60-63 del PDF)
public class Usuarios
{
    public int IdUsuario { get; set; }
    public string Nombre { get; set; } = "";
    public string Apellido { get; set; } = "";
    public string Email { get; set; } = "";
    public string Provincia { get; set; } = "";
    public int Direccion { get; set; }
    public string Calle { get; set; } = "";
    public string CodigoPostal { get; set; } = "";
    public DateTime FechaRegistro { get; set; }
    public bool TipoUsuario { get; set; }
    public string Contrasena { get; set; } = "";
    public bool AceptaTerminos { get; set; }
    public string FotoPerfil { get; set; } = "";

    // Constructor sin parámetros para Dapper
    public Usuarios()
    {
    }

    // Constructor con parámetros principales
    public Usuarios(string nombre, string apellido, string email, string provincia, int direccion, string calle, string codigoPostal, bool tipoUsuario, string contrasena, bool aceptaTerminos, string fotoPerfil)
    {
        Nombre = nombre;
        Apellido = apellido;
        Email = email;
        Provincia = provincia;
        Direccion = direccion;
        Calle = calle;
        CodigoPostal = codigoPostal;
        FechaRegistro = DateTime.Now;
        TipoUsuario = tipoUsuario;
        Contrasena = contrasena;
        AceptaTerminos = aceptaTerminos;
        FotoPerfil = fotoPerfil;
    }
}
