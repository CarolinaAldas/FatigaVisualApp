using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Reflection;

namespace FatigaVisualApp.ViewModels;

public class ModuloEducativo
{
    public string Icono { get; set; } = string.Empty;
    public string Titulo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string Detalle { get; set; } = string.Empty;
    public string Etiqueta { get; set; } = string.Empty;
    public string ColorEtiqueta { get; set; } = "#1D9E75";
    public string ColorFondo { get; set; } = "#E1F5EE";
}

public partial class LearnViewModel : ObservableObject
{
    [ObservableProperty]
    List<ModuloEducativo> modulos = [];

    [ObservableProperty]
    List<ModuloEducativo> modulosFiltrados = [];

    [ObservableProperty]
    string filtroActivo = "Todos";

    public LearnViewModel()
    {
        CargarModulos();
    }

    private void CargarModulos()
    {
        Modulos =
        [
            new() {
                Icono = "👁️",
                Titulo = "Regla 20-20-20",
                Descripcion = "Cada 20 min · 20 seg · 20 pies",
                Detalle = "Cada 20 minutos, mira algo a 20 pies de distancia por 20 segundos. Reduce la fatiga ocular hasta un 30%.",
                Etiqueta = "Esencial",
                ColorEtiqueta = "#085041",
                ColorFondo = "#E1F5EE"
            },
            new() {
                Icono = "🎨",
                Titulo = "Prueba de percepción del color",
                Descripcion = "Test Ishihara referencial",
                Detalle = "Prueba básica para detectar posibles alteraciones en la percepción del color. Solo referencial, no reemplaza diagnóstico médico.",
                Etiqueta = "Test",
                ColorEtiqueta = "#3C3489",
                ColorFondo = "#EEEDFE"
            },
            new() {
                Icono = "💡",
                Titulo = "Configuración de pantalla",
                Descripcion = "Brillo, contraste y luz azul",
                Detalle = "Ajusta el brillo al nivel del ambiente. Activa el filtro de luz azul después de las 8 PM. Usa modo oscuro cuando sea posible.",
                Etiqueta = "Tip",
                ColorEtiqueta = "#633806",
                ColorFondo = "#FAEEDA"
            },
            new() {
                Icono = "🏃",
                Titulo = "Ejercicios de relajación",
                Descripcion = "Palming y enfoque dinámico",
                Detalle = "Palming: cubre los ojos con las palmas 30 segundos. Enfoque dinámico: alterna la vista entre un objeto cercano y lejano 10 veces.",
                Etiqueta = "Ejercicio",
                ColorEtiqueta = "#712B13",
                ColorFondo = "#FAECE7"
            },
            new() {
                Icono = "💧",
                Titulo = "Hidratación ocular",
                Descripcion = "Parpadeo consciente y gotas",
                Detalle = "Parpadea conscientemente cada 4 segundos frente a pantallas. Considera usar lágrimas artificiales si sientes sequedad.",
                Etiqueta = "Salud",
                ColorEtiqueta = "#27500A",
                ColorFondo = "#EAF3DE"
            },
            new() {
                Icono = "🌙",
                Titulo = "Higiene del sueño visual",
                Descripcion = "Descanso nocturno para los ojos",
                Detalle = "Evita pantallas 1 hora antes de dormir. La luz azul suprime la melatonina y afecta la calidad del sueño y la recuperación ocular.",
                Etiqueta = "Salud",
                ColorEtiqueta = "#27500A",
                ColorFondo = "#EAF3DE"
            },
        ];

        AplicarFiltro("Todos");
    }

    [RelayCommand]
    void Filtrar(string filtro)
    {
        FiltroActivo = filtro;
        AplicarFiltro(filtro);
    }

    private void AplicarFiltro(string filtro)
    {
        ModulosFiltrados = filtro switch
        {
            "Ejercicios" => Modulos.Where(m => m.Etiqueta == "Ejercicio").ToList(),
            "Tests" => Modulos.Where(m => m.Etiqueta == "Test").ToList(),
            "Tips" => Modulos.Where(m => m.Etiqueta == "Tip" || m.Etiqueta == "Salud").ToList(),
            _ => Modulos.ToList()
        };
    }
}