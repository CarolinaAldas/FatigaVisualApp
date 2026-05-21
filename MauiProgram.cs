using FatigaVisualApp.Services;
using FatigaVisualApp.ViewModels;
using FatigaVisualApp.Views;

namespace FatigaVisualApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // Servicios
        builder.Services.AddSingleton<ApiService>();

        // ViewModels
        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<RegistroViewModel>();

        // Views
        builder.Services.AddTransient<LoginView>();
        builder.Services.AddTransient<RegistroView>();

        return builder.Build();
    }
}