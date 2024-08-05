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
        }

        protected override void OnNavigated(ShellNavigatedEventArgs args)
        {
            base.OnNavigated(args);
            TitleBarText.Text = Current.CurrentPage.Title;
            TitleBarImage.IsVisible = TitleBarText.Text.ToLower().Contains("home") ? true : false;
        }
    }
}
