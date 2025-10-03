using FishApp.Helpers;
using FishApp.ViewModels;
using Microsoft.Maui.Controls;

namespace FishApp.Views;

public partial class SearchPage : ContentPage
{
    public SearchPage()
    {
        InitializeComponent();
        BindingContext = ServiceHelper.GetService<SearchViewModel>();
    }
}
