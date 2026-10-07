namespace TeAyudo.Models;

public class HomeViewModel
{
    public string Titulo { get; set; } = "";
    public string[] Presentacion { get; set; } = Array.Empty<string>();
    public string[] SobreNosotros { get; set; } = Array.Empty<string>();
    public string AyudaTitulo { get; set; } = "";
    public string AyudaTexto { get; set; } = "";
    public string TrabajoTitulo { get; set; } = "";
    public string TrabajoTexto { get; set; } = "";
    public ContenidoItem[] Pasos { get; set; } = Array.Empty<ContenidoItem>();
    public ContenidoItem[] Valores { get; set; } = Array.Empty<ContenidoItem>();
    public ContenidoItem[] Preguntas { get; set; } = Array.Empty<ContenidoItem>();
}

public class ContenidoItem
{
    public string Titulo { get; set; } = "";
    public string Texto { get; set; } = "";
    public string Icono { get; set; } = "";
}
