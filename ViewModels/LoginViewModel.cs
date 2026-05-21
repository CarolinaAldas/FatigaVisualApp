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

        // Trae todos los usuarios y busca por correo y password
        var usuarios = await _api.GetUsuariosAsync();

        Cargando = false;

        if (usuarios is null)
        {
            Mensaje = "Error de conexión con el servidor";
            return;
        }

        var usuario = usuarios.FirstOrDefault(u =>
            u.Correo.ToLower() == Correo.ToLower() &&
            u.PasswordHash == Password);

        if (usuario is null)
        {
            Mensaje = "Correo o contraseña incorrectos";
            return;
        }

        // Login exitoso — guarda el usuario en preferencias
        Preferences.Set("usuarioId", usuario.Id);
        Preferences.Set("usuarioNombre", usuario.Nombre);

        Mensaje = $"¡Bienvenido {usuario.Nombre}!";

        await Task.Delay(1000);

        // Navega a la pantalla principal
        await Shell.Current.GoToAsync("//HomeView");
    }

    [RelayCommand]
    async Task IrARegistro()
        => await Shell.Current.GoToAsync("//RegistroView");
}