using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace FishApp.Models;

/// <summary>
/// Stores the fishes assigned to the aquarium for serialization.
/// </summary>
public class AquariumFishEntry
{
    public string FishName { get; set; } = string.Empty;
}

/// <summary>
/// Represents a user created aquarium.
/// </summary>
public class Aquarium
{
    public string Name { get; set; } = string.Empty;
    public int CapacityLiters { get; set; }
    public AquariumWaterParameters WaterParameters { get; set; } = new();

    [JsonInclude]
    public ObservableCollection<AquariumFishEntry> Fish { get; set; } = new();

    [JsonConstructor]
    public Aquarium()
    {
    }

    public Aquarium(string name, int capacity, AquariumWaterParameters parameters)
    {
        Name = name;
        CapacityLiters = capacity;
        WaterParameters = parameters;
    }
}

/// <summary>
/// Container class to persist aquariums to disk.
/// </summary>
public class AquariumData
{
    public ObservableCollection<Aquarium> Aquariums { get; set; } = new();
}
