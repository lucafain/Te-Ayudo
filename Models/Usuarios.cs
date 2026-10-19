using System.ComponentModel.DataAnnotations;

namespace TeAyudo.Models;

public class Usuarios
{
    public int IdUsuario { get; set; }

    [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
    [StringLength(100)]
    public string? NombreUsuario { get; set; }

    [StringLength(100)]
    public string? Nombre { get; set; }

    [StringLength(100)]
    public string? Apellido { get; set; }

    [Required(ErrorMessage = "El email es obligatorio.")]
    [EmailAddress(ErrorMessage = "Email no válido.")]
    [StringLength(150)]
    public string? Email { get; set; }

    [Phone]
    [StringLength(30)]
    public string? Telefono { get; set; }

    [StringLength(100)]
    public string? Provincia { get; set; }

    [StringLength(150)]
    public string? Direccion { get; set; }

    [StringLength(150)]
    public string? Calle { get; set; }

    [StringLength(20)]
    public string? CodigoPostal { get; set; }

    public DateTime FechaRegistro { get; set; }

    [StringLength(30)]
    public string? TipoUsuario { get; set; }

    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    [StringLength(200)]
    public string? Contrasena { get; set; }

    [StringLength(260)]
    public string? FotoPerfilRuta { get; set; }

    [StringLength(120)]
    public string? AmbitoLaboral { get; set; }

    [StringLength(300)]
    public string? LinkedIn { get; set; }

    [StringLength(260)]
    public string? CvRuta { get; set; }

    public bool AceptaTerminos { get; set; }
}