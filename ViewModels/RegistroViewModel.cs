using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FatigaVisualApp.Models;
using FatigaVisualApp.Services;

namespace FatigaVisualApp.ViewModels;

public partial class RegistroViewModel : ObservableObject
{
    private readonly ApiService _api;

    public RegistroViewModel(ApiService api)
    {
        _api = api;
    }

    [ObservableProperty]
    string nombre = string.Empty;

    [ObservableProperty]
    string correo = string.Empty;

    [ObservableProperty]
    string password = string.Empty;

    [ObservableProperty]
    string mensaje = string.Empty;

    [ObservableProperty]
    bool cargando = false;

    [RelayCommand]
    async Task Registrar()
    {
        if (string.IsNullOrWhiteSpace(Nombre) ||
            string.IsNullOrWhiteSpace(Correo) ||
            string.IsNullOrWhiteSpace(Password))
        {
            Mensaje = "Completa todos los campos";
            return;
        }

        Cargando = true;
        Mensaje = string.Empty;

        var usuario = new Usuario
        {
            Nombre = Nombre,
            Correo = Correo,
            PasswordHash = Password
        };

        var resultado = await _api.CrearUsuarioAsync(usuario);

        Cargando = false;

        if (resultado is not null)
        {
            Mensaje = $"¡Bienvenido {resultado.Nombre}!";
            await Shell.Current.GoToAsync("//LoginView");
        }
        else
        {
            Mensaje = "Error al registrar. Intenta de nuevo.";
        }
    }

    [RelayCommand]
    async Task IrALogin()
        => await Shell.Current.GoToAsync("//LoginView");
}