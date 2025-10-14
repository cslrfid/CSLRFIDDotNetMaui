using CSLRFIDMobile.Services;
using CSLRFIDMobile.Services.Popups;
using CSLRFIDMobile.View;
using Microsoft.Extensions.Logging;
using TinyMvvm;
using CSLRFIDMobile.Helper;
using Plugin.Maui.Audio;
using SkiaSharp.Views.Maui.Controls.Hosting;
using epj.CircularGauge.Maui;
using Syncfusion.Maui.Toolkit.Hosting;




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
                .UseSkiaSharp()
                .UseCircularGauge()
                .ConfigureSyncfusionToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
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

            builder.AddAudio();

            // Register popup service
            builder.Services.AddSingleton<IPopupService, SyncfusionPopupService>();

            // Register app state service
            builder.Services.AddSingleton<AppStateService>();

            // Register CSL reader service
            builder.Services.AddSingleton<CSLReaderService>();

            // Register view models and pages
            builder.Services.AddSingleton<ViewModelMainMenu>();
            builder.Services.AddSingleton<PageMainMenu>();
            builder.Services.AddTransient<DeviceListViewModel>();
            builder.Services.AddTransient<PageDeviceList>();
            builder.Services.AddTransient<ViewModelInventory>();
            builder.Services.AddTransient<PageInventory>();
            builder.Services.AddTransient<ViewModelGeigerSearch>();
            builder.Services.AddTransient<PageGeigerSearch>();
            builder.Services.AddTransient<ViewModelGeigerSettings>();
            builder.Services.AddTransient<PageGeigerSettings>();
            builder.Services.AddTransient<ViewModelSetting>();
            builder.Services.AddTransient<PageAbout>();
            builder.Services.AddTransient<PageSetting>();
            builder.Services.AddTransient<PageTabbedSetting>();
            builder.Services.AddTransient<PageSettingAdministration>();
            builder.Services.AddTransient<PageSettingAntenna>();
            builder.Services.AddTransient<PageSettingOperation>();
            builder.Services.AddTransient<PageSettingPowerSequencing>();

            return builder.Build();
        }
    }
}
