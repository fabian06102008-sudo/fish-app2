using System;
using Microsoft.Extensions.DependencyInjection;

namespace FishApp.Helpers;

/// <summary>
/// Provides helper methods to resolve registered services from anywhere in the application.
/// </summary>
public static class ServiceHelper
{
    private static IServiceProvider? _services;

    public static void Initialize(IServiceProvider services)
    {
        _services = services;
    }

    public static T GetService<T>() where T : notnull
    {
        if (_services is null)
        {
            throw new InvalidOperationException("Service provider has not been initialized.");
        }

        return _services.GetRequiredService<T>();
    }
}
