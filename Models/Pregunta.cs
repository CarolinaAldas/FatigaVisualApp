namespace FatigaVisualApp.Models;

public class Pregunta
{
    public string Texto { get; set; } = string.Empty;
    public List<string> Opciones { get; set; } = [];
    public int RespuestaSeleccionada { get; set; } = -1;
}