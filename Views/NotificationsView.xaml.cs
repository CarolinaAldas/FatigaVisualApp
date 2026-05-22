using FatigaVisualApp.ViewModels;

namespace FatigaVisualApp.Views;

public partial class NotificationsView : ContentPage
{
    public NotificationsView(NotificationsViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}