using FatigaVisualApp.ViewModels;

namespace FatigaVisualApp.Views;

public partial class LearnView : ContentPage
{
    public LearnView(LearnViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}