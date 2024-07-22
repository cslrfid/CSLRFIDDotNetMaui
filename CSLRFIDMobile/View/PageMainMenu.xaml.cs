using TinyMvvm;

namespace CSLRFIDMobile.View
{
    public partial class PageMainMenu : TinyView
    {
        public PageMainMenu(ViewModelMainMenu viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }

}
