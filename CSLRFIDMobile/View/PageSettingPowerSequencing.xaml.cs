﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSLRFIDMobile.Services;

namespace CSLRFIDMobile.View
{
	public partial class PageSettingPower : ContentPage
    {
        private readonly CSLReaderService _cslReaderService;

        public PageSettingPower(CSLReaderService cslReaderService)
        {
            InitializeComponent();

            _cslReaderService = cslReaderService;

            if (DeviceInfo.Current.Platform == DevicePlatform.iOS)
            {
                this.IconImageSource = ImageSource.FromFile("icons8-Settings-50-4-30x30.png");
            }

            entryNumberofPower.Text = _cslReaderService.config!.RFID_PowerSequencing_NumberofPower.ToString();
            entryPower1.Text = _cslReaderService.config!.RFID_PowerSequencing_Level[0].ToString();
            entryDWell1.Text = _cslReaderService.config!.RFID_PowerSequencing_DWell[0].ToString();
            entryPower2.Text = _cslReaderService.config!.RFID_PowerSequencing_Level[1].ToString();
            entryDWell2.Text = _cslReaderService.config!.RFID_PowerSequencing_DWell[1].ToString();
            entryPower3.Text = _cslReaderService.config!.RFID_PowerSequencing_Level[2].ToString();
            entryDWell3.Text = _cslReaderService.config!.RFID_PowerSequencing_DWell[2].ToString();
            entryPower4.Text = _cslReaderService.config!.RFID_PowerSequencing_Level[3].ToString();
            entryDWell4.Text = _cslReaderService.config!.RFID_PowerSequencing_DWell[3].ToString();
            entryPower5.Text = _cslReaderService.config!.RFID_PowerSequencing_Level[4].ToString();
            entryDWell5.Text = _cslReaderService.config!.RFID_PowerSequencing_DWell[4].ToString();
            entryPower6.Text = _cslReaderService.config!.RFID_PowerSequencing_Level[5].ToString();
            entryDWell6.Text = _cslReaderService.config!.RFID_PowerSequencing_DWell[5].ToString();
            entryPower7.Text = _cslReaderService.config!.RFID_PowerSequencing_Level[6].ToString();
            entryDWell7.Text = _cslReaderService.config!.RFID_PowerSequencing_DWell[6].ToString();
            entryPower8.Text = _cslReaderService.config!.RFID_PowerSequencing_Level[7].ToString();
            entryDWell8.Text = _cslReaderService.config!.RFID_PowerSequencing_DWell[7].ToString();
            entryPower9.Text = _cslReaderService.config!.RFID_PowerSequencing_Level[8].ToString();
            entryDWell9.Text = _cslReaderService.config!.RFID_PowerSequencing_DWell[8].ToString();
            entryPower10.Text = _cslReaderService.config!.RFID_PowerSequencing_Level[9].ToString();
            entryDWell10.Text = _cslReaderService.config!.RFID_PowerSequencing_DWell[9].ToString();
            entryPower11.Text = _cslReaderService.config!.RFID_PowerSequencing_Level[10].ToString();
            entryDWell11.Text = _cslReaderService.config!.RFID_PowerSequencing_DWell[10].ToString();
            entryPower12.Text = _cslReaderService.config!.RFID_PowerSequencing_Level[11].ToString();
            entryDWell12.Text = _cslReaderService.config!.RFID_PowerSequencing_DWell[11].ToString();
            entryPower13.Text = _cslReaderService.config!.RFID_PowerSequencing_Level[12].ToString();
            entryDWell13.Text = _cslReaderService.config!.RFID_PowerSequencing_DWell[12].ToString();
            entryPower14.Text = _cslReaderService.config!.RFID_PowerSequencing_Level[13].ToString();
            entryDWell14.Text = _cslReaderService.config!.RFID_PowerSequencing_DWell[13].ToString();
            entryPower15.Text = _cslReaderService.config!.RFID_PowerSequencing_Level[14].ToString();
            entryDWell15.Text = _cslReaderService.config!.RFID_PowerSequencing_DWell[14].ToString();
            entryPower16.Text = _cslReaderService.config!.RFID_PowerSequencing_Level[15].ToString();
            entryDWell16.Text = _cslReaderService.config!.RFID_PowerSequencing_DWell[15].ToString();

            entryNumberofPowerUnfocused(null, null);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        public void entryNumberofPowerUnfocused(object sender, EventArgs e)
        {
            int numberofPower;

            try
            {
                numberofPower = int.Parse(entryNumberofPower.Text);

                if (numberofPower > 16)
                    numberofPower = 16;
                else if (numberofPower < 0)
                    numberofPower = 0;
            }
            catch (Exception ex)
            {
                numberofPower = 0; 
            }

            entryNumberofPower.Text = numberofPower.ToString();

            stackPower1.IsVisible = (numberofPower >= 1);
            stackPower2.IsVisible = (numberofPower >= 2);
            stackPower3.IsVisible = (numberofPower >= 3);
            stackPower4.IsVisible = (numberofPower >= 4);
            stackPower5.IsVisible = (numberofPower >= 5);
            stackPower6.IsVisible = (numberofPower >= 6);
            stackPower7.IsVisible = (numberofPower >= 7);
            stackPower8.IsVisible = (numberofPower >= 8);
            stackPower9.IsVisible = (numberofPower >= 9);
            stackPower10.IsVisible = (numberofPower >= 10);
            stackPower11.IsVisible = (numberofPower >= 11);
            stackPower12.IsVisible = (numberofPower >= 12);
            stackPower13.IsVisible = (numberofPower >= 13);
            stackPower14.IsVisible = (numberofPower >= 14);
            stackPower15.IsVisible = (numberofPower >= 15);
            stackPower16.IsVisible = (numberofPower >= 16);
        }

        public async void btnOKClicked(object sender, EventArgs e)
        {
            _cslReaderService.config!.RFID_PowerSequencing_NumberofPower = int.Parse(entryNumberofPower.Text);
            _cslReaderService.config!.RFID_PowerSequencing_Level[0] = uint.Parse(entryPower1.Text);
            _cslReaderService.config!.RFID_PowerSequencing_DWell[0] = uint.Parse(entryDWell1.Text);
            _cslReaderService.config!.RFID_PowerSequencing_Level[1] = uint.Parse(entryPower2.Text);
            _cslReaderService.config!.RFID_PowerSequencing_DWell[1] = uint.Parse(entryDWell2.Text);
            _cslReaderService.config!.RFID_PowerSequencing_Level[2] = uint.Parse(entryPower3.Text);
            _cslReaderService.config!.RFID_PowerSequencing_DWell[2] = uint.Parse(entryDWell3.Text);
            _cslReaderService.config!.RFID_PowerSequencing_Level[3] = uint.Parse(entryPower4.Text);
            _cslReaderService.config!.RFID_PowerSequencing_DWell[3] = uint.Parse(entryDWell4.Text);
            _cslReaderService.config!.RFID_PowerSequencing_Level[4] = uint.Parse(entryPower5.Text);
            _cslReaderService.config!.RFID_PowerSequencing_DWell[4] = uint.Parse(entryDWell5.Text);
            _cslReaderService.config!.RFID_PowerSequencing_Level[5] = uint.Parse(entryPower6.Text);
            _cslReaderService.config!.RFID_PowerSequencing_DWell[5] = uint.Parse(entryDWell6.Text);
            _cslReaderService.config!.RFID_PowerSequencing_Level[6] = uint.Parse(entryPower7.Text);
            _cslReaderService.config!.RFID_PowerSequencing_DWell[6] = uint.Parse(entryDWell7.Text);
            _cslReaderService.config!.RFID_PowerSequencing_Level[7] = uint.Parse(entryPower8.Text);
            _cslReaderService.config!.RFID_PowerSequencing_DWell[7] = uint.Parse(entryDWell8.Text);
            _cslReaderService.config!.RFID_PowerSequencing_Level[8] = uint.Parse(entryPower9.Text);
            _cslReaderService.config!.RFID_PowerSequencing_DWell[8] = uint.Parse(entryDWell9.Text);
            _cslReaderService.config!.RFID_PowerSequencing_Level[9] = uint.Parse(entryPower10.Text);
            _cslReaderService.config!.RFID_PowerSequencing_DWell[9] = uint.Parse(entryDWell10.Text);
            _cslReaderService.config!.RFID_PowerSequencing_Level[10] = uint.Parse(entryPower11.Text);
            _cslReaderService.config!.RFID_PowerSequencing_DWell[10] = uint.Parse(entryDWell11.Text);
            _cslReaderService.config!.RFID_PowerSequencing_Level[11] = uint.Parse(entryPower12.Text);
            _cslReaderService.config!.RFID_PowerSequencing_DWell[11] = uint.Parse(entryDWell12.Text);
            _cslReaderService.config!.RFID_PowerSequencing_Level[12] = uint.Parse(entryPower13.Text);
            _cslReaderService.config!.RFID_PowerSequencing_DWell[12] = uint.Parse(entryDWell13.Text);
            _cslReaderService.config!.RFID_PowerSequencing_Level[13] = uint.Parse(entryPower14.Text);
            _cslReaderService.config!.RFID_PowerSequencing_DWell[13] = uint.Parse(entryDWell14.Text);
            _cslReaderService.config!.RFID_PowerSequencing_Level[14] = uint.Parse(entryPower15.Text);
            _cslReaderService.config!.RFID_PowerSequencing_DWell[14] = uint.Parse(entryDWell15.Text);
            _cslReaderService.config!.RFID_PowerSequencing_Level[15] = uint.Parse(entryPower16.Text);
            _cslReaderService.config!.RFID_PowerSequencing_DWell[15] = uint.Parse(entryDWell16.Text);

            await _cslReaderService.SaveConfig();

            if (_cslReaderService.config!.RFID_PowerSequencing_NumberofPower == 0)
                _cslReaderService.reader!.rfid.SetPowerSequencing(0);
            else
                _cslReaderService.reader!.rfid.SetPowerSequencing(_cslReaderService.config!.RFID_PowerSequencing_NumberofPower, _cslReaderService.config!.RFID_PowerSequencing_Level, _cslReaderService.config!.RFID_PowerSequencing_DWell);
        }
    }
}
