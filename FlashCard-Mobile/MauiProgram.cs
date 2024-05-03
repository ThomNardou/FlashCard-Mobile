using Microsoft.Extensions.Logging;
using MauiIcons.Fluent;
using MauiIcons.Material;
using MauiIcons.Cupertino;

namespace FlashCard_Mobile
{
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
                    fonts.AddFont("Kanit-Light.ttf", "Kanit");
                })
                .UseFluentMauiIcons()
                .UseMaterialMauiIcons()
                .UseCupertinoMauiIcons();

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
