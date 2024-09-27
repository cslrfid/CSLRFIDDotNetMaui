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
        private readonly CSLReaderService _cslReaderService;
        private readonly IUserDialogs _userDialogs;

        [ObservableProperty]
        public string connectedButton = String.Empty;
        [ObservableProperty]
        public string labelVoltage = String.Empty;
        [ObservableProperty]
        public bool isBatteryLevelVisible = false;
        [ObservableProperty]
        public string labelVoltageTextColor = "Black";        
        [ObservableProperty]
        public string labelAppVersion = String.Empty;

        public ViewModelMainMenu(CSLReaderService appStateService, IUserDialogs userDialogs)
        {
            _userDialogs = userDialogs;
            _cslReaderService = appStateService;

            LabelAppVersion = "v1.0";

        }

        private void _cslReaderService_BatteryLevelEvent(object? sender, CSLBatteryLevelEventArgs e)
        {
            LabelVoltage = e.BatteryValue;
            LabelVoltageTextColor = e.IsLowBattery ? "Red" : "Black";
            IsBatteryLevelVisible = true;
        }

        public override async Task OnAppearing()
        {
            _cslReaderService.reader?.rfid.StopOperation();
            _cslReaderService.reader?.barcode.Stop();

            _cslReaderService.adapter.DeviceConnectionLost += OnDeviceConnectionLost!;

            _cslReaderService.BatteryLevelEvent += _cslReaderService_BatteryLevelEvent;
            _cslReaderService.EnableBatteryEvent();

            await base.OnAppearing();

            CheckConnection();

            if (_cslReaderService.reader?.rfid.GetModel() != MODEL.UNKNOWN)
            {
                _cslReaderService.reader?.rfid.CancelAllSelectCriteria();
            }
            _cslReaderService.reader!.rfid.Options.TagRanging.focus = false;
            _cslReaderService.reader!.rfid.Options.TagRanging.fastid = false;

        }

        public override async Task OnDisappearing()
        {
            await base.OnDisappearing();

            _cslReaderService.adapter.DeviceConnectionLost -= OnDeviceConnectionLost!;
            _cslReaderService.BatteryLevelEvent -= _cslReaderService_BatteryLevelEvent;
            
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
            if (_cslReaderService.reader?.Status != CSLibrary.HighLevelInterface.READERSTATE.DISCONNECT)
            {
                ConnectedButton = "Connected to " + _cslReaderService.reader?.ReaderName + "\nPress to Select Another Reader";
                IsBatteryLevelVisible = true;
            }
            else
            {
                ConnectedButton = "Press to Scan & Connect to Reader";
                IsBatteryLevelVisible = false;
            }
        }
        [RelayCommand]
        async Task ConnectButton()
        {
            if (_cslReaderService.reader?.BLEBusy ?? false)
            {
                _userDialogs.ShowToast("Configuring Reader, Please Wait", null, TimeSpan.FromSeconds(1));
                return;
            }

            // for Geiger and Read/Write
            _cslReaderService._SELECT_EPC = "";
            _cslReaderService._SELECT_PC = 3000;

            LabelVoltage = "";

            await Shell.Current.GoToAsync(nameof(PageDeviceList), true);

            CheckConnection();
        }

        [RelayCommand]
        async Task InventoryButton()
        {
            if (_cslReaderService.reader?.BLEBusy ?? false)
            {
                _userDialogs.ShowToast("Configuring Reader, Please Wait", null, TimeSpan.FromSeconds(1));
                return;
            }
            else
            {
                if (_cslReaderService.reader?.Status == CSLibrary.HighLevelInterface.READERSTATE.DISCONNECT)
                {
                    ShowConnectionWarringMessage();
                    return;
                }

                await Shell.Current.GoToAsync(nameof(PageInventory), true);
            }
        }

        [RelayCommand]
        async Task GeigerButton()
        {
            if (_cslReaderService.reader?.BLEBusy ?? false)
            {
                _userDialogs.ShowToast("Configuring Reader, Please Wait", null, TimeSpan.FromSeconds(1));
                return;
            }
            else
            {
                if (_cslReaderService.reader?.Status == CSLibrary.HighLevelInterface.READERSTATE.DISCONNECT)
                {
                    ShowConnectionWarringMessage();
                    return;
                }

                await Shell.Current.GoToAsync(nameof(PageGeigerSearch), true);
            }
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
