using System;
using FishApp.Helpers;
using FishApp.ViewModels;
using Microsoft.Maui.Controls;

namespace FishApp.Views;

public partial class AquariumsPage : ContentPage
{
    private readonly AquariumsViewModel _viewModel;

    public AquariumsPage()
    {
        InitializeComponent();
        _viewModel = ServiceHelper.GetService<AquariumsViewModel>();
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.InitializeAsync();
    }

    private async void OnAddFishClicked(object sender, EventArgs e)
    {
        var result = await _viewModel.TryAddFishToSelectedAquariumAsync();
        if (!result.Success)
        {
            await DisplayAlert("Warnung", result.Message, "OK");
        }
    }

    private async void OnDeleteAquariumClicked(object sender, EventArgs e)
    {
        if (_viewModel.SelectedAquarium is null)
        {
            return;
        }

        var confirm = await DisplayAlert("Aquarium löschen", $"Möchtest du {_viewModel.SelectedAquarium.Name} wirklich löschen?", "Ja", "Nein");
        if (confirm)
        {
            await _viewModel.DeleteSelectedAquariumAsync();
        }
    }
}
