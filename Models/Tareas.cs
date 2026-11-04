namespace TeAyudo.Models;

public class Tareas
{
    public int IdTarea { get; set; }
    public string Titulo { get; set; }
    public string Descripcion { get; set; }
    public bool Estado { get; set; }
    public DateTime FechaCreacion { get; set; }
    public string Lugar { get; set; }
    public string Direccion
    {
        get { return Lugar; }
        set { Lugar = value; }
    }
    public DateTime DiaTarea { get; set; }
    public DateTime DiaFecha { get; set; }
    public TimeSpan HoraTarea { get; set; }
    public string Aclaracion { get; set; }
    public double Monto { get; set; }
    public string TipoLugar { get; set; }
    public int IdUsuarioSolicitante { get; set; }
    public string Barrio { get; set; }

    public Tareas()
    {
        Titulo = "";
        Descripcion = "";
        Lugar = "";
        Aclaracion = "";
        TipoLugar = "";
        Barrio = "";
    }

    public Tareas(string titulo, string descripcion, bool estado, string lugar, DateTime diaTarea, TimeSpan horaTarea, string aclaracion, double monto, string tipoLugar, int idUsuarioSolicitante, string barrio)
    {
        Titulo = titulo;
        Descripcion = descripcion;
        Estado = estado;
        FechaCreacion = DateTime.Now;
        Lugar = lugar;
        DiaTarea = diaTarea;
        HoraTarea = horaTarea;
        Aclaracion = aclaracion;
        Monto = monto;
        TipoLugar = tipoLugar;
        IdUsuarioSolicitante = idUsuarioSolicitante;
        Barrio = barrio;
    }
}
