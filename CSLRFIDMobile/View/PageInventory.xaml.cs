using TinyMvvm;

namespace CSLRFIDMobile.View
{
    public partial class PageInventory : TinyView
    {
        public PageInventory(ViewModelInventory viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}