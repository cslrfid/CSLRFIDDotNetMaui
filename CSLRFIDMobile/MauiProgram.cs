using Controls.UserDialogs.Maui;
using CSLRFIDMobile.Services;
using CSLRFIDMobile.View;
using Microsoft.Extensions.Logging;
using TinyMvvm;
using CSLRFIDMobile.Helper;

#if ANDROID
using CSLRFIDMobile.Platforms.Android;
#endif
#if IOS
using CSLRFIDMobile.Platforms.iOS;
#endif


namespace CSLRFIDMobile
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseUserDialogs(true, () =>
                {
                    //setup your default styles for dialogs
                    AlertConfig.DefaultBackgroundColor = Colors.White;
#if ANDROID
        AlertConfig.DefaultMessageFontFamily = "OpenSans-Regular.ttf";
#else
                    AlertConfig.DefaultMessageFontFamily = "OpenSans-Regular";
#endif

                    ToastConfig.DefaultCornerRadius = 15;
                })
                .ConfigureMauiHandlers(handlers => {
#if ANDROID
                handlers.AddHandler<CustomViewCell, CustomViewCellHandler>();
#endif
#if IOS
                    handlers.AddHandler<CustomViewCell, CustomViewCellHandler>();
#endif
                })
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            builder.UseTinyMvvm();
#if DEBUG
            builder.Logging.AddDebug();
#endif

            builder.Services.AddSingleton<CSLReaderService>();
            builder.Services.AddSingleton<ViewModelMainMenu>();
            builder.Services.AddSingleton<PageMainMenu>();
            builder.Services.AddTransient<DeviceListViewModel>();
            builder.Services.AddTransient<PageDeviceList>();
            builder.Services.AddTransient<ViewModelInventory>();
            builder.Services.AddTransient<PageInventory>();

            return builder.Build();
        }
    }
}
