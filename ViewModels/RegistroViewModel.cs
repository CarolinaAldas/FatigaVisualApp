using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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

        var response = await _api.RegistroAsync(Nombre, Correo, Password);

        Cargando = false;

        if (response is null)
        {
            Mensaje = "Error al registrar. El correo puede estar en uso.";
            return;
        }

        // Guardar sesión
        await SecureStorage.Default.SetAsync("jwt_token", response.Token);
        Preferences.Set("usuarioId", response.Usuario.Id);
        Preferences.Set("usuarioNombre", response.Usuario.Nombre);
        Preferences.Set("usuarioFoto", response.Usuario.FotoUrl ?? "");

        _api.SetAuthToken(response.Token);

        Mensaje = $"¡Bienvenido {response.Usuario.Nombre}!";
        await Task.Delay(800);
        await Shell.Current.GoToAsync("//HomeView");
    }

    [RelayCommand]
    async Task LoginGoogle()
    {
        try
        {
            Cargando = true;
            Mensaje = string.Empty;

#if ANDROID || IOS
            var authResult = await WebAuthenticator.Default.AuthenticateAsync(
                new WebAuthenticatorOptions
                {
                    Url = new Uri("https://accounts.google.com/o/oauth2/auth?" +
                    "client_id=60141951582-no7fcejf468ihrrtei5fpgq76it2q7dn.apps.googleusercontent.com" +
                    "&redirect_uri=http%3A%2F%2Flocalhost%3A5062%2Fsignin-google" +
                    "&response_type=code" +
                    "&scope=openid%20email%20profile"),
                                    CallbackUrl = new Uri("fatigavisual://callback")
                });

            if (authResult?.AccessToken is not null)
            {
                var response = await _api.LoginGoogleAsync(authResult.AccessToken);
                if (response is not null)
                {
                    await SecureStorage.Default.SetAsync("jwt_token", response.Token);
                    Preferences.Set("usuarioId", response.Usuario.Id);
                    Preferences.Set("usuarioNombre", response.Usuario.Nombre);
                    Preferences.Set("usuarioFoto", response.Usuario.FotoUrl ?? "");
                    _api.SetAuthToken(response.Token);
                    await Shell.Current.GoToAsync("//HomeView");
                }
            }
#else
            await Browser.Default.OpenAsync(
                "https://accounts.google.com/o/oauth2/auth?" +
                "client_id=60141951582-jgo927gg5narh4rplsm1agm5tvt9uqj2.apps.googleusercontent.com" +
                "&redirect_uri=http%3A%2F%2Flocalhost%3A5062%2Fsignin-google" +
                "&response_type=code" +
                "&scope=openid%20email%20profile",
                BrowserLaunchMode.SystemPreferred);
            Mensaje = "Continúa en el navegador";
#endif
            Cargando = false;
        }
        catch (TaskCanceledException)
        {
            Cargando = false;
            Mensaje = "Login cancelado";
        }
        catch (Exception ex)
        {
            Cargando = false;
            Mensaje = "Error: " + ex.Message;
        }
    }

    [RelayCommand]
    async Task IrALogin()
        => await Shell.Current.GoToAsync("//LoginView");
}