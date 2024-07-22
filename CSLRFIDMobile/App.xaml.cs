using TinyMvvm;

namespace CSLRFIDMobile
{
    public partial class App : TinyApplication
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }

    }
}
