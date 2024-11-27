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

        protected override void OnAppearing()
        {
            base.OnAppearing();

            ViewModelInventory vm = (ViewModelInventory)BindingContext;
            if (vm.ClearRfidListView == null)
            {
                vm.ClearRfidListView = (() =>
                {
                    ListViewTagData.SelectedItem = null;
                });
            }
        }
    }
}