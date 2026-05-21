namespace FatigaVisualApp.Models;

public class Evaluacion
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }
    public DateTime Fecha { get; set; }
    public string RespuestasJson { get; set; } = string.Empty;
    public int IndiceFatiga { get; set; }
    public string Nivel { get; set; } = string.Empty;
}