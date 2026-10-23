namespace TeAyudo.Models;

// Modelo Postulaciones con getters y setters automáticos (Slides 60-63 del PDF)
// estado (varchar 50): "Pendiente", "Aceptada", "Rechazada" (la rechazó el adulto) o "Descartada" (la descartó el joven)
public class Postulaciones
{
    public int IdPostulacion { get; set; }
    public int IdUsuario { get; set; }
    public int IdTarea { get; set; }
    public string Estado { get; set; } = "";
    public double Calificacion { get; set; }
    public bool Puntual { get; set; }

    public Postulaciones()
    {
    }

    public Postulaciones(int idUsuario, int idTarea, string estado, double calificacion, bool puntual)
    {
        IdUsuario = idUsuario;
        IdTarea = idTarea;
        Estado = estado;
        Calificacion = calificacion;
        Puntual = puntual;
    }
}
