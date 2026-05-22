using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FatigaVisualApp.Models;
using FatigaVisualApp.Services;

namespace FatigaVisualApp.ViewModels;

public partial class StatsViewModel : ObservableObject
{
    private readonly ApiService _api;

    public StatsViewModel(ApiService api)
    {
        _api = api;
    }

    [ObservableProperty]
    int ultimoIndice = 0;

    [ObservableProperty]
    string ultimoNivel = "Sin datos";

    [ObservableProperty]
    string colorNivel = "#888888";

    [ObservableProperty]
    int totalEvaluaciones = 0;

    [ObservableProperty]
    int mejorIndice = 0;

    [ObservableProperty]
    int peorIndice = 0;

    [ObservableProperty]
    string tendencia = "—";

    [ObservableProperty]
    List<Evaluacion> historial = [];

    [ObservableProperty]
    bool cargando = false;

    [ObservableProperty]
    bool hayDatos = false;

    public async Task InicializarAsync()
    {
        Cargando = true;
        int usuarioId = Preferences.Get("usuarioId", 0);

        var evaluaciones = await _api.GetEvaluacionesAsync(usuarioId);

        Cargando = false;

        if (evaluaciones is null || evaluaciones.Count == 0)
        {
            HayDatos = false;
            return;
        }

        HayDatos = true;
        Historial = evaluaciones.Take(7).ToList();
        TotalEvaluaciones = evaluaciones.Count;

        var ultima = evaluaciones.First();
        UltimoIndice = ultima.IndiceFatiga;
        UltimoNivel = ultima.Nivel switch
        {
            "bajo" => "Nivel bajo 😊",
            "medio" => "Nivel moderado 😐",
            "alto" => "Nivel alto 😟",
            _ => "Sin datos"
        };
        ColorNivel = ultima.Nivel switch
        {
            "bajo" => "#1D9E75",
            "medio" => "#EF9F27",
            "alto" => "#E24B4A",
            _ => "#888888"
        };

        MejorIndice = evaluaciones.Min(e => e.IndiceFatiga);
        PeorIndice = evaluaciones.Max(e => e.IndiceFatiga);

        if (evaluaciones.Count >= 2)
        {
            var diff = evaluaciones[0].IndiceFatiga - evaluaciones[1].IndiceFatiga;
            Tendencia = diff < 0 ? "↓ Mejorando" : diff > 0 ? "↑ Empeorando" : "→ Estable";
        }
        else
        {
            Tendencia = "Primera evaluación";
        }
    }

    [RelayCommand]
    async Task IrACuestionario()
        => await Shell.Current.GoToAsync("//QuizView");

    [RelayCommand]
    async Task Volver()
        => await Shell.Current.GoToAsync("//HomeView");
}