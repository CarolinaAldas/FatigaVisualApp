using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace FatigaVisualApp.ViewModels;

public partial class NotificationsViewModel : ObservableObject
{
    private IDispatcherTimer? _timer;
    private int _segundosRestantes = 20;

    [ObservableProperty]
    string tiempoRestante = "00:20";

    [ObservableProperty]
    bool timerActivo = false;

    [ObservableProperty]
    string textoBoton = "Iniciar pausa";

    [ObservableProperty]
    bool notif2020Activa = true;

    [ObservableProperty]
    bool notifParpadeoActiva = true;

    [ObservableProperty]
    bool notifEvaluacionActiva = false;

    [ObservableProperty]
    bool notifNocturnoActiva = true;

    [ObservableProperty]
    int pausasCompletadas = 0;

    [RelayCommand]
    void ToggleTimer()
    {
        if (TimerActivo)
        {
            DetenerTimer();
        }
        else
        {
            IniciarTimer();
        }
    }

    private void IniciarTimer()
    {
        _segundosRestantes = 20;
        TimerActivo = true;
        TextoBoton = "Detener";

        _timer = Application.Current!.Dispatcher.CreateTimer();
        _timer.Interval = TimeSpan.FromSeconds(1);
        _timer.Tick += (s, e) =>
        {
            _segundosRestantes--;
            TiempoRestante = $"00:{_segundosRestantes:D2}";

            if (_segundosRestantes <= 0)
            {
                DetenerTimer();
                PausasCompletadas++;
                Application.Current?.MainPage?.DisplayAlert(
                    "Pausa completada",
                    "¡Excelente! Descansaste tu vista 20 segundos.",
                    "OK");
            }
        };
        _timer.Start();
    }

    private void DetenerTimer()
    {
        _timer?.Stop();
        _timer = null;
        TimerActivo = false;
        TextoBoton = "Iniciar pausa";
        _segundosRestantes = 20;
        TiempoRestante = "00:20";
    }
}