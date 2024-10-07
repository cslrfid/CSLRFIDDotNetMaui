
using CSLRFIDMobile.Model;
using CSLRFIDMobile.Services;

namespace CSLRFIDMobile.View
{
	public partial class PageSettingAdministration : ContentPage
	{
        readonly string [] _ShareDataFormatOptions = new string[] { "JSON", "CSV", "Excel CSV" };

        private readonly CSLReaderService _cslReaderService;

        public PageSettingAdministration(CSLReaderService cslReaderService)
        {
            InitializeComponent();

            _cslReaderService = cslReaderService;


            if (DeviceInfo.Current.Platform == DevicePlatform.iOS)
            {
                this.IconImageSource = ImageSource.FromFile("icons8-Settings-50-2-30x30.png");
            }

            switch (_cslReaderService.config!.BatteryLevelIndicatorFormat)
            {
                case 0:
                    buttonBatteryLevelFormat.Text = "Voltage";
                    break;

                default:
                    buttonBatteryLevelFormat.Text = "Percentage";
                    break;
            }

            switchInventoryAlertSound.IsToggled = _cslReaderService.config!.RFID_InventoryAlertSound;

            F1.Text = _cslReaderService.config!.RFID_Shortcut[0].Function.ToString();
            F1MinTime.Text = _cslReaderService.config!.RFID_Shortcut[0].DurationMin.ToString();
            F1MaxTime.Text = _cslReaderService.config!.RFID_Shortcut[0].DurationMax.ToString();
            F2.Text = _cslReaderService.config!.RFID_Shortcut[1].Function.ToString();
            F2MinTime.Text = _cslReaderService.config!.RFID_Shortcut[1].DurationMin.ToString();
            F2MaxTime.Text = _cslReaderService.config!.RFID_Shortcut[1].DurationMax.ToString();

            //entryTagDelay.Text = BleMvxApplication._config.RFID_TagDelayTime.ToString();
            //entryInventoryDuration.Text = BleMvxApplication._config.RFID_InventoryDuration.ToString();

            entryReaderName.Text = _cslReaderService.reader!.ReaderName;

            labelReaderModel.Text = "Reader Model : " + _cslReaderService.reader!.rfid.GetFullModelName();

            switchNewTagLocation.IsToggled = _cslReaderService.config!.RFID_NewTagLocation;
            buttonShareDataFormat.Text = _ShareDataFormatOptions[_cslReaderService.config!.RFID_ShareFormat];

            switchRSSIDBm.IsToggled = _cslReaderService.config!.RFID_DBm;
            //switchSavetoFile.IsToggled = BleMvxApplication._config.RFID_SavetoFile;
            switchSavetoCloud.IsToggled = _cslReaderService.config!.RFID_SavetoCloud;
            switchhttpProtocol.IsToggled = (_cslReaderService.config!.RFID_CloudProtocol == 0) ? false : true;
            entryServerIP.Text = _cslReaderService.config!.RFID_IPAddress;

            switchVibration.IsToggled = _cslReaderService.config!.RFID_Vibration;
            //switchVibrationTag.IsToggled = BleMvxApplication._config.RFID_VibrationTag;
            entryVibrationWindow.Text = _cslReaderService.config!.RFID_VibrationWindow.ToString();
            entryVibrationTime.Text = _cslReaderService.config!.RFID_VibrationTime.ToString();

            switchKeepScreenOn.IsToggled = _cslReaderService.config!._keepScreenOn;

            //entryBatteryIntervalTime.Text = BleMvxApplication._config.RFID_BatteryPollingTime.ToString();

            entryAuthServerURL.Text = _cslReaderService.config!.Impinj_AuthenticateServerURL;
            entryVerificationemail.Text = _cslReaderService.config!.Impinj_AuthenticateEmail;
            entryVerificationpassword.Text = _cslReaderService.config!.Impinj_AuthenticatePassword;

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        public async void btnOKClicked(object sender, EventArgs e)
        {
            int cnt;

            switch (buttonBatteryLevelFormat.Text)
            {
                case "Voltage":
                    _cslReaderService.config!.BatteryLevelIndicatorFormat = 0;
                    break;

                default:
                    _cslReaderService.config!.BatteryLevelIndicatorFormat = 1;
                    break;
            }

            _cslReaderService.config!.RFID_InventoryAlertSound = switchInventoryAlertSound.IsToggled;

            _cslReaderService.config!.RFID_Shortcut[0].Function = (CONFIG.MAINMENUSHORTCUT.FUNCTION)Enum.Parse(typeof(CONFIG.MAINMENUSHORTCUT.FUNCTION), F1.Text);
            _cslReaderService.config!.RFID_Shortcut[0].DurationMin = uint.Parse(F1MinTime.Text);
            _cslReaderService.config!.RFID_Shortcut[0].DurationMax = uint.Parse(F1MaxTime.Text);
            _cslReaderService.config!.RFID_Shortcut[1].Function = (CONFIG.MAINMENUSHORTCUT.FUNCTION)Enum.Parse(typeof(CONFIG.MAINMENUSHORTCUT.FUNCTION), F2.Text);
            _cslReaderService.config!.RFID_Shortcut[1].DurationMin = uint.Parse(F2MinTime.Text);
            _cslReaderService.config!.RFID_Shortcut[1].DurationMax = uint.Parse(F2MaxTime.Text);

            _cslReaderService.config!.RFID_DBm = switchRSSIDBm.IsToggled;
            _cslReaderService.config!.RFID_SavetoCloud = switchSavetoCloud.IsToggled;
            _cslReaderService.config!.RFID_CloudProtocol = switchhttpProtocol.IsToggled ? 1 : 0;
            _cslReaderService.config!.RFID_IPAddress = entryServerIP.Text;

            _cslReaderService.config!.RFID_NewTagLocation = switchNewTagLocation.IsToggled;
            _cslReaderService.config!.RFID_ShareFormat = Array.IndexOf(_ShareDataFormatOptions, buttonShareDataFormat.Text);

            _cslReaderService.config!.RFID_Vibration = switchVibration.IsToggled;
            _cslReaderService.config!.RFID_VibrationWindow = UInt32.Parse(entryVibrationWindow.Text);
            _cslReaderService.config!.RFID_VibrationTime = UInt32.Parse(entryVibrationTime.Text);


            _cslReaderService.config!.Impinj_AuthenticateServerURL = entryAuthServerURL.Text;
            _cslReaderService.config!.Impinj_AuthenticateEmail = entryVerificationemail.Text;
            _cslReaderService.config!.Impinj_AuthenticatePassword = entryVerificationpassword.Text;

            await _cslReaderService.SaveConfig();
            await DisplayAlert("Configuration Saved!", "", null, "OK");

            if (entryReaderName.Text != _cslReaderService.reader!.ReaderName)
            {
                _cslReaderService.reader!.bluetoothIC.SetDeviceName (entryReaderName.Text);
                await DisplayAlert("New Reader Name effective after reset CS108", "", null, "OK");
            }
        }

        public async void buttonBatteryLevelFormatClicked(object sender, EventArgs e)
        {
            var answer = await DisplayActionSheet("View Battery Level Format", "Cancel", null, "Voltage", "Percentage");

            if (answer != null && answer !="Cancel")
                buttonBatteryLevelFormat.Text = answer;
        }

        public async void buttonShareDataFormatClicked(object sender, EventArgs e)
        {
            var answer = await DisplayActionSheet("Share Data Format", null, null, _ShareDataFormatOptions);

            if (answer != null)
                buttonShareDataFormat.Text = answer;
        }

        public void btnBarcodeResetClicked(object sender, EventArgs e)
        {
            if (_cslReaderService.reader!.barcode.state == CSLibrary.BarcodeReader.STATE.NOTVALID)
            {
                DisplayAlert(null, "Barcode module not exists", "OK");
                return;
            }

            _cslReaderService.reader!.barcode.FactoryReset();
        }

        public async void btnConfigResetClicked(object sender, EventArgs e)
        {
            _cslReaderService.ResetConfig();
            _cslReaderService.reader!.rfid.SetDefaultChannel();

            if (_cslReaderService.reader!.rfid.IsFixedChannel())
            {
                _cslReaderService.config!.RFID_FrequenceSwitch = 1;
                _cslReaderService.config!.RFID_FixedChannel = _cslReaderService.reader!.rfid.GetCurrentFrequencyChannel();
            }
            else
            {
                _cslReaderService.config!.RFID_FrequenceSwitch = 0; // Hopping
            }

            await _cslReaderService.SaveConfig()!;


            string macadd = _cslReaderService.reader.GetMacAddress();

            if (macadd.Length >= 6)
            {
                if (_cslReaderService.reader.rfid.GetModel() == CSLibrary.RFIDDEVICE.MODEL.CS108)
                {

                    _cslReaderService.reader.bluetoothIC.SetDeviceName("CS108Reader" + macadd.Substring(macadd.Length - 6));
                    await DisplayAlert("New Reader Name effective after reset CS108", "", null, "OK");
                }
                else if (_cslReaderService.reader.rfid.GetModel() == CSLibrary.RFIDDEVICE.MODEL.CS710S)
                {
                    _cslReaderService.reader.bluetoothIC.SetDeviceName("CS710SReader" + macadd.Substring(macadd.Length - 6));
                    await DisplayAlert("New Reader Name effective after reset CS710S", "", null, "OK");
                }
            }
        }

        public void btnGetSerialNumber(object sender, EventArgs e)
        {
            _cslReaderService.reader!.siliconlabIC.GetSerialNumber();
        }

        public async void btnFunctionSelectedClicked(object sender, EventArgs e)
        {
            var answer = await DisplayActionSheet(null,CONFIG.MAINMENUSHORTCUT.FUNCTION.NONE.ToString(), null, CONFIG.MAINMENUSHORTCUT.FUNCTION.INVENTORY.ToString(),CONFIG.MAINMENUSHORTCUT.FUNCTION.BARCODE.ToString());

            Button b = (Button)sender;
            b.Text = answer;
        }

        public void btnCSLCloudClicked(object sender, EventArgs e)
        {
            switchhttpProtocol.IsToggled = false;
            //entryServerIP.Text = "https://www.convergence.com.hk:29090/WebServiceRESTs/1.0/req";
            entryServerIP.Text = "https://democloud.convergence.com.hk:29090/WebServiceRESTs/1.0/req";
        }
    }
}
