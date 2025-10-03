using FishApp.Views;

namespace FishApp;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(AquariumsPage), typeof(AquariumsPage));
        Routing.RegisterRoute(nameof(FishDatabasePage), typeof(FishDatabasePage));
        Routing.RegisterRoute(nameof(SearchPage), typeof(SearchPage));
        Routing.RegisterRoute(nameof(FishDetailPage), typeof(FishDetailPage));
    }
}
