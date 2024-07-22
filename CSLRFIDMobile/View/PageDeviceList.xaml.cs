using TinyMvvm;

namespace CSLRFIDMobile.View
{
	public partial class PageDeviceList : TinyView
    {
		public PageDeviceList(DeviceListViewModel viewModel)
		{
			InitializeComponent();
			BindingContext = viewModel;
		}
	}
}