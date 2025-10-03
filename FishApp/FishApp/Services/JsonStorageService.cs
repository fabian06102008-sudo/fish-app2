using System;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using FishApp.Models;
using Microsoft.Maui.Storage;

namespace FishApp.Services;

/// <summary>
/// Persists aquarium data in a JSON file located inside the platforms app-data directory.
/// </summary>
public class JsonStorageService
{
    private const string FileName = "aquariums.json";
    private readonly SemaphoreSlim _mutex = new(1, 1);
    private readonly JsonSerializerOptions _serializerOptions = new()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true,
    };

    private string FilePath => Path.Combine(FileSystem.AppDataDirectory, FileName);

    /// <summary>
    /// Loads aquariums from disk or returns an empty container if the file does not exist.
    /// </summary>
    public async Task<AquariumData> LoadAsync()
    {
        await _mutex.WaitAsync();
        try
        {
            if (!File.Exists(FilePath))
            {
                return new AquariumData();
            }

            await using var stream = File.OpenRead(FilePath);
            var data = await JsonSerializer.DeserializeAsync<AquariumData>(stream, _serializerOptions);
            return data ?? new AquariumData();
        }
        finally
        {
            _mutex.Release();
        }
    }

    /// <summary>
    /// Saves the provided data set to disk.
    /// </summary>
    public async Task SaveAsync(AquariumData data)
    {
        if (data is null)
        {
            throw new ArgumentNullException(nameof(data));
        }

        await _mutex.WaitAsync();
        try
        {
            Directory.CreateDirectory(FileSystem.AppDataDirectory);
            await using var stream = File.Create(FilePath);
            await JsonSerializer.SerializeAsync(stream, data, _serializerOptions);
        }
        finally
        {
            _mutex.Release();
        }
    }
}
