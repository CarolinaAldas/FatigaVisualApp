using FatigaVisualApp.ViewModels;

namespace FatigaVisualApp.Views;

public partial class QuizView : ContentPage
{
    public QuizView(QuizViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}