using FishApp.Helpers;
using FishApp.Services;
using FishApp.ViewModels;
using FishApp.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FishApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Services.AddSingleton<JsonStorageService>();
        builder.Services.AddSingleton<FishDatabaseService>();

        builder.Services.AddSingleton<AquariumsViewModel>();
        builder.Services.AddSingleton<FishDatabaseViewModel>();
        builder.Services.AddSingleton<SearchViewModel>();

        builder.Services.AddTransient<AquariumsPage>();
        builder.Services.AddTransient<FishDatabasePage>();
        builder.Services.AddTransient<SearchPage>();
        builder.Services.AddTransient<FishDetailPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        var app = builder.Build();
        ServiceHelper.Initialize(app.Services);
        return app;
    }
}
