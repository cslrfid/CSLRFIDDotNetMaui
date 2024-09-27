using TinyMvvm;


namespace CSLRFIDMobile.View
{
    public partial class PageGeigerSettings : TinyView
    {

        public PageGeigerSettings(ViewModelGeigerSettings viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

    }
}
