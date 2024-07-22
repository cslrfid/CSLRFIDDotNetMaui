namespace CSLRFIDMobile.ViewModel;
using TinyMvvm;

public partial class BaseViewModel : TinyViewModel
{

    [ObservableProperty]
    string title = String.Empty;

}
