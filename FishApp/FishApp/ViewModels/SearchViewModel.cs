using System.Threading.Tasks;
using System.Windows.Input;
using FishApp.Models;
using FishApp.Services;
using Microsoft.Maui.Controls;

namespace FishApp.ViewModels;

public class SearchViewModel : BaseViewModel
{
    private readonly FishDatabaseService _databaseService;
    private string _query = string.Empty;
    private Fish? _result;
    private bool _hasSearched;

    public SearchViewModel(FishDatabaseService databaseService)
    {
        _databaseService = databaseService;
        SearchCommand = new Command(async () => await SearchAsync());
    }

    public string Query
    {
        get => _query;
        set => SetProperty(ref _query, value);
    }

    public Fish? Result
    {
        get => _result;
        private set => SetProperty(ref _result, value);
    }

    public ICommand SearchCommand { get; }

    public Task SearchAsync()
    {
        _hasSearched = true;
        Result = _databaseService.GetByName(Query);
        OnPropertyChanged(nameof(ShowNoResult));
        return Task.CompletedTask;
    }

    public bool ShowNoResult => _hasSearched && Result is null;
}
