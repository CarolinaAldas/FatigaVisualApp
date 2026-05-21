using FatigaVisualApp.ViewModels;

namespace FatigaVisualApp.Views;

public partial class RegistroView : ContentPage
{
    public RegistroView(RegistroViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}