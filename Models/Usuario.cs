namespace TeAyudo.Models;

// Alias singular para compatibilidad con el estándar de Models (Slide 31 del PDF: Clase Auto)
public class Usuario : Usuarios
{
    public Usuario() : base()
    {
    }

    public Usuario(string email, string contrasena, string tipoUsuario, string? nombreUsuario = null)
        : base(email, contrasena, tipoUsuario, nombreUsuario)
    {
    }
}
