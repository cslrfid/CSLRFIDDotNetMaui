using CSLibrary;
using static CSLibrary.RFIDDEVICE;
using Plugin.BLE.Abstractions.Contracts;
using CSLRFIDMobile.Model;
using Newtonsoft.Json;
using Plugin.BLE;
using Plugin.BLE.Abstractions;
using Controls.UserDialogs.Maui;


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

        private readonly IUserDialogs _userDialogs;
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

        private bool IsInitializationCompleted = false;

        public CSLReaderService(IUserDialogs userDialogs)
        {
            _userDialogs = userDialogs;

            bluetoothLe = CrossBluetoothLE.Current;
            adapter = bluetoothLe?.Adapter!;

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

                await Connect(dev, device.BTServiceType);
                DateTime timer = DateTime.Now;
                while ((DateTime.Now - timer).TotalSeconds < 10.00 )
                {
                    if (IsInitializationCompleted)
                        break;
                    await Task.Delay(100);
                }

                return IsInitializationCompleted;

            }
            catch (Exception ex)
            {
                await _userDialogs.AlertAsync(ex.Message, "Connection error");
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
                            _userDialogs.AlertAsync("BLE protocol error, Please reset reader");

                        }
                        break;

                    case CSLibrary.Constants.ReaderCallbackType.CONNECTION_LOST:
                        break;

                    default:
                        break;
                }

            });
        }

        void StateChangedEvent(object sender, CSLibrary.Events.OnStateChangedEventArgs e)
        {
            if (e.state == CSLibrary.Constants.RFState.INITIALIZATION_COMPLETE)
            {
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

    }

}
