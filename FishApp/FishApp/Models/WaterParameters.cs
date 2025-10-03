using System;

namespace FishApp.Models;

/// <summary>
/// Represents a simple water parameter range with minimum and maximum values.
/// </summary>
public class WaterRange
{
    public double Min { get; set; }
    public double Max { get; set; }

    public bool IsWithin(double value) => value >= Min && value <= Max;

    public override string ToString() => $"{Min:0.0} - {Max:0.0}";
}

/// <summary>
/// Defines the desired water parameters for a specific fish.
/// </summary>
public class WaterParameters
{
    public WaterRange Ph { get; set; } = new() { Min = 6.0, Max = 8.0 };
    public WaterRange Gh { get; set; } = new() { Min = 5, Max = 15 };
    public WaterRange Kh { get; set; } = new() { Min = 3, Max = 12 };
    public WaterRange Temperature { get; set; } = new() { Min = 22, Max = 28 };
}

/// <summary>
/// Stores water parameter values for an aquarium.
/// </summary>
public class AquariumWaterParameters
{
    public double Ph { get; set; }
    public double Gh { get; set; }
    public double Kh { get; set; }
    public double Temperature { get; set; }

    public override string ToString() =>
        $"pH {Ph:0.0}, GH {Gh:0.0}, KH {Kh:0.0}, Temp {Temperature:0.0}°C";
}
