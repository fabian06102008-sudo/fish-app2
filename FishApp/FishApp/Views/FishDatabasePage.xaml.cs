using System.Collections.Generic;
using System.Linq;
using FishApp.Helpers;
using FishApp.Models;
using FishApp.ViewModels;
using Microsoft.Maui.Controls;

namespace FishApp.Views;

public partial class FishDatabasePage : ContentPage
{
    private readonly FishDatabaseViewModel _viewModel;

    public FishDatabasePage()
    {
        InitializeComponent();
        _viewModel = ServiceHelper.GetService<FishDatabaseViewModel>();
        BindingContext = _viewModel;
    }

    private async void OnFishSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is Fish selected)
        {
            await Shell.Current.GoToAsync(nameof(FishDetailPage), new Dictionary<string, object>
            {
                ["Fish"] = selected
            });
            ((CollectionView)sender).SelectedItem = null;
        }
    }
}
