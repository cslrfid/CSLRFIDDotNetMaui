﻿using Controls.UserDialogs.Maui;
using Plugin.BLE.Abstractions.Contracts;
using CSLRFIDMobile.Services;
using CSLRFIDMobile.Model;
using CSLRFIDMobile.View;


namespace CSLRFIDMobile.ViewModel
{
    public partial class ViewModelSetting : BaseViewModel
    {
        private readonly CSLReaderService _cslReaderService;
        private readonly IUserDialogs _userDialogs;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsAntennaConfigVisible))]
        public bool isPowerSequencingVisible = true;        
        public bool IsAntennaConfigVisible => !IsPowerSequencingVisible;

        [RelayCommand]
        async Task SettingOperations()
        {
            await Shell.Current.GoToAsync(nameof(PageSettingOperation), true);
        }

        [RelayCommand]
        async Task SettingAdministration()
        {
            await Shell.Current.GoToAsync(nameof(PageSettingAdministration), true);
        }

        [RelayCommand]
        async Task SettingAntennaPower()
        {
            await Shell.Current.GoToAsync(nameof(PageSettingAntenna), true);
        }

        [RelayCommand]
        async Task SettingPowerSequencing()
        {
            await Shell.Current.GoToAsync(nameof(PageSettingPower), true);
        }

        [RelayCommand]
        async Task SettingAbout()
        {
            await Shell.Current.GoToAsync(nameof(PageAbout), true);
        }

        public ViewModelSetting(CSLReaderService appStateService, IUserDialogs userDialogs)
        {
            _userDialogs = userDialogs;
            _cslReaderService = appStateService;

            switch (_cslReaderService.reader!.rfid.GetModelName())
            {
                case "CS108":
                case "CS710S":
                    IsPowerSequencingVisible = true;
                    break;

                default:
                    IsPowerSequencingVisible = false;
                    break;
            }


        }

        public override async Task OnAppearing()
        {
            await base.OnAppearing();

            _cslReaderService.reader!.siliconlabIC.OnAccessCompleted += new EventHandler<CSLibrary.SiliconLabIC.Events.OnAccessCompletedEventArgs>(OnAccessCompletedEvent!);
        }

        public override async Task OnDisappearing()
        {
            _cslReaderService.reader!.siliconlabIC.OnAccessCompleted -= new EventHandler<CSLibrary.SiliconLabIC.Events.OnAccessCompletedEventArgs>(OnAccessCompletedEvent!);

            await base.OnDisappearing();
        }

        void OnAccessCompletedEvent(object sender, CSLibrary.SiliconLabIC.Events.OnAccessCompletedEventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                switch (e.type)
                {
                    case CSLibrary.SiliconLabIC.Constants.AccessCompletedCallbackType.SERIALNUMBER:
                        _userDialogs.Alert("Serial Number : " + (string)e.info);
                        break;
                }
            });
        }
    }
}
