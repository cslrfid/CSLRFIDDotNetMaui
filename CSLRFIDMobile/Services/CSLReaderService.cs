using CSLibrary;
using static CSLibrary.RFIDDEVICE;
using Plugin.BLE.Abstractions.Contracts;
using CSLRFIDMobile.Model;
using Newtonsoft.Json;
using Plugin.BLE;
using Plugin.BLE.Abstractions;
using CSLRFIDMobile.Services.Popups;
using Plugin.BLE.Abstractions.Extensions;


namespace CSLRFIDMobile.Services
{
    public class CSLBatteryLevelEventArgs : EventArgs
    {
        public string BatteryValue { get; set; } = String.Empty;
        public bool IsLowBattery { get; set; } = false;

        public CSLBatteryLevelEventArgs(string batteryValue, bool isLowBattery)
        {
            BatteryValue = batteryValue;
            IsLowBattery = isLowBattery;
        }   
    }

    public class CSLReaderService
    {
        public HighLevelInterface? reader = new HighLevelInterface();
        public CONFIG? config;
        public event EventHandler<CSLBatteryLevelEventArgs>? BatteryLevelEvent;

        private readonly IPopupService _popupService;
        private readonly AppStateService _appStateService;
        public IBluetoothLE? bluetoothLe;
        public IAdapter adapter;

        // System
        public IDevice? deviceinfo;

        // for Geiger and Read/Write
        public string _SELECT_EPC = "";
        public string _SELECT_TID = "";
        public UInt16 _SELECT_PC = 0x0000;

        public int _inventoryEntryPoint = 0;
        public bool _settingPage1TagPopulationChanged = false;
        public bool _settingPage3QvalueChanged = false;
        public bool _settingPage4QvalueChanged = false;

        // for battery level display
        public bool _batteryLow = false;
        public string _labelVoltage = String.Empty;

        // Linked reader serial number for auto-reconnect validation
        public string LinkedReaderSn { get; set; } = String.Empty;

        private bool IsInitializationCompleted = false;

        public CSLReaderService(IPopupService popupService, AppStateService appStateService)
        {
            _popupService = popupService;
            _appStateService = appStateService;

            bluetoothLe = CrossBluetoothLE.Current;
            adapter = bluetoothLe?.Adapter!;

            // Preload previously linked serial number if any
            LinkedReaderSn = _appStateService.Settings.CSLLinkedDevice ?? string.Empty;

            SetEvent(true);
        }

        public async Task<bool> ConnectDeviceAsync(DeviceListItemViewModel device)
        {
            try
            {
                CancellationTokenSource tokenSource = new CancellationTokenSource();

                await adapter.ConnectToDeviceAsync(device.Device, new ConnectParameters(autoConnect: false, forceBleTransport: true), tokenSource.Token);

                // Connect to CS108
                var dev = adapter.ConnectedDevices.FirstOrDefault(d => d.Id.Equals(device.Device.Id));

                if (dev == null)
                    return false;

                //wait unit initialization completed otherwise will be timed out
                IsInitializationCompleted = false;

                // Ensure linked SN preference is loaded
                if (string.IsNullOrEmpty(LinkedReaderSn))
                    LinkedReaderSn = _appStateService.Settings.CSLLinkedDevice ?? string.Empty;

                if (config == null)
                {
                    await LoadConfig(LinkedReaderSn, device.BTServiceType);
                }

                await Connect(dev, device.BTServiceType);
                DateTime timer = DateTime.Now;
                while ((DateTime.Now - timer).TotalSeconds < 20.00 )
                {
                    if (IsInitializationCompleted)
                        break;
                    await Task.Delay(100);
                }

                return IsInitializationCompleted;

            }
            catch (Exception ex)
            {
                await _popupService.AlertAsync(ex.Message, "Connection error");
                CSLibrary.Debug.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                device.Update();
            }
        }

        private async Task Connect(IDevice _device, MODEL deviceType)
        {
            deviceinfo = _device;

            await reader?.ConnectAsync(adapter, _device, deviceType)!;

            CSLibrary.Debug.WriteLine("load config");

            bool LoadSuccess = await LoadConfig(_device.Id.ToString(), deviceType);
            config!.readerID = _device.Id.ToString();
        }      

        public void SetEvent(bool onoff)
        {
            reader?.CancelEventOnReaderStateChanged();
            reader?.notification.ClearEventHandler(); // Key Button event handler
            reader?.rfid.ClearEventHandler(); // Cancel RFID event handler
            reader?.barcode.ClearEventHandler(); // Cancel Barcode event handler

            if (onoff)
            {
                reader!.OnReaderStateChanged += new EventHandler<CSLibrary.Events.OnReaderStateChangedEventArgs>(ReaderStateCChangedEvent!);
                reader!.rfid.OnStateChanged += new EventHandler<CSLibrary.Events.OnStateChangedEventArgs>(StateChangedEvent!);
            }
        }

        public void EnableBatteryEvent()
        {
            reader!.notification.OnVoltageEvent += new EventHandler<CSLibrary.Notification.VoltageEventArgs>(VoltageEvent!);
        }

        void ReaderStateCChangedEvent(object sender, CSLibrary.Events.OnReaderStateChangedEventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                CSLibrary.Debug.WriteLine(e.type.ToString());

                switch (e.type)
                {
                    case CSLibrary.Constants.ReaderCallbackType.COMMUNICATION_ERROR:
                        {
                            _ = _popupService.AlertAsync("BLE protocol error, Please reset reader");

                        }
                        break;

                    case CSLibrary.Constants.ReaderCallbackType.CONNECTION_LOST:
                        break;

                    default:
                        break;
                }

            });
        }

        async void StateChangedEvent(object sender, CSLibrary.Events.OnStateChangedEventArgs e)
        {
            if (e.state == CSLibrary.Constants.RFState.INITIALIZATION_COMPLETE)
            {
                // Get device serial number for validation
                string deviceType = reader?.rfid.GetModelName() ?? String.Empty;
                string deviceSn;
                if (deviceType == "CS108")
                    deviceSn = reader?.siliconlabIC.GetSerialNumberSync().Substring(0, 13) ?? String.Empty;
                else if (deviceType == "CS710S")
                    deviceSn = reader?.siliconlabIC.GetSerialNumberSync().Substring(0, 16) ?? String.Empty;
                else
                    deviceSn = String.Empty;


                // Cache and persist linked reader info
                LinkedReaderSn = deviceSn;
                _appStateService.Settings.CSLLinkedDevice = LinkedReaderSn;
                _appStateService.Settings.CSLLinkedDeviceId = deviceinfo?.Id.ToString() ?? string.Empty;
                await _appStateService.SaveConfig();


                if (reader!.rfid.GetModelName() == "CS710S-1" && config!.RFID_Profile == 244)
                    config.RFID_Profile = 241;

                // System Setting
                _batteryLow = false;

                // Set Country and Region information
                if (config?.RFID_Region == "" || config?.readerModel != reader.rfid.GetModel())
                {
                    config!.readerModel = reader.rfid.GetModel();
                    config.RFID_Region = reader.rfid.GetCurrentCountry();

                    if (reader.rfid.IsFixedChannel())
                    {
                        config.RFID_FrequenceSwitch = 1;
                        config.RFID_FixedChannel = reader.rfid.GetCurrentFrequencyChannel();
                    }
                    else
                    {
                        config.RFID_FrequenceSwitch = 0; // Hopping
                    }
                }

                uint portNum = reader.rfid.GetAntennaPort();
                for (uint cnt = 0; cnt < portNum; cnt++)
                {
                    reader.rfid.SetAntennaPortState(cnt, config.RFID_AntennaEnable[cnt] ? CSLibrary.Constants.AntennaPortState.ENABLED : CSLibrary.Constants.AntennaPortState.DISABLED);
                    reader.rfid.SetPowerLevel(config.RFID_Antenna_Power[cnt], cnt);
                    reader.rfid.SetInventoryDuration(config.RFID_Antenna_Dwell[cnt], cnt);
                }

                if ((reader.bluetoothIC.GetFirmwareVersion() & 0x0F0000) != 0x030000) // ignore CS463
                    ClassBattery.SetBatteryMode(ClassBattery.BATTERYMODE.IDLE);
                reader.battery.SetPollingTime(config.RFID_BatteryPollingTime);

                IsInitializationCompleted = true;
            }
        }

        void VoltageEvent(object sender, CSLibrary.Notification.VoltageEventArgs e)
        {
            try
            {
                if (e.Voltage == 0xffff)
                {
                    _labelVoltage = String.Empty;
                }
                else
                {
                    double voltage = (double)e.Voltage / 1000;

                    var batlow = ClassBattery.BatteryLow(voltage);

                    if (_batteryLow && batlow == ClassBattery.BATTERYLEVELSTATUS.NORMAL)
                    {
                        _batteryLow = false;
                    }
                    else if (!_batteryLow && batlow != ClassBattery.BATTERYLEVELSTATUS.NORMAL)
                    {
                        _batteryLow = true;
                    }

                    switch (config?.BatteryLevelIndicatorFormat)
                    {
                        case 0:
                            _labelVoltage = voltage.ToString("0.000") + "v";
                            break;

                        default:
                            _labelVoltage = ClassBattery.Voltage2Percent(voltage).ToString("0") + "%";
                            break;
                    }

                    BatteryLevelEvent?.Invoke(this, new CSLBatteryLevelEventArgs(_labelVoltage, _batteryLow));
                }
            }
            catch (Exception ex)  
            {
                CSLibrary.Debug.WriteLine($"Error reading battery level reporting: {ex}");
                BatteryLevelEvent?.Invoke(this, new CSLBatteryLevelEventArgs(String.Empty, false));
            }
        }

        public async Task<bool> LoadConfig(string readerID, MODEL model)
        {
            try
            {
                string contentJSON = String.Empty;
                string configFile = Path.Combine(FileSystem.Current.AppDataDirectory, readerID + ".cfg");
                if (File.Exists(configFile))
                {
                    using (StreamReader reader = new StreamReader(configFile))
                    {
                        contentJSON = await reader.ReadToEndAsync();
                        reader.Close();
                    }

                }

                var setting = String.IsNullOrEmpty(contentJSON) ? null : JsonConvert.DeserializeObject<CONFIG>(contentJSON);

                if (setting != null)
                {
                    config = setting;
                }
                else
                {
                    config = new CONFIG(model);
                }
            }
            catch (Exception ex)
            {
                CSLibrary.Debug.WriteLine($"Error loading reader config: {ex}");
                return false;
            }

            return true;
        }

        public async Task<bool> SaveConfig()
        {
            try
            {
                string configFile = Path.Combine(FileSystem.Current.AppDataDirectory, config?.readerID + ".cfg");

                string contentJSON = JsonConvert.SerializeObject(config);
                using (StreamWriter writer = new StreamWriter(configFile))
                {
                    await writer.WriteAsync(contentJSON);
                    writer.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                CSLibrary.Debug.WriteLine($"Error saving reader config: {ex}");
                return false;
            }
        }

        public void ResetConfig(uint port = 1)
        {
            var readerID = config!.readerID;
            var readerModel = config!.readerModel;
            var country = config!.country;
            config = new CONFIG(config!.readerModel);
            config.readerID = readerID;
            config.readerModel = readerModel;
            config.country = country;
        }

        /// <summary>
        /// Connect to a previously known device by Id and auto-detect reader model
        /// </summary>
        public async Task<bool> ConnectKnownDeviceAsync(Guid deviceId)
        {
            try
            {
                CancellationTokenSource tokenSource = new CancellationTokenSource();

                var dev = await adapter.ConnectToKnownDeviceAsync(deviceId, new ConnectParameters(autoConnect: false, forceBleTransport: true), tokenSource.Token);

                if (dev == null)
                    return false;

                // Detect model by available service
                MODEL model = MODEL.UNKNOWN;
                try
                {
                    var svc710 = await dev.GetServiceAsync(Guid.Parse("00009802-0000-1000-8000-00805f9b34fb"));
                    if (svc710 != null)
                        model = MODEL.CS710S;
                    else
                    {
                        var svc108 = await dev.GetServiceAsync(Guid.Parse("00009800-0000-1000-8000-00805f9b34fb"));
                        if (svc108 != null)
                            model = MODEL.CS108;
                    }
                }
                catch { }

                if (model == MODEL.UNKNOWN)
                    return false;

                //wait until initialization completed otherwise will be timed out
                IsInitializationCompleted = false;

                // Ensure linked SN preference is loaded
                if (string.IsNullOrEmpty(LinkedReaderSn))
                    LinkedReaderSn = _appStateService.Settings.CSLLinkedDevice ?? string.Empty;

                await Connect(dev, model);
                DateTime timer = DateTime.Now;
                while ((DateTime.Now - timer).TotalSeconds < 10.00)
                {
                    if (IsInitializationCompleted)
                        break;
                    await Task.Delay(100);
                }

                return IsInitializationCompleted;
            }
            catch (Exception ex)
            {
                CSLibrary.Debug.WriteLine(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Try auto-reconnecting to the previously linked device id in settings
        /// </summary>
        public async Task<bool> TryAutoReconnectAsync()
        {
            try
            {
                string idStr = _appStateService.Settings.CSLLinkedDeviceId;
                if (string.IsNullOrWhiteSpace(idStr))
                    return false;
                if (!Guid.TryParse(idStr, out var id))
                    return false;

                return await ConnectKnownDeviceAsync(id);
            }
            catch (Exception ex)
            {
                CSLibrary.Debug.WriteLine($"Auto-reconnect failed: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Scan for a specific device id and return it if discovered within timeout
        /// </summary>
        public async Task<IDevice?> FindDeviceByIdAsync(Guid deviceId, TimeSpan timeout, CancellationToken cancellationToken = default)
        {
            var tcs = new TaskCompletionSource<IDevice?>();
            EventHandler<Plugin.BLE.Abstractions.EventArgs.DeviceEventArgs>? handler = null;
            EventHandler<Plugin.BLE.Abstractions.EventArgs.DeviceEventArgs>? handlerAdv = null;
            using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            handler = (s, e) =>
            {
                if (e.Device.Id == deviceId)
                {
                    tcs.TrySetResult(e.Device);
                }
            };
            handlerAdv = (s, e) =>
            {
                if (e.Device.Id == deviceId)
                {
                    tcs.TrySetResult(e.Device);
                }
            };
            adapter.DeviceDiscovered += handler;
            adapter.DeviceAdvertised += handlerAdv;

            try
            {
                cts.CancelAfter(timeout);

                _ = adapter.StartScanningForDevicesAsync(cts.Token);

                using (cts.Token.Register(() => tcs.TrySetResult(null)))
                {
                    var found = await tcs.Task.ConfigureAwait(false);
                    return found;
                }
            }
            catch (Exception ex)
            {
                CSLibrary.Debug.WriteLine($"FindDeviceByIdAsync error: {ex.Message}");
                return null;
            }
            finally
            {
                adapter.DeviceDiscovered -= handler;
                adapter.DeviceAdvertised -= handlerAdv;
                try { await adapter.StopScanningForDevicesAsync(); } catch { }
            }
        }

        /// <summary>
        /// Scan once for the linked device id stored in settings
        /// </summary>
        public async Task<IDevice?> ScanLinkedDeviceOnceAsync(TimeSpan? scanTimeout = null, CancellationToken cancellationToken = default)
        {
            try
            {
                string idStr = _appStateService.Settings.CSLLinkedDeviceId;
                if (string.IsNullOrWhiteSpace(idStr))
                    return null;
                if (!Guid.TryParse(idStr, out var id))
                    return null;

                return await FindDeviceByIdAsync(id, scanTimeout ?? TimeSpan.FromSeconds(2), cancellationToken);
            }
            catch (Exception ex)
            {
                CSLibrary.Debug.WriteLine($"ScanLinkedDeviceOnceAsync error: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Connect to a device by id (wrapper for ConnectKnownDeviceAsync)
        /// </summary>
        public Task<bool> ConnectDeviceByIdAsync(Guid deviceId)
        {
            return ConnectKnownDeviceAsync(deviceId);
        }

        /// <summary>
        /// Reconnect only if the previously linked device is currently advertising/present
        /// </summary>
        public async Task<bool> TryReconnectIfPresentAsync(TimeSpan? scanTimeout = null)
        {
            try
            {
                var found = await ScanLinkedDeviceOnceAsync(scanTimeout);
                if (found == null)
                    return false; // not present, skip reconnect

                if (!Guid.TryParse(_appStateService.Settings.CSLLinkedDeviceId, out var id))
                    return false;

                return await ConnectDeviceByIdAsync(id);
            }
            catch (Exception ex)
            {
                CSLibrary.Debug.WriteLine($"TryReconnectIfPresentAsync error: {ex.Message}");
                return false;
            }
        }

    }

}
