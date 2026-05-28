namespace FatigaVisualApp.Views;

public partial class SplashView : ContentPage
{
    public SplashView()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await RunSplashAnimation();
    }

    private async Task RunSplashAnimation()
    {
        // Glow de fondo
        await AmbientGlow.FadeTo(1, 600);

        // Pausa con ojo cerrado
        await Task.Delay(400);

        // Ojo abre — fade cruzado
        await Task.WhenAll(
            EyeClosed.FadeTo(0, 900, Easing.CubicIn),
            EyeOpen.FadeTo(1, 900, Easing.CubicOut)
        );

        // Texto aparece
        await Task.Delay(200);
        await Task.WhenAll(
            TextBlock.FadeTo(1, 700, Easing.CubicOut),
            TextBlock.TranslateTo(0, 0, 700, Easing.CubicOut)
        );

        // Dots aparecen
        await DotsLoader.FadeTo(1, 400);
        _ = AnimateDots();

        // Esperar
        await Task.Delay(2000);

        // Ojo cierra
        await Task.WhenAll(
            EyeOpen.FadeTo(0, 600, Easing.CubicIn),
            EyeClosed.FadeTo(1, 600, Easing.CubicOut),
            TextBlock.FadeTo(0, 500),
            DotsLoader.FadeTo(0, 400)
        );

        await Task.Delay(300);

        // Navegar al Login
        await Shell.Current.GoToAsync("//LoginView");
    }

    private async Task AnimateDots()
    {
        while (true)
        {
            await Dot1.FadeTo(1, 300);
            await Dot1.FadeTo(0.2, 300);
            await Dot2.FadeTo(1, 300);
            await Dot2.FadeTo(0.2, 300);
            await Dot3.FadeTo(1, 300);
            await Dot3.FadeTo(0.2, 300);
        }
    }
}