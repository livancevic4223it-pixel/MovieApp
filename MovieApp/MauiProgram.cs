using Microsoft.Extensions.Logging;
using MovieApp.Services;
using MovieApp.Views;
using Plugin.LocalNotification;

namespace MovieApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseLocalNotification()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton<DatabaseService>();
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<FavoritesPage>();
            builder.Services.AddTransient<TopListPage>();
            builder.Services.AddTransient<MovieDetailPage>();
            builder.Services.AddTransient<AddEditMoviePage>();
            builder.Services.AddTransient<ReminderPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}