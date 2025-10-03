using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using FishApp.Models;

namespace FishApp.Services;

/// <summary>
/// Provides a small in-memory fish database used in multiple views.
/// </summary>
public class FishDatabaseService
{
    private readonly IReadOnlyList<Fish> _fish;

    public FishDatabaseService()
    {
        _fish = BuildFishList();
    }

    public ObservableCollection<Fish> GetAllFish()
    {
        return new ObservableCollection<Fish>(_fish);
    }

    public Fish? GetByName(string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return null;
        }

        return _fish.FirstOrDefault(f => f.Name.Equals(name, System.StringComparison.OrdinalIgnoreCase));
    }

    private static IReadOnlyList<Fish> BuildFishList() => new List<Fish>
    {
        new()
        {
            Name = "Molly",
            MinimumTankSizeLiters = 80,
            Origin = "Mittelamerika",
            Temperament = "Friedlich",
            WaterParameters = new WaterParameters
            {
                Ph = new WaterRange { Min = 7.0, Max = 8.2 },
                Gh = new WaterRange { Min = 10, Max = 30 },
                Kh = new WaterRange { Min = 8, Max = 15 },
                Temperature = new WaterRange { Min = 24, Max = 27 }
            }
        },
        new()
        {
            Name = "Betta (Kampffisch)",
            MinimumTankSizeLiters = 20,
            Origin = "Südostasien",
            Temperament = "Aggressiv gegenüber Artgenossen",
            IncompatibleFish = new List<string> { "Betta (Kampffisch)" },
            WaterParameters = new WaterParameters
            {
                Ph = new WaterRange { Min = 6.0, Max = 7.5 },
                Gh = new WaterRange { Min = 5, Max = 15 },
                Kh = new WaterRange { Min = 3, Max = 10 },
                Temperature = new WaterRange { Min = 24, Max = 30 }
            }
        },
        new()
        {
            Name = "Skalar",
            MinimumTankSizeLiters = 250,
            Origin = "Amazonas",
            Temperament = "Halb-aggressiv",
            WaterParameters = new WaterParameters
            {
                Ph = new WaterRange { Min = 6.0, Max = 7.5 },
                Gh = new WaterRange { Min = 5, Max = 15 },
                Kh = new WaterRange { Min = 3, Max = 8 },
                Temperature = new WaterRange { Min = 24, Max = 29 }
            }
        },
        new()
        {
            Name = "Oscar",
            MinimumTankSizeLiters = 450,
            Origin = "Südamerika",
            Temperament = "Aggressiv",
            IncompatibleFish = new List<string> { "Molly", "Betta (Kampffisch)", "Skalar" },
            WaterParameters = new WaterParameters
            {
                Ph = new WaterRange { Min = 6.0, Max = 7.5 },
                Gh = new WaterRange { Min = 5, Max = 20 },
                Kh = new WaterRange { Min = 3, Max = 10 },
                Temperature = new WaterRange { Min = 23, Max = 27 }
            }
        }
    };
}
