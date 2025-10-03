using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using FishApp.Models;
using FishApp.Services;
using Microsoft.Maui.Controls;

namespace FishApp.ViewModels;

public class ValidationResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;

    public static ValidationResult Ok() => new() { Success = true };
    public static ValidationResult Fail(string message) => new() { Success = false, Message = message };
}

public class AquariumsViewModel : BaseViewModel
{
    private readonly JsonStorageService _storageService;
    private readonly FishDatabaseService _fishDatabaseService;
    private AquariumData _data = new();
    private Aquarium? _selectedAquarium;
    private Fish? _selectedFish;
    private bool _isBusy;
    private string _newAquariumName = string.Empty;
    private int _newAquariumLiters;
    private double _newAquariumPh = 7.0;
    private double _newAquariumGh = 8.0;
    private double _newAquariumKh = 6.0;
    private double _newAquariumTemperature = 25.0;

    public AquariumsViewModel(JsonStorageService storageService, FishDatabaseService fishDatabaseService)
    {
        _storageService = storageService;
        _fishDatabaseService = fishDatabaseService;

        AvailableFish = _fishDatabaseService.GetAllFish();

        AddAquariumCommand = new Command(async () => await AddAquariumAsync(), () => !IsBusy);
        DeleteAquariumCommand = new Command<Aquarium>(async aquarium => await DeleteAquariumAsync(aquarium));
    }

    public ObservableCollection<Aquarium> Aquariums => _data.Aquariums;

    public ObservableCollection<Fish> AvailableFish { get; }

    public ICommand AddAquariumCommand { get; }

    public ICommand DeleteAquariumCommand { get; }

    public Aquarium? SelectedAquarium
    {
        get => _selectedAquarium;
        set => SetProperty(ref _selectedAquarium, value);
    }

    public Fish? SelectedFish
    {
        get => _selectedFish;
        set => SetProperty(ref _selectedFish, value);
    }

    public bool IsBusy
    {
        get => _isBusy;
        private set
        {
            if (SetProperty(ref _isBusy, value))
            {
                (AddAquariumCommand as Command)?.ChangeCanExecute();
            }
        }
    }

    public string NewAquariumName
    {
        get => _newAquariumName;
        set => SetProperty(ref _newAquariumName, value);
    }

    public int NewAquariumLiters
    {
        get => _newAquariumLiters;
        set => SetProperty(ref _newAquariumLiters, value);
    }

    public double NewAquariumPh
    {
        get => _newAquariumPh;
        set => SetProperty(ref _newAquariumPh, value);
    }

    public double NewAquariumGh
    {
        get => _newAquariumGh;
        set => SetProperty(ref _newAquariumGh, value);
    }

    public double NewAquariumKh
    {
        get => _newAquariumKh;
        set => SetProperty(ref _newAquariumKh, value);
    }

    public double NewAquariumTemperature
    {
        get => _newAquariumTemperature;
        set => SetProperty(ref _newAquariumTemperature, value);
    }

    public async Task InitializeAsync()
    {
        if (Aquariums.Any())
        {
            return;
        }

        IsBusy = true;
        try
        {
            _data = await _storageService.LoadAsync();
            OnPropertyChanged(nameof(Aquariums));
            SelectedAquarium ??= Aquariums.FirstOrDefault();
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task AddAquariumAsync()
    {
        if (string.IsNullOrWhiteSpace(NewAquariumName))
        {
            return;
        }

        if (NewAquariumLiters <= 0)
        {
            return;
        }

        var parameters = new AquariumWaterParameters
        {
            Ph = NewAquariumPh,
            Gh = NewAquariumGh,
            Kh = NewAquariumKh,
            Temperature = NewAquariumTemperature
        };

        var aquarium = new Aquarium(NewAquariumName.Trim(), NewAquariumLiters, parameters);
        Aquariums.Add(aquarium);
        SelectedAquarium = aquarium;
        await _storageService.SaveAsync(_data);

        // reset fields after save
        NewAquariumName = string.Empty;
        NewAquariumLiters = 0;
    }

    private async Task DeleteAquariumAsync(Aquarium? aquarium)
    {
        if (aquarium is null)
        {
            return;
        }

        if (Aquariums.Contains(aquarium))
        {
            Aquariums.Remove(aquarium);
            await _storageService.SaveAsync(_data);
            if (SelectedAquarium == aquarium)
            {
                SelectedAquarium = null;
            }
        }
    }

    public Task DeleteSelectedAquariumAsync() => DeleteAquariumAsync(SelectedAquarium);

    /// <summary>
    /// Validates and adds the selected fish to the selected aquarium. Returns validation result for the UI.
    /// </summary>
    public async Task<ValidationResult> TryAddFishToSelectedAquariumAsync()
    {
        if (SelectedAquarium is null)
        {
            return ValidationResult.Fail("Bitte zuerst ein Aquarium auswählen.");
        }

        if (SelectedFish is null)
        {
            return ValidationResult.Fail("Bitte einen Fisch auswählen.");
        }

        var fish = SelectedFish;

        if (SelectedAquarium.CapacityLiters < fish.MinimumTankSizeLiters)
        {
            return ValidationResult.Fail($"Das Aquarium ist zu klein. Mindestens {fish.MinimumTankSizeLiters} Liter erforderlich.");
        }

        var water = SelectedAquarium.WaterParameters;
        if (!fish.WaterParameters.Ph.IsWithin(water.Ph) ||
            !fish.WaterParameters.Gh.IsWithin(water.Gh) ||
            !fish.WaterParameters.Kh.IsWithin(water.Kh) ||
            !fish.WaterParameters.Temperature.IsWithin(water.Temperature))
        {
            return ValidationResult.Fail("Die Wasserwerte passen nicht zum Fisch.");
        }

        foreach (var existingEntry in SelectedAquarium.Fish)
        {
            var existing = _fishDatabaseService.GetByName(existingEntry.FishName);
            if (existing is null)
            {
                continue;
            }

            if (existing.IncompatibleFish.Contains(fish.Name) || fish.IncompatibleFish.Contains(existing.Name))
            {
                return ValidationResult.Fail($"{fish.Name} verträgt sich nicht mit {existing.Name}.");
            }

            if (existing.Temperament.Contains("Aggressiv", StringComparison.OrdinalIgnoreCase) &&
                fish.Temperament.Contains("Aggressiv", StringComparison.OrdinalIgnoreCase))
            {
                return ValidationResult.Fail("Zwei aggressive Arten sollten nicht zusammen gehalten werden.");
            }
        }

        SelectedAquarium.Fish.Add(new AquariumFishEntry { FishName = fish.Name });
        await _storageService.SaveAsync(_data);
        return ValidationResult.Ok();
    }
}
