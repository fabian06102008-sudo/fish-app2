using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace FishApp.Models;

/// <summary>
/// Represents a fish within the in-app knowledge base.
/// </summary>
public class Fish
{
    public string Name { get; set; } = string.Empty;
    public int MinimumTankSizeLiters { get; set; }
    public WaterParameters WaterParameters { get; set; } = new();
    public string Origin { get; set; } = string.Empty;
    public string Temperament { get; set; } = string.Empty;
    public List<string> IncompatibleFish { get; set; } = new();

    public override string ToString() => Name;

    [JsonIgnore]
    public string IncompatibleDescription => IncompatibleFish.Any()
        ? string.Join(", ", IncompatibleFish)
        : "Keine";
}
