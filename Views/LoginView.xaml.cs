using FatigaVisualApp.ViewModels;

namespace FatigaVisualApp.Views;

public partial class LoginView : ContentPage
{
    public LoginView(LoginViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}