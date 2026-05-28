using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FatigaVisualApp.Services;

namespace FatigaVisualApp.ViewModels;

public partial class LoginViewModel : ObservableObject
{
    private readonly ApiService _api;

    public LoginViewModel(ApiService api)
    {
        _api = api;
    }

    [ObservableProperty]
    string correo = string.Empty;

    [ObservableProperty]
    string password = string.Empty;

    [ObservableProperty]
    string mensaje = string.Empty;

    [ObservableProperty]
    bool cargando = false;

    [RelayCommand]
    async Task Login()
    {
        if (string.IsNullOrWhiteSpace(Correo) ||
            string.IsNullOrWhiteSpace(Password))
        {
            Mensaje = "Completa todos los campos";
            return;
        }

        Cargando = true;
        Mensaje = string.Empty;

        var response = await _api.LoginAsync(Correo, Password);

        Cargando = false;

        if (response is null)
        {
            Mensaje = "Correo o contraseña incorrectos";
            return;
        }

        // Guardar sesión
        await SecureStorage.Default.SetAsync("jwt_token", response.Token);
        Preferences.Set("usuarioId", response.Usuario.Id);
        Preferences.Set("usuarioNombre", response.Usuario.Nombre);
        Preferences.Set("usuarioFoto", response.Usuario.FotoUrl ?? "");

        // Configurar token en HttpClient
        _api.SetAuthToken(response.Token);

        Mensaje = $"¡Bienvenido {response.Usuario.Nombre}!";
        await Task.Delay(800);
        await Shell.Current.GoToAsync("//HomeView");
    }

    [RelayCommand]
    async Task IrARegistro()
        => await Shell.Current.GoToAsync("//RegistroView");
}