using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using CSLRFIDMobile.Services.Popups;
using CSLRFIDMobile.Services;
using CSLRFIDMobile.View;
using Plugin.BLE;
using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using Plugin.BLE.Abstractions.Extensions;
using TinyMvvm;
using static CSLibrary.RFIDDEVICE;

namespace CSLRFIDMobile.ViewModel
{
    public partial class DeviceListViewModel : BaseViewModel
    {
        private readonly IPopupService _popupService;
        private readonly CSLReaderService _cslReaderService;
        private readonly AppStateService _appStateService;


        private CancellationTokenSource? _cancellationTokenSource = new CancellationTokenSource();
        public ObservableCollection<DeviceListItemViewModel> Devices { get; set; } = new ObservableCollection<DeviceListItemViewModel>();

        public bool IsStateOn => _cslReaderService.bluetoothLe!.IsOn;

        public string StateText => GetStateText();

        public DeviceListItemViewModel? SelectedDevice
        {
            get { return null; }
            set
            {
                if (value != null)
                {
                    _ = HandleSelectedDevice(value);
                    OnPropertyChanged(nameof(SelectedDevice));
                }
            }
        }

        public string ScanLabelText
        {
            get
            {
                string result = IsScanning ? "Stop Scan" : "Start Scan";
                return result;
            }
        }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(ScanLabelText))]
        public bool isScanning;


        public DeviceListViewModel(CSLReaderService cslReaderService, IPopupService popupService, AppStateService appStateService)
        {
            _popupService = popupService;
            _cslReaderService = cslReaderService;
            _appStateService = appStateService;

            _cslReaderService.adapter.DeviceDisconnected += OnDeviceDisconnected!;

            try
            {
                _ = _cslReaderService.reader?.DisconnectAsync()!;
            }
            catch (Exception ex)
            {
                CSLibrary.Debug.WriteLine($"Device disconnect error: {ex.Message}");
            }

            PerformScanForDevices();

        }

        private string GetStateText()
        {
            try
            {
                switch (_cslReaderService.bluetoothLe?.State)
                {
                    case BluetoothState.Unavailable:
                        return "BLE is not available on this device.";
                    case BluetoothState.Unauthorized:
                        return "You are not allowed to use BLE.";
                    case BluetoothState.TurningOn:
                        return "BLE is warming up, please wait.";
                    case BluetoothState.On:
                        return "BLE is on.";
                    case BluetoothState.TurningOff:
                        return "BLE is turning off. That's sad!";
                    case BluetoothState.Off:
                        if (DeviceInfo.Current.Platform == DevicePlatform.iOS)
                            _ = _popupService.AlertAsync("Please put finger at bottom of screen and swipe up \"Control Center\" and turn on Bluetooth.  If Bluetooth is already on, turn it off and on again");
                        return "BLE is off. Turn it on!";
                }
            }
            catch (Exception ex)
            {
                _ = _popupService.AlertAsync(ex.Message, "GetState Error");
            }

            return "Unknown BLE state.";
        }

        private void Adapter_ScanTimeoutElapsed(object sender, EventArgs e)
        {
            CleanupCancellationToken();
            IsScanning = false;
        }

        private void OnDeviceDiscovered(object sender, DeviceEventArgs args)
        {
            try
            {
                bool CSLRFIDReaderService = false;
                MODEL BTServiceType = MODEL.UNKNOWN;

                // CS108 filter
                switch (DeviceInfo.Current.Platform.ToString())
                {
                    case "WinUI":
                    case "UWP": 

                        if (args.Device.AdvertisementRecords.Count < 1)
                            return;

                        foreach (AdvertisementRecord service in args.Device.AdvertisementRecords)
                        {
                            if (service.Data.Length == 2)
                            {
                                // CS108 Service ID = 0x0098
                                if (service.Data[0] == 0x00 && service.Data[1] == 0x98)
                                {
                                    BTServiceType = MODEL.CS108;
                                    CSLRFIDReaderService = true;
                                    break;
                                }

                                // CS710S Service ID = 0x0298
                                if ((service.Data[0] == 0x02 && service.Data[1] == 0x98))
                                {
                                    BTServiceType = MODEL.CS710S;
                                    CSLRFIDReaderService = true;
                                    break;
                                }
                            }
                        }
                        break;

                    default:
                        if (args.Device.AdvertisementRecords.Count < 1)
                            return;

                        foreach (AdvertisementRecord service in args.Device.AdvertisementRecords)
                        {
                            if (service.Data.Length == 2)
                            {
                                // CS108 Service ID = 0x9800
                                if (service.Data[0] == 0x98 && service.Data[1] == 0x00)
                                {
                                    BTServiceType = MODEL.CS108;
                                    CSLRFIDReaderService = true;
                                    break;
                                }

                                // CS710S Service ID ios = 0x9802, android = 0x5350
                                if ((service.Data[0] == 0x98 && service.Data[1] == 0x02) || (service.Data[0] == 0x53 && service.Data[1] == 0x50))
                                {
                                    BTServiceType = MODEL.CS710S;
                                    CSLRFIDReaderService = true;
                                    break;
                                }
                            }
                        }
                        break;
                }

                if (!CSLRFIDReaderService)
                    return;

                AddOrUpdateDevice(args.Device, BTServiceType);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Could handle discover device: {ex.Message}");
            }
        }

        private void AddOrUpdateDevice(IDevice device, MODEL BTServiceType)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                try
                {
                    var vm = Devices.FirstOrDefault(d => d.Device.Id == device.Id);
                    if (vm != null)
                    {
                        vm.Update(device);
                    }
                    else
                    {
                        Devices.Add(new DeviceListItemViewModel(device, BTServiceType));
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Could not add device {ex.Message}");
                }
            });
        }

        public override async Task OnAppearing()
        {
            await base.OnAppearing();

            // Clear previously linked BLE device Id to force fresh scanning/connection
            if (!string.IsNullOrEmpty(_appStateService.Settings.CSLLinkedDeviceId))
            {
                _appStateService.Settings.CSLLinkedDeviceId = string.Empty;
                await _appStateService.SaveConfig();
            }

            _cslReaderService.SetEvent(true);

        }

        public List<DeviceListItemViewModel> SystemDevices { get; private set; } = new List<DeviceListItemViewModel>();

        public override async Task OnDisappearing()
        {
            try
            {
                _cslReaderService.adapter.DeviceDisconnected -= OnDeviceDisconnected!;

                await _cslReaderService.adapter?.StopScanningForDevicesAsync()!;
            }
            catch (Exception ex)
            {
                CSLibrary.Debug.WriteLine($"Device Suspend error: {ex.Message}");
            }
        }

        private void ScanForDevices()
        {
            try
            {
                _cancellationTokenSource = new CancellationTokenSource();

                _cslReaderService.adapter.ScanMode = ScanMode.LowLatency;
                _ = _cslReaderService.adapter.StartScanningForDevicesAsync(_cancellationTokenSource.Token);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Could not Scan devices: {ex.Message}");
            }
        }

        private void CleanupCancellationToken()
        {
            try
            {
                _cancellationTokenSource?.Dispose();
                _cancellationTokenSource = null;

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Can not stop _cancellationTokenSource: {ex.Message}");
            }
        }

        [RelayCommand]
        private async Task Disconnect(DeviceListItemViewModel device)
        {
            if (_cslReaderService.reader?.Status != CSLibrary.HighLevelInterface.READERSTATE.DISCONNECT)
                _cslReaderService.reader?.DisconnectAsync();

            try
            {
                if (!device.IsConnected)
                    return;

                await _popupService.ShowLoadingAsync($"Disconnecting {device.Name}...");

                await _cslReaderService.adapter.DisconnectDeviceAsync(device.Device);
            }
            catch (Exception ex)
            {
                await _popupService.AlertAsync(ex.Message, "Disconnect error");
            }
            finally
            {
                device.Update();
            }
        }

        private async Task HandleSelectedDevice(DeviceListItemViewModel device)
        {
            try
            {
                if (!await _popupService.ConfirmAsync($"Connect to device '{device.Name}'?"))
                {
                    return;
                }

                await _popupService.ShowLoadingAsync($"Connecting to {device.Name}...");

                // cancel search
                _cancellationTokenSource?.Cancel();
                IsScanning = false;

                if (!await _cslReaderService.ConnectDeviceAsync(device))
                {
                    await _popupService.HideLoadingAsync();
                    SelectedDevice = null;
                    await _popupService.AlertAsync($"Unable to connect to device {device.Name}");
                }
                else
                {
                    await _popupService.HideLoadingAsync();
                    await Shell.Current.GoToAsync("..", true);
                }
            }
            catch (Exception ex)
            {
                SelectedDevice = null;
                await _popupService.AlertAsync(ex.Message, "Connection error");
            }

        }

        [RelayCommand]
        private async Task ConnectDispose(DeviceListItemViewModel item)
        {
            try
            {
                using (item.Device)
                {
                    await _cslReaderService.adapter.ConnectToDeviceAsync(item.Device);
                    item.Update();
                }
            }
            catch (Exception ex)
            {
                _ = _popupService.AlertAsync(ex.Message, "Failed to connect and dispose.");
            }
            finally
            {
                await _popupService.HideLoadingAsync();
            }
        }

        private async void OnDeviceDisconnected(object sender, DeviceEventArgs e)
        {
            Devices.FirstOrDefault(d => d.Id == e.Device.Id)?.Update();
            await _popupService.HideLoadingAsync();
            await _popupService.ShowToastAsync($"Disconnected {e.Device.Name}");
        }

        [RelayCommand]
        private void PerformScanForDevices()
        {            
            if (!IsScanning)
            {
                IsScanning = true;
                StartScanForDevices();
            }
            else
            {
                _cancellationTokenSource?.Cancel();
                IsScanning = false;
            }

        }

        private async void StartScanForDevices()
        {
   
            if (!await HasCorrectPermissions())
            {
                await _popupService.AlertAsync("Permissons fail - can't scan");
                return;
            }

            Devices.Clear();

            _cancellationTokenSource = new CancellationTokenSource();
            _cslReaderService.adapter.ScanMode = ScanMode.LowLatency;

            _cslReaderService.adapter.DeviceDiscovered -= OnDeviceDiscovered!;
            _cslReaderService.adapter.DeviceAdvertised -= OnDeviceDiscovered!;
            _cslReaderService.adapter.ScanTimeoutElapsed -= Adapter_ScanTimeoutElapsed!;

            _cslReaderService.adapter.DeviceDiscovered += OnDeviceDiscovered!;
            _cslReaderService.adapter.DeviceAdvertised += OnDeviceDiscovered!;
            _cslReaderService.adapter.ScanTimeoutElapsed += Adapter_ScanTimeoutElapsed!;
            _cslReaderService.adapter.ScanMode = ScanMode.LowLatency;
            
            await _cslReaderService.adapter.StartScanningForDevicesAsync(_cancellationTokenSource.Token);
        }

        private async Task UpdateConnectedDevices()
        {
            foreach (var connectedDevice in _cslReaderService.adapter.ConnectedDevices)
            {
                //update rssi for already connected devices (so tha 0 is not shown in the list)
                try
                {
                    await connectedDevice.UpdateRssiAsync();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Failed to update RSSI for {connectedDevice.Name}. Error: {ex.Message}");
                }
            }
        }

        private async Task<bool> HasCorrectPermissions()
        {
            var permissionResult = await Permissions.CheckStatusAsync<Permissions.Bluetooth>();
            if (permissionResult != PermissionStatus.Granted)
                permissionResult = await Permissions.RequestAsync<Permissions.Bluetooth>();
            if (permissionResult != PermissionStatus.Granted)
            {
                await _popupService.AlertAsync("Permission denied. Not scanning.");
                AppInfo.ShowSettingsUI();
                return false;
            }

            return true;
        }
    }
}
