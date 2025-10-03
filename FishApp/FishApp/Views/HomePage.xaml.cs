using System;
using Microsoft.Maui.Controls;

namespace FishApp.Views;

public partial class HomePage : ContentPage
{
    public HomePage()
    {
        InitializeComponent();
    }

    private async void OnAquariumsClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(AquariumsPage));
    }

    private async void OnDatabaseClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(FishDatabasePage));
    }

    private async void OnSearchClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(SearchPage));
    }
}
