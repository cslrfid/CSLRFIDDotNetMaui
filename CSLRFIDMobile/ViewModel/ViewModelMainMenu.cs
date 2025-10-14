using CSLRFIDMobile.Services;
using CSLRFIDMobile.Services.Popups;
using CSLRFIDMobile.View;
using Microsoft.Maui.Controls.Shapes;
using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.Contracts;
using static CSLibrary.RFIDDEVICE;

namespace CSLRFIDMobile.ViewModel
{

    public partial class ViewModelMainMenu : BaseViewModel
    {
        private readonly CSLReaderService _cslReaderService;
        private readonly IPopupService _popupService;
        private readonly AppStateService _appStateService;

        private IDispatcherTimer? _scanTimer;
        private bool _scanInProgress;

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

        public ViewModelMainMenu(CSLReaderService cslReaderService, IPopupService popupService, AppStateService appStateService)
        {
            _popupService = popupService;
            _cslReaderService = cslReaderService;
            _appStateService = appStateService;

            GetPermission();

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

            if (_cslReaderService.reader?.rfid.GetModel() != MODEL.UNKNOWN)
            {
                _cslReaderService.reader?.rfid.CancelAllSelectCriteria();
            }
            _cslReaderService.reader!.rfid.Options.TagRanging.focus = false;
            _cslReaderService.reader!.rfid.Options.TagRanging.fastid = false;

            await _appStateService!.LoadConfig();
            _cslReaderService.LinkedReaderSn = _appStateService?.Settings.CSLLinkedDevice ?? String.Empty;
            CheckConnection();

            // Start auto-reconnect timer if device is paired but disconnected
            TryStartScanTimer();

        }

        public override async Task OnDisappearing()
        {
            await base.OnDisappearing();

            _cslReaderService.adapter.DeviceConnectionLost -= OnDeviceConnectionLost!;
            _cslReaderService.BatteryLevelEvent -= _cslReaderService_BatteryLevelEvent;

            // Stop auto-reconnect timer when leaving page
            StopScanTimer();

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
                if (!String.IsNullOrEmpty(_appStateService.Settings.CSLLinkedDeviceId))
                {
                    ConnectedButton = "Waiting for Linked Reader...\nPress to Select Another Reader";
                }
                else
                    ConnectedButton = "Press to Scan & Connect to Reader";
                IsBatteryLevelVisible = false;
            }
        }
        [RelayCommand]
        async Task ConnectButton()
        {
            if (_cslReaderService.reader?.BLEBusy ?? false)
            {
                await _popupService.ShowToastAsync("Configuring Reader, Please Wait", null, TimeSpan.FromSeconds(1));
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
                await _popupService.ShowToastAsync("Configuring Reader, Please Wait", null, TimeSpan.FromSeconds(1));
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
                await _popupService.ShowToastAsync("Configuring Reader, Please Wait", null, TimeSpan.FromSeconds(1));
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

        [RelayCommand]
        async Task SettingButton()
        {
            if (_cslReaderService.reader?.BLEBusy ?? false)
            {
                await _popupService.ShowToastAsync("Configuring Reader, Please Wait", null, TimeSpan.FromSeconds(1));
                return;
            }
            else
            {
                if (_cslReaderService.reader?.Status == CSLibrary.HighLevelInterface.READERSTATE.DISCONNECT)
                {
                    ShowConnectionWarringMessage();
                    return;
                }

                await Shell.Current.GoToAsync(nameof(PageTabbedSetting), true);
            }
        }

        async void ShowConnectionWarringMessage()
        {
            string connectWarringMsg = "Reader NOT connected\n\nPlease connect to reader first!!!";

            await _popupService.ShowToastAsync(connectWarringMsg);
        }

        private void OnDeviceConnectionLost(object sender, Plugin.BLE.Abstractions.EventArgs.DeviceErrorEventArgs e)
        {
            TryStartScanTimer();
            CheckConnection();
        }

        /// <summary>
        /// Check preconditions for auto-reconnect timer
        /// </summary>
        private bool PreconditionsForTimer()
        {
            var id = _appStateService.Settings.CSLLinkedDeviceId;
            var sn = _appStateService.Settings.CSLLinkedDevice;
            return !string.IsNullOrWhiteSpace(id) && !string.IsNullOrWhiteSpace(sn);
        }

        /// <summary>
        /// Start the auto-reconnect timer
        /// </summary>
        private void TryStartScanTimer()
        {
            if (!PreconditionsForTimer())
            {
                StopScanTimer();
                return;
            }

            if (_scanTimer != null)
                return;

            _scanTimer = Application.Current!.Dispatcher.CreateTimer();
            _scanTimer.Interval = TimeSpan.FromSeconds(3);
            _scanTimer.Tick += async (s, e) => await OnScanTimerTickAsync();
            _scanTimer.Start();
        }

        /// <summary>
        /// Stop the auto-reconnect timer
        /// </summary>
        private void StopScanTimer()
        {
            if (_scanTimer == null)
                return;

            _scanTimer.Stop();
            _scanTimer = null;
        }

        /// <summary>
        /// Auto-reconnect timer tick handler
        /// </summary>
        private async Task OnScanTimerTickAsync()
        {
            CheckConnection();
            if (_scanInProgress)
                return;

            if (!PreconditionsForTimer())
            {
                StopScanTimer();
                return;
            }

            try
            {
                _scanInProgress = true;

                var found = await _cslReaderService.ScanLinkedDeviceOnceAsync(TimeSpan.FromSeconds(2));
                if (found == null)
                    return;

                if (!Guid.TryParse(_appStateService.Settings.CSLLinkedDeviceId, out var id))
                    return;

                await _popupService.ShowLoadingAsync("Connecting to Reader...");
                var ok = await _cslReaderService.ConnectDeviceByIdAsync(id);
                await _popupService.HideLoadingAsync();

                if (ok)
                {
                    await _popupService.ShowToastAsync("Connected to reader", duration: TimeSpan.FromSeconds(1));
                    CheckConnection();
                    StopScanTimer();
                }
            }
            catch (Exception ex)
            {
                await _popupService.HideLoadingAsync();
                CSLibrary.Debug.WriteLine($"Background reconnect error: {ex.Message}");
            }
            finally
            {
                _scanInProgress = false;
            }
        }

    }
}
