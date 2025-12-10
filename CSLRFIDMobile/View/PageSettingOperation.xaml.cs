using CSLRFIDMobile.Services;


using static CSLibrary.FrequencyBand;

namespace CSLRFIDMobile.View
{

	public partial class PageSettingOperation : ContentPage
	{
        string[] ActiveRegionsTextList;
        double[] ActiveFrequencyList;
        string[] ActiveFrequencyTextList;
        string[] _profileList;
        string[] _freqOrderOptions = null!;

        private readonly CSLReaderService _cslReaderService;

        public PageSettingOperation(CSLReaderService cslReaderService)
        {
            InitializeComponent();

            _cslReaderService = cslReaderService;

            _profileList = _cslReaderService.reader!.rfid.GetActiveLinkProfileName();

            if (DeviceInfo.Current.Platform == DevicePlatform.iOS)
            {
                this.IconImageSource = ImageSource.FromFile("icons8-Settings-50-3-30x30.png");
            }

            stackLayoutInventoryDuration.IsVisible = stackLayoutPower.IsVisible = (_cslReaderService.reader!.rfid.GetAntennaPort() == 1);

            ActiveRegionsTextList = _cslReaderService.reader!.rfid.GetActiveCountryNameList();
            ActiveFrequencyList = _cslReaderService.reader!.rfid.GetAvailableFrequencyTable();
            if (ActiveFrequencyList != null && ActiveFrequencyList.Length > 0)
                ActiveFrequencyTextList = ActiveFrequencyList.OfType<object>().Select(o => o.ToString() + " MHz").ToArray();
            else
                ActiveFrequencyTextList = new string[] { "N/A" };
            buttonRegion.Text = _cslReaderService.config!.RFID_Region;

            switch (_cslReaderService.config!.RFID_FrequenceSwitch)
            {
                case 0:
                    buttonFrequencyOrder.Text = "Hopping";
                    break;
                case 1:
                    buttonFrequencyOrder.Text = "Fixed";
                    break;
            }

            if (ActiveRegionsTextList == null || ActiveRegionsTextList.Count() == 1)
                buttonRegion.IsEnabled = false;

//            if (_freqOrderOptions.Length == 1)
                buttonFrequencyOrder.IsEnabled = false;

            int fixedChannelIndex = _cslReaderService.config!.RFID_FixedChannel;
            if (fixedChannelIndex < 0 || fixedChannelIndex >= ActiveFrequencyTextList.Length)
                fixedChannelIndex = 0;
            buttonFixedChannel.Text = ActiveFrequencyTextList[fixedChannelIndex];

            checkbuttonFixedChannel();

            entryPower.Text = _cslReaderService.config!.RFID_Antenna_Power[0].ToString();
            entryInventoryDuration.Text = _cslReaderService.config!.RFID_Antenna_Dwell[0].ToString();
            entryCompactInventoryDelay.Text = _cslReaderService.config!.RFID_CompactInventoryDelayTime.ToString();
            entryIntraPacketDelay.Text = _cslReaderService.config!.RFID_IntraPacketDelayTime.ToString();
            entryDuplicateEliminationRollingWindow.Text = _cslReaderService.config!.RFID_DuplicateEliminationRollingWindow.ToString();

            buttonSession.Text = _cslReaderService.config!.RFID_TagGroup.session.ToString();
            if (_cslReaderService.config!.RFID_ToggleTarget)
            {
                buttonTarget.Text = "Toggle A/B";
            }
            else
            {
                buttonTarget.Text = _cslReaderService.config!.RFID_TagGroup.target.ToString();
            }

            switchFocus.IsToggled = _cslReaderService.config!.RFID_Focus;
            switchFastId.IsToggled = _cslReaderService.config!.RFID_FastId;

            buttonAlgorithm.Text = _cslReaderService.config!.RFID_Algorithm.ToString();
            entryTagPopulation.Text = _cslReaderService.config!.RFID_TagPopulation.ToString();
            if (_cslReaderService.config!.RFID_QOverride)
            {
                entryQOverride.IsEnabled = true;
                buttonQOverride.Text = "Reset";
            }
            else
            {
                entryQOverride.IsEnabled = false;
                buttonQOverride.Text = "Override";
            }

            entryMaxQ.Text = _cslReaderService.config!.RFID_DynamicQParms.maxQValue.ToString();
            entryMinQ.Text = _cslReaderService.config!.RFID_DynamicQParms.minQValue.ToString();
            entryMinQCycled.Text = _cslReaderService.config!.RFID_DynamicQParms.MinQCycles.ToString();

            switchQIncreaseUseQuery.IsToggled = _cslReaderService.config!.RFID_DynamicQParms.QIncreaseUseQuery;
            switchQDecreaseUseQuery.IsToggled = _cslReaderService.config!.RFID_DynamicQParms.QDecreaseUseQuery;
            entryNoEPCMaxQ.Text = _cslReaderService.config!.RFID_DynamicQParms.NoEPCMaxQ.ToString();

            if (_profileList != null && _profileList.Length > 0)
            {
                foreach (string profilestr in _profileList)
                {
                    int colonIndex = profilestr.IndexOf(":");
                    if (colonIndex > 0 && uint.Parse(profilestr.Substring(0, colonIndex)) == _cslReaderService.config!.RFID_Profile)
                    {
                        buttonProfile.Text = profilestr;
                        break;
                    }
                }
            }

            SetQvalue();
        }

        public async void buttonRegionClicked(object sender, EventArgs e)
        {
            if (ActiveRegionsTextList == null || ActiveRegionsTextList.Length == 0)
                return;

            var answer = await (Application.Current!.Windows[0].Page as Page)!.DisplayActionSheet("Regions", "Cancel", null, ActiveRegionsTextList);

            if (answer != null && answer != "Cancel")
            {
                int cnt;

                buttonRegion.Text = answer;

                for (cnt = 0; cnt < ActiveRegionsTextList.Length; cnt++)
                {
                    if (ActiveRegionsTextList[cnt] == answer)
                    {
                        var freqTable = _cslReaderService.reader!.rfid.GetAvailableFrequencyTable(ActiveRegionsTextList[cnt]);
                        ActiveFrequencyList = freqTable != null ? freqTable.ToArray() : new double[0];
                        break;
                    }
                }
                if (cnt == ActiveRegionsTextList.Length || ActiveFrequencyList == null || ActiveFrequencyList.Length == 0)
                    ActiveFrequencyList = new double[1] { 0.0 };

                ActiveFrequencyTextList = ActiveFrequencyList.OfType<object>().Select(o => o.ToString() + " MHz").ToArray();
                if (ActiveFrequencyTextList.Length > 0)
                    buttonFixedChannel.Text = ActiveFrequencyTextList[0];
            }
        }

        public async void buttonFrequencyOrderClicked(object sender, EventArgs e)
        {
            string answer;

            answer = await (Application.Current!.Windows[0].Page as Page)!.DisplayActionSheet("Frequence Channel Order", "Cancel", null, _freqOrderOptions);

            if (answer != null && answer != "Cancel")
                buttonFrequencyOrder.Text = answer;

            checkbuttonFixedChannel();
        }

        void checkbuttonFixedChannel()
        {
            if (buttonFrequencyOrder.Text == "Fixed")
                buttonFixedChannel.IsEnabled = true;
            else
                buttonFixedChannel.IsEnabled = false;
        }

        public async void buttonFixedChannelClicked(object sender, EventArgs e)
        {
            if (ActiveFrequencyTextList == null || ActiveFrequencyTextList.Length == 0)
                return;

            var answer = await (Application.Current!.Windows[0].Page as Page)!.DisplayActionSheet("Frequency Channel Order", "Cancel", null, ActiveFrequencyTextList);

            if (answer != null && answer != "Cancel")
                buttonFixedChannel.Text = answer;
        }

        public async void entryPowerCompleted(object sender, EventArgs e)
        {
            uint value;

            try
            {
                value = uint.Parse(entryPower.Text);
                if (value < 0 || value > 320)
                    throw new System.ArgumentException("Power can only be set to 320 or below", "Power");
                entryPower.Text = value.ToString();
            }
            catch (Exception ex)
            {
                await (Application.Current!.Windows[0].Page as Page)!.DisplayAlert("Power", "Power can only be set to 320 or below", "OK");
                entryPower.Text = "300";
            }
        }

        public async void entryTagPopulationCompleted(object sender, EventArgs e)
        {
            uint tagPopulation;

            try
            {
                tagPopulation = uint.Parse(entryTagPopulation.Text);
                if (tagPopulation < 1 || tagPopulation > 8192)
                    throw new System.ArgumentException("Value not valid", "tagPopulation");
                entryTagPopulation.Text = tagPopulation.ToString();
            }
            catch (Exception ex)
            {
                await (Application.Current!.Windows[0].Page as Page)!.DisplayAlert("", "Value not valid!!!", "OK");
                tagPopulation = 60;
                entryTagPopulation.Text = "60";
            }

            if (!entryQOverride.IsEnabled)
                entryQOverride.Text = ((uint)(Math.Log((tagPopulation * 2), 2)) + 1).ToString();
        }

        public async void entryQOverrideCompiled(object sender, EventArgs e)
        {
            uint Q;
            try
            {
                Q = uint.Parse(entryQOverride.Text);
                if (Q < 0 || Q > 15)
                    throw new System.ArgumentException("Value not valid", "tagPopulation");
                entryQOverride.Text = Q.ToString();
            }
            catch (Exception ex)
            {
                await (Application.Current!.Windows[0].Page as Page)!.DisplayAlert("", "Value not valid!!!", "OK");
                Q = 7;
                entryQOverride.Text = "7";
            }

            //entryTagPopulation.Text = (1U << (int)Q).ToString();
        }

        public async void buttonQOverrideClicked(object sender, EventArgs e)
        {
            if (entryQOverride.IsEnabled)
            {
                entryQOverride.IsEnabled = false;
                buttonQOverride.Text = "Override";
                entryTagPopulationCompleted(null, null);
            }
            else
            {
                entryQOverride.IsEnabled = true;
                buttonQOverride.Text = "Reset";
            }
        }

        public async void entryDuplicateEliminationRollingWindowCompleted(object sender, EventArgs e)
        {
            uint value;

            try
            {
                value = uint.Parse(entryDuplicateEliminationRollingWindow.Text);
                if (value < 0 || value > 255)
                    throw new System.ArgumentException("Value not valid", "DuplicateEliminationRollingWindow");
                entryDuplicateEliminationRollingWindow.Text = value.ToString();
            }
            catch (Exception ex)
            {
                await (Application.Current!.Windows[0].Page as Page)!.DisplayAlert("", "Value not valid!!!", "OK");
                entryDuplicateEliminationRollingWindow.Text = "0";
            }
        }

        public async void entryCompactInventoryDelayCompleted(object sender, EventArgs e)
        {
            int value;

            try
            {
                value = int.Parse(entryCompactInventoryDelay.Text);
                if (value < 0 || value > 15)
                    throw new System.ArgumentException("Value not valid", "tagPopulation");
                entryCompactInventoryDelay.Text = value.ToString();
            }
            catch (Exception ex)
            {
                await (Application.Current!.Windows[0].Page as Page)!.DisplayAlert("", "Value not valid!!!", "OK");
                entryCompactInventoryDelay.Text = "0";
            }
        }

        public async void entryIntraPacketDelayCompleted(object sender, EventArgs e)
        {
            int value;

            try
            {
                value = int.Parse(entryIntraPacketDelay.Text);
                if (value < 0 || value > 255)
                    throw new System.ArgumentException("Value not valid", "tagPopulation");
                entryIntraPacketDelay.Text = value.ToString();
            }
            catch (Exception ex)
            {
                await (Application.Current!.Windows[0].Page as Page)!.DisplayAlert("", "Value not valid!!!", "OK");
                entryIntraPacketDelay.Text = "0";
            }
        }



        protected override void OnAppearing()
        {
            if (_cslReaderService._settingPage1TagPopulationChanged)
            {
                _cslReaderService._settingPage1TagPopulationChanged = false;
                entryTagPopulation.Text = _cslReaderService.config!.RFID_TagPopulation.ToString();
            }

            base.OnAppearing();
        }

        public async void btnOKClicked(object sender, EventArgs e)
        {
            int cnt;

            for (cnt = 0; cnt < ActiveRegionsTextList.Length; cnt++)
            {
                if (ActiveRegionsTextList[cnt] == buttonRegion.Text)
                {
                    _cslReaderService.config!.RFID_Region = ActiveRegionsTextList[cnt];
                    break;
                }
            }
            if (cnt == ActiveRegionsTextList.Length)
                _cslReaderService.config!.RFID_Region = "UNKNOWN";

            switch (buttonFrequencyOrder.Text)
            {
                case "Hopping":
                    _cslReaderService.config!.RFID_FrequenceSwitch = 0;
                    break;
                case "Fixed":
                    _cslReaderService.config!.RFID_FrequenceSwitch = 1;
                    break;
            }

            if (ActiveFrequencyTextList != null && ActiveFrequencyTextList.Length > 0)
            {
                for (cnt = 0; cnt < ActiveFrequencyTextList.Length; cnt++)
                {
                    if (buttonFixedChannel.Text == ActiveFrequencyTextList[cnt])
                    {
                        _cslReaderService.config!.RFID_FixedChannel = cnt;
                        break;
                    }
                }
                if (cnt == ActiveFrequencyTextList.Length)
                    _cslReaderService.config!.RFID_FixedChannel = 0;
            }
            else
            {
                _cslReaderService.config!.RFID_FixedChannel = 0;
            }

            _cslReaderService.config!.RFID_Antenna_Power[0] = UInt16.Parse(entryPower.Text);
            _cslReaderService.config!.RFID_Antenna_Dwell[0] = UInt16.Parse(entryInventoryDuration.Text);
            _cslReaderService.config!.RFID_CompactInventoryDelayTime = int.Parse(entryCompactInventoryDelay.Text);
            _cslReaderService.config!.RFID_IntraPacketDelayTime = int.Parse(entryIntraPacketDelay.Text);
            _cslReaderService.config!.RFID_DuplicateEliminationRollingWindow = byte.Parse(entryDuplicateEliminationRollingWindow.Text);

            switch (buttonSession.Text)
            {
                case "S0":
                    _cslReaderService.config!.RFID_TagGroup.session = CSLibrary.Constants.Session.S0;
                    break;

                case "S1":
                    _cslReaderService.config!.RFID_TagGroup.session = CSLibrary.Constants.Session.S1;
                    break;

                case "S2":
                    _cslReaderService.config!.RFID_TagGroup.session = CSLibrary.Constants.Session.S2;
                    break;

                case "S3":
                    _cslReaderService.config!.RFID_TagGroup.session = CSLibrary.Constants.Session.S3;
                    break;
            }

            switch (buttonTarget.Text)
            {
                case "A":
                    _cslReaderService.config!.RFID_ToggleTarget = false;
                    _cslReaderService.config!.RFID_TagGroup.target = CSLibrary.Constants.SessionTarget.A;
                    _cslReaderService.config!.RFID_FixedQParms.toggleTarget = 0;
                    _cslReaderService.config!.RFID_DynamicQParms.toggleTarget = 0;
                    break;
                case "B":
                    _cslReaderService.config!.RFID_ToggleTarget = false;
                    _cslReaderService.config!.RFID_TagGroup.target = CSLibrary.Constants.SessionTarget.B;
                    _cslReaderService.config!.RFID_FixedQParms.toggleTarget = 0;
                    _cslReaderService.config!.RFID_DynamicQParms.toggleTarget = 0;
                    break;
                default:
                    _cslReaderService.config!.RFID_ToggleTarget = true;
                    _cslReaderService.config!.RFID_DynamicQParms.toggleTarget = 1;
                    _cslReaderService.config!.RFID_FixedQParms.toggleTarget = 1;
                    break;
            }
            _cslReaderService.config!.RFID_Focus = switchFocus.IsToggled;
            _cslReaderService.config!.RFID_FastId = switchFastId.IsToggled;

            if (buttonAlgorithm.Text == "DYNAMICQ")
            {
                _cslReaderService.config!.RFID_Algorithm = CSLibrary.Constants.SingulationAlgorithm.DYNAMICQ;
            }
            else
            {
                _cslReaderService.config!.RFID_Algorithm = CSLibrary.Constants.SingulationAlgorithm.FIXEDQ;
            }
            _cslReaderService.config!.RFID_TagPopulation = UInt16.Parse(entryTagPopulation.Text);
            _cslReaderService.config!.RFID_QOverride = entryQOverride.IsEnabled;
            _cslReaderService.config!.RFID_DynamicQParms.startQValue = uint.Parse(entryQOverride.Text);
            _cslReaderService.config!.RFID_DynamicQParms.maxQValue = uint.Parse(entryMaxQ.Text);
            _cslReaderService.config!.RFID_DynamicQParms.minQValue = uint.Parse(entryMinQ.Text);
            _cslReaderService.config!.RFID_FixedQParms.qValue = uint.Parse(entryQOverride.Text);
            _cslReaderService.config!.RFID_DynamicQParms.QIncreaseUseQuery = switchQIncreaseUseQuery.IsToggled;
            _cslReaderService.config!.RFID_DynamicQParms.QDecreaseUseQuery = switchQDecreaseUseQuery.IsToggled;
            _cslReaderService.config!.RFID_DynamicQParms.MinQCycles = uint.Parse(entryMinQCycled.Text);
            _cslReaderService.config!.RFID_DynamicQParms.NoEPCMaxQ = uint.Parse(entryNoEPCMaxQ.Text);

            int colonIndex = buttonProfile.Text.IndexOf(":");
            if (colonIndex > 0)
                _cslReaderService.config!.RFID_Profile = UInt16.Parse(buttonProfile.Text.Substring(0, colonIndex));

            await _cslReaderService.SaveConfig();

            _cslReaderService.reader!.rfid.SetCountry(_cslReaderService.config!.RFID_Region, (int)_cslReaderService.config!.RFID_FixedChannel);
        }

        public async void entryInventoryDurationCompleted(object sender, EventArgs e)
        {
            uint value;

            try
            {
                value = uint.Parse(entryInventoryDuration.Text);
                if (value < 0 || value > 3000)
                    throw new System.ArgumentException("Value not valid", "tagPopulation");
                entryInventoryDuration.Text = value.ToString();
            }
            catch (Exception ex)
            {
                await (Application.Current!.Windows[0].Page as Page)!.DisplayAlert("", "Value not valid!!!", "OK");
                entryInventoryDuration.Text = "0";
            }
        }

        public async void buttonSessionClicked(object sender, EventArgs e)
        {
            var answer = await (Application.Current!.Windows[0].Page as Page)!.DisplayActionSheet("Session", "Cancel", null, "S0", "S1", "S2", "S3"); // S2 S3

            if (answer != null && answer !="Cancel")
                buttonSession.Text = answer;
        }

        public async void buttonTargetClicked(object sender, EventArgs e)
        {
            var answer = await (Application.Current!.Windows[0].Page as Page)!.DisplayActionSheet(null, "Cancel", null, "A", "B", "Toggle A/B");

            if (answer != null && answer !="Cancel")
                buttonTarget.Text = answer;
        }

        public async void buttonAlgorithmClicked(object sender, EventArgs e)
        {
            var answer = await (Application.Current!.Windows[0].Page as Page)!.DisplayAlert("Algorithm", "", "DYNAMICQ", "FIXEDQ");
            buttonAlgorithm.Text = answer ? "DYNAMICQ" : "FIXEDQ";
        }

        void SetQvalue ()
        {
            switch (buttonAlgorithm.Text)
            {
                default:
                    entryQOverride.Text = "0";
                    break;

                case "DYNAMICQ":
                    entryQOverride.Text = _cslReaderService.config!.RFID_DynamicQParms.startQValue.ToString();
                    break;

                case "FIXEDQ":
                    entryQOverride.Text = _cslReaderService.config!.RFID_FixedQParms.qValue.ToString();
                    break;
            }
        }

        public async void buttonProfileClicked(object sender, EventArgs e)
        {
            if (_profileList == null || _profileList.Length == 0)
                return;

            var answer = await (Application.Current!.Windows[0].Page as Page)!.DisplayActionSheet(null, "Cancel", null, _profileList);

            if (answer != null && answer != "Cancel")
            {
                buttonProfile.Text = answer;

                if (_cslReaderService.reader!.rfid.GetModel() == CSLibrary.RFIDDEVICE.MODEL.CS108)
                {
                    int colonIndex = answer.IndexOf(":");
                    if (colonIndex > 0 && uint.Parse(answer.Substring(0, colonIndex)) == 3)
                        entryCompactInventoryDelay.Text = "2";
                    else
                        entryCompactInventoryDelay.Text = "0";
                }
            }
        }

        public void switchFocusPropertyChanged(object sender, EventArgs e)
        {
            if (switchFocus == null)
                return;

            if (switchFocus.IsToggled)
            {
                buttonSession.Text = "S1";
                buttonTarget.Text = "A";
                entryCompactInventoryDelay.Text = "0";
                entryInventoryDuration.Text = "2000";
                buttonSession.IsEnabled = false;
                buttonTarget.IsEnabled = false;
                entryCompactInventoryDelay.IsEnabled = false;
                entryInventoryDuration.IsEnabled = false;
            }
            else
            {
                buttonSession.IsEnabled = true;
                buttonTarget.IsEnabled = true;
                entryCompactInventoryDelay.IsEnabled = true;
                entryInventoryDuration.IsEnabled = true;
            }

        }
    }
}
