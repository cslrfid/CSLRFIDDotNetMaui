using CSLRFIDMobile.View;

namespace CSLRFIDMobile
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(PageDeviceList), typeof(PageDeviceList));
            Routing.RegisterRoute(nameof(PageInventory), typeof(PageInventory));
            Routing.RegisterRoute(nameof(PageGeigerSearch), typeof(PageGeigerSearch));
            Routing.RegisterRoute(nameof(PageGeigerSettings), typeof(PageGeigerSettings));
            Routing.RegisterRoute(nameof(PageAbout), typeof(PageAbout));
            Routing.RegisterRoute(nameof(PageSetting), typeof(PageSetting));
            Routing.RegisterRoute(nameof(PageSettingAdministration), typeof(PageSettingAdministration));
            Routing.RegisterRoute(nameof(PageSettingAntenna), typeof(PageSettingAntenna));
            Routing.RegisterRoute(nameof(PageSettingOperation), typeof(PageSettingOperation));
            Routing.RegisterRoute(nameof(PageSettingPowerSequencing), typeof(PageSettingPowerSequencing));
        }

        protected override void OnNavigated(ShellNavigatedEventArgs args)
        {
            base.OnNavigated(args);
            TitleBarText.Text = Current.CurrentPage.Title;
            TitleBarImage.IsVisible = TitleBarText.Text.ToLower().Contains("home") ? true : false;
        }
    }
}
