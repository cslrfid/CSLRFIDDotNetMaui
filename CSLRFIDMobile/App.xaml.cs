using TinyMvvm;

namespace CSLRFIDMobile
{
    public partial class App : TinyApplication
    {
        public App()
        {
            InitializeComponent();
            UserAppTheme = AppTheme.Light;
            MainPage = new AppShell();
        }

    }
}
