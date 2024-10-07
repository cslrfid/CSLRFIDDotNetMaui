using System;
using CSLRFIDMobile.Services;

namespace CSLRFIDMobile.View
{
	public partial class PageSettingAntenna : ContentPage
	{
        private readonly CSLReaderService _cslReaderService;

        class ANTENNAOPTION
        {
            private Microsoft.Maui.Controls.Switch? switchAntennaEnable;
            private Microsoft.Maui.Controls.Entry? entryPower;
            private Microsoft.Maui.Controls.Entry? entryDwell;
        }

        public PageSettingAntenna(CSLReaderService cslReaderService)
        {
            InitializeComponent();

            _cslReaderService = cslReaderService;

            // the page only support 4 ports
            if (_cslReaderService.reader!.rfid.GetAntennaPort() != 4)
                return;

            if (DeviceInfo.Current.Platform == DevicePlatform.iOS)
            {
                this.IconImageSource = ImageSource.FromFile("icons8-Settings-50-3-30x30.png");
            }

            ANTENNAOPTION[] antennaOptions = new ANTENNAOPTION[_cslReaderService.reader!.rfid.AntennaList.Count];

            switchAntenna1Enable.IsToggled = _cslReaderService.config!.RFID_AntennaEnable[0];
            switchAntenna2Enable.IsToggled = _cslReaderService.config!.RFID_AntennaEnable[1];
            switchAntenna3Enable.IsToggled = _cslReaderService.config!.RFID_AntennaEnable[2];
            switchAntenna4Enable.IsToggled = _cslReaderService.config!.RFID_AntennaEnable[3];

            entryPower1.Text = _cslReaderService.config!.RFID_Antenna_Power[0].ToString();
            entryPower2.Text = _cslReaderService.config!.RFID_Antenna_Power[1].ToString();
            entryPower3.Text = _cslReaderService.config!.RFID_Antenna_Power[2].ToString();
            entryPower4.Text = _cslReaderService.config!.RFID_Antenna_Power[3].ToString();

            entryDwell1.Text = _cslReaderService.config!.RFID_Antenna_Dwell[0].ToString();
            entryDwell2.Text = _cslReaderService.config!.RFID_Antenna_Dwell[1].ToString();
            entryDwell3.Text = _cslReaderService.config!.RFID_Antenna_Dwell[2].ToString();
            entryDwell4.Text = _cslReaderService.config!.RFID_Antenna_Dwell[3].ToString();
        }

        protected override void OnAppearing()
        {
            if (_cslReaderService._settingPage1TagPopulationChanged)
            {
                _cslReaderService._settingPage1TagPopulationChanged = false;
            }

            base.OnAppearing();
        }

        public async void btnOKClicked(object sender, EventArgs e)
        {
            _cslReaderService.config!.RFID_AntennaEnable[0] = switchAntenna1Enable.IsToggled;
            _cslReaderService.config!.RFID_AntennaEnable[1] = switchAntenna2Enable.IsToggled;
            _cslReaderService.config!.RFID_AntennaEnable[2] = switchAntenna3Enable.IsToggled;
            _cslReaderService.config!.RFID_AntennaEnable[3] = switchAntenna4Enable.IsToggled;

            _cslReaderService.config!.RFID_Antenna_Power[0] = uint.Parse(entryPower1.Text);
            _cslReaderService.config!.RFID_Antenna_Power[1] = uint.Parse(entryPower2.Text);
            _cslReaderService.config!.RFID_Antenna_Power[2] = uint.Parse(entryPower3.Text);
            _cslReaderService.config!.RFID_Antenna_Power[3] = uint.Parse(entryPower4.Text);

            _cslReaderService.config!.RFID_Antenna_Dwell[0] = uint.Parse(entryDwell1.Text);
            _cslReaderService.config!.RFID_Antenna_Dwell[1] = uint.Parse(entryDwell2.Text);
            _cslReaderService.config!.RFID_Antenna_Dwell[2] = uint.Parse(entryDwell3.Text);
            _cslReaderService.config!.RFID_Antenna_Dwell[3] = uint.Parse(entryDwell4.Text);

            await _cslReaderService.SaveConfig();

            for (uint cnt = 0; cnt < 4; cnt++)
            {
                _cslReaderService.reader!.rfid.SetAntennaPortState(cnt, _cslReaderService.config!.RFID_AntennaEnable[cnt] ? CSLibrary.Constants.AntennaPortState.ENABLED : CSLibrary.Constants.AntennaPortState.DISABLED);
                _cslReaderService.reader!.rfid.SetPowerLevel(_cslReaderService.config!.RFID_Antenna_Power[cnt], cnt);
                _cslReaderService.reader!.rfid.SetInventoryDuration(_cslReaderService.config!.RFID_Antenna_Dwell[cnt], cnt);
            }
        }
    }
}
