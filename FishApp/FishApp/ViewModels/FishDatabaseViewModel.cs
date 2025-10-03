using System.Collections.ObjectModel;
using System.Linq;
using FishApp.Models;
using FishApp.Services;

namespace FishApp.ViewModels;

public class FishDatabaseViewModel : BaseViewModel
{
    private readonly FishDatabaseService _databaseService;
    private string _searchText = string.Empty;

    public FishDatabaseViewModel(FishDatabaseService databaseService)
    {
        _databaseService = databaseService;
        AllFish = _databaseService.GetAllFish();
        FilteredFish = new ObservableCollection<Fish>(AllFish);
    }

    public ObservableCollection<Fish> AllFish { get; }

    public ObservableCollection<Fish> FilteredFish { get; }

    public string SearchText
    {
        get => _searchText;
        set
        {
            if (SetProperty(ref _searchText, value))
            {
                ApplyFilter();
            }
        }
    }

    public void ApplyFilter()
    {
        FilteredFish.Clear();
        var query = string.IsNullOrWhiteSpace(SearchText) ? AllFish : AllFish.Where(f => f.Name.Contains(SearchText, System.StringComparison.OrdinalIgnoreCase));
        foreach (var fish in query)
        {
            FilteredFish.Add(fish);
        }
    }
}
