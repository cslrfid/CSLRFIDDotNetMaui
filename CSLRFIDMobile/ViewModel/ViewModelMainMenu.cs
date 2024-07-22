using Controls.UserDialogs.Maui;
using Plugin.BLE.Abstractions.Contracts;
using CSLRFIDMobile.Services;
using Plugin.BLE.Abstractions;
using CSLRFIDMobile.View;
using static CSLibrary.RFIDDEVICE;

namespace CSLRFIDMobile.ViewModel
{

    public partial class ViewModelMainMenu : BaseViewModel
    {
        private readonly CSLReaderService _appStateService;
        private readonly IUserDialogs _userDialogs;

        [ObservableProperty]
        public string connectedButton = String.Empty;
        [ObservableProperty]
        public string labelVoltage = String.Empty;

        public string labelVoltageTextColor = "Black";
        public string LabelVoltageTextColor
        {
            get => labelVoltageTextColor;
            set => SetProperty(ref labelVoltageTextColor, _appStateService._batteryLow ? "Red" : "Black");
        }

        [ObservableProperty]
        public string connectedButtonTextColor = "Black";
        [ObservableProperty]
        public string labelAppVersion = String.Empty;

        public ViewModelMainMenu(CSLReaderService appStateService, IUserDialogs userDialogs)
        {
            _userDialogs = userDialogs;
            _appStateService = appStateService;

            LabelAppVersion = "Version\n" + "1.0";

            _appStateService.adapter.DeviceConnectionLost += OnDeviceConnectionLost!;

            //GetPermission();
        }

        ~ViewModelMainMenu()
        {
            //SetEvent(false);
        }

        public override async Task OnAppearing()
        {
            _appStateService.reader?.rfid.StopOperation();
            _appStateService.reader?.barcode.Stop();

            await base.OnAppearing();

            //SetEvent(true);

            CheckConnection();

            if (_appStateService.reader?.rfid.GetModel() != MODEL.UNKNOWN)
            {
                _appStateService.reader?.rfid.CancelAllSelectCriteria();
            }
            _appStateService.reader!.rfid.Options.TagRanging.focus = false;
            _appStateService.reader!.rfid.Options.TagRanging.fastid = false;

        }

        public override async Task OnDisappearing()
        {
            await base.OnDisappearing();
        }

        // MUST be geant location permission
        private async void GetPermission()
        {
            if (DeviceInfo.Current.Platform == DevicePlatform.Android)
            {
                while (await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>() != PermissionStatus.Granted)
                {                 
                    await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                }
            }
        }

        private void CheckConnection()
        {
            if (_appStateService.reader?.Status != CSLibrary.HighLevelInterface.READERSTATE.DISCONNECT)
            {
                ConnectedButton = "Connected to " + _appStateService.reader?.ReaderName + "\nPress to Select Another Reader";
                ConnectedButtonTextColor = "White";
            }
            else
            {
                ConnectedButton = "Press to Scan & Connect to Reader";
                ConnectedButtonTextColor = "Red";
            }
        }
        [RelayCommand]
        async Task ConnectButton()
        {
            if (_appStateService.reader?.BLEBusy ?? false)
            {
                _userDialogs.ShowToast("Configuring Reader, Please Wait", null, TimeSpan.FromSeconds(1));
                return;
            }

            // for Geiger and Read/Write
            _appStateService._SELECT_EPC = "";
            _appStateService._SELECT_PC = 3000;

            LabelVoltage = "";

            await Shell.Current.GoToAsync(nameof(PageDeviceList), true);

            CheckConnection();
        }

        void ShowConnectionWarringMessage()
        {
            string connectWarringMsg = "Reader NOT connected\n\nPlease connect to reader first!!!";

            _userDialogs.ShowSnackbar(connectWarringMsg);
        }

        private void OnDeviceConnectionLost(object sender, Plugin.BLE.Abstractions.EventArgs.DeviceErrorEventArgs e)
        {
            CheckConnection();
        }

    }
}
