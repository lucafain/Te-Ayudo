namespace TeAyudo.Models;

public class Postulaciones
{
    public int IdPostulacion { get; set; }
    public int IdUsuario { get; set; }
    public int IdTarea { get; set; }
    public string Estado { get; set; }
    public double Calificacion { get; set; }
    public bool Puntual { get; set; }

    public Postulaciones()
    {
        Estado = "";
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
