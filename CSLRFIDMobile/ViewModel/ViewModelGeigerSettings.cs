using System;
using System.Windows.Input;

using Plugin.BLE.Abstractions.Contracts;
using Controls.UserDialogs.Maui;
using CSLRFIDMobile.Services;

namespace CSLRFIDMobile.ViewModel
{
    public partial class ViewModelGeigerSettings : BaseViewModel
    {
        private readonly IUserDialogs _userDialogs;
        private readonly CSLReaderService _cslReaderService;

        [ObservableProperty]
        public string lowerLimit = String.Empty;

        [ObservableProperty]
        public string upperLimit = String.Empty;



        public ViewModelGeigerSettings(CSLReaderService cslReaderService, IUserDialogs userDialogs)
        {

            _userDialogs = userDialogs;
            _cslReaderService = cslReaderService;

            //pre-populate the fields with the saved config
            LowerLimit = _cslReaderService.config!.PowerLowerLimitIndBm.ToString();
            UpperLimit = _cslReaderService.config!.PowerUpperLimitIndBm.ToString();            

        }

        [RelayCommand]
        private void UpperLimitUnfocused()
        {
            int value;

            try
            {
                if (!int.TryParse(UpperLimit, out value))
                {
                    //reset value back to original
                    UpperLimit = _cslReaderService.config!.PowerUpperLimitIndBm.ToString();                     
                }

            }
            catch (Exception ex)
            {
                UpperLimit = _cslReaderService.config!.PowerUpperLimitIndBm.ToString();
            }
        }

        [RelayCommand]
        private void LowerLimitUnfocused()
        {
            int value;

            try
            {
                if (!int.TryParse(LowerLimit, out value))
                {
                    //reset value back to original
                    LowerLimit = _cslReaderService.config!.PowerLowerLimitIndBm.ToString();
                }

            }
            catch (Exception ex)
            {
                LowerLimit = _cslReaderService.config!.PowerLowerLimitIndBm.ToString();

            }
        }

        [RelayCommand]
        async Task SaveSettings()
        {            
            _cslReaderService.config!.PowerLowerLimitIndBm = int.Parse(LowerLimit);
            _cslReaderService.config!.PowerUpperLimitIndBm = int.Parse(UpperLimit);

            await _cslReaderService.SaveConfig();
            _userDialogs.Alert("Configuration Saved");
        }
    }
}
