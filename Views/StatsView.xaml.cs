using FatigaVisualApp.ViewModels;

namespace FatigaVisualApp.Views;

public partial class StatsView : ContentPage
{
    private readonly StatsViewModel _vm;

    public StatsView(StatsViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
        _vm = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _vm.InicializarAsync();
    }
}