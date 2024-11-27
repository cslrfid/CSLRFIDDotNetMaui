using TinyMvvm;

namespace CSLRFIDMobile.View
{
    public partial class PageGeigerSearch : TinyView
    {
        public PageGeigerSearch(ViewModelGeigerSearch viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}