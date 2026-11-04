namespace TeAyudo.Models;

public class ContenidoItem
{
    public string Titulo { get; set; }
    public string Texto { get; set; }
    public string Icono { get; set; }

    public ContenidoItem()
    {
        Titulo = "";
        Texto = "";
        Icono = "";
    }

    public ContenidoItem(string titulo, string texto, string icono = "")
    {
        Titulo = titulo;
        Texto = texto;
        Icono = icono;
    }
}
