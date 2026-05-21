using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FatigaVisualApp.Services;

namespace FatigaVisualApp.ViewModels;

public partial class HomeViewModel : ObservableObject
{
    private readonly ApiService _api;

    public HomeViewModel(ApiService api)
    {
        _api = api;
    }

    [ObservableProperty]
    string nombreUsuario = string.Empty;

    [ObservableProperty]
    int indiceFatiga = 0;

    [ObservableProperty]
    string nivelFatiga = "Sin evaluación";

    [ObservableProperty]
    string colorNivel = "#888888";

    [ObservableProperty]
    int pausasHoy = 0;

    [ObservableProperty]
    int rachaDias = 0;

    [ObservableProperty]
    string fecha = DateTime.Now.ToString("dddd, dd 'de' MMMM");

    public async Task InicializarAsync()
    {
        NombreUsuario = Preferences.Get("usuarioNombre", "Usuario");
        int usuarioId = Preferences.Get("usuarioId", 0);

        // Trae la última evaluación
        var evaluaciones = await _api.GetEvaluacionesAsync(usuarioId);
        if (evaluaciones is not null && evaluaciones.Count > 0)
        {
            var ultima = evaluaciones.First();
            IndiceFatiga = ultima.IndiceFatiga;
            NivelFatiga = ultima.Nivel;

            NivelFatiga = ultima.Nivel switch
            {
                "bajo" => "Nivel bajo — ¡Excelente!",
                "medio" => "Nivel moderado — mejorando",
                "alto" => "Nivel alto — necesitas pausas",
                _ => "Sin evaluación"
            };

            ColorNivel = ultima.Nivel switch
            {
                "bajo" => "#1D9E75",
                "medio" => "#EF9F27",
                "alto" => "#E24B4A",
                _ => "#888888"
            };
        }

        // Trae estadísticas
        var stats = await _api.GetEstadisticasAsync(usuarioId);
        if (stats is not null && stats.Count > 0)
        {
            var hoy = stats.FirstOrDefault(s =>
                s.Fecha.Date == DateTime.Today);
            PausasHoy = hoy?.PausasCompletadas ?? 0;
            RachaDias = stats.Max(s => s.RachaDias);
        }
    }

    [RelayCommand]
    async Task IrACuestionario()
        => await Shell.Current.GoToAsync("//QuizView");

    [RelayCommand]
    async Task IrAEstadisticas()
        => await Shell.Current.GoToAsync("//StatsView");

    [RelayCommand]
    async Task CerrarSesion()
    {
        Preferences.Remove("usuarioId");
        Preferences.Remove("usuarioNombre");
        await Shell.Current.GoToAsync("//LoginView");
    }
}