using CSLRFIDMobile.Services;
using TinyMvvm;

namespace CSLRFIDMobile.View
{
    
    public partial class PageSetting : TinyView
	{
        public PageSetting(ViewModelSetting viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;

        }
    }
}
