using CSLRFIDMobile.Services.Popups;
using CSLRFIDMobile.Services;
using CSLRFIDMobile.Model;
using System.Windows.Input;
using Plugin.Maui.Audio;
using CSLRFIDMobile.Helper;
using CSLRFIDMobile.View;
using CSLibrary.Constants;

namespace CSLRFIDMobile.ViewModel
{
    public partial class ViewModelGeigerSearch : BaseViewModel, IDisposable
    {
        private readonly IPopupService _popupService;
        private readonly CSLReaderService _cslReaderService;
        private readonly IAudioManager _audioManager;

        [RelayCommand]
        void StartGeigerButton()
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                if (!_startInventory)
                {
                    StartGeiger();
                }
                else
                {
                    StopGeiger();
                }
            });
        }

        [RelayCommand]
        async Task SettingsPage()
        {
            await Shell.Current.GoToAsync(nameof(PageGeigerSettings), true);
        }

        private Queue<double> _progressBarRollingWindow = new Queue<double>();
        private int _rssi = 0;
        private string rssi { get { return _rssi.ToString(); } }

        [ObservableProperty]
        public double progressbarRSSIValue = 0;

        [ObservableProperty]
        public int radialGaugeRSSIValue = 0;

        [ObservableProperty]
        public string startGeigerButtonText = "Start";

        [ObservableProperty]
        private string entryEPC = String.Empty;

        [ObservableProperty]
        public bool switchFlashTagsIsToggled = false;


        public IDispatcherTimer? timer;

        private int _Threshold = 0;
        public string labelThresholdText 
        { 
            get 
            { 
                return _Threshold.ToString(); 
            }
            set 
            { 
                try 
                { 
                    _Threshold = int.Parse(value); 
                } catch (Exception ex) { }
                OnPropertyChanged(labelThresholdText);
            } 
        }

        bool _startInventory = false;
        int _beepSoundCount = 0;
        int _noTagCount = 0;

        public ViewModelGeigerSearch(IPopupService popupService, CSLReaderService cslReaderService, IAudioManager audioManager)
        {
            _popupService = popupService;
            _cslReaderService = cslReaderService;
            _audioManager = audioManager;

            timer = Application.Current?.Dispatcher.CreateTimer();
            if (timer is not null)
            {
                timer!.Interval = TimeSpan.FromMilliseconds(50);
                timer!.Tick += (s, e) => PlayBeepSound();
                timer!.Start();
            }

            InventorySetting();
        }

        void StartGeiger()
        {
            if (_startInventory)
                return;

            StartGeigerButtonText = "Stop";
            _startInventory = true;

            ProgressbarRSSIValue = 0.0;
            RadialGaugeRSSIValue = 0;

            _progressBarRollingWindow.Clear();

            if (SwitchFlashTagsIsToggled)
            {
                InventoryLedSetting();
            }
            else
            {
                InventorySetting();
            }

            _cslReaderService.reader?.rfid.SetPowerLevel(_cslReaderService.config?.RFID_Antenna_Power);

            _cslReaderService.reader!.rfid.Options.TagSelected.flags = CSLibrary.Constants.SelectMaskFlags.ENABLE_TOGGLE;

            _cslReaderService.reader!.rfid.Options.TagSelected.bank = CSLibrary.Constants.MemoryBank.EPC;
            _cslReaderService.reader!.rfid.Options.TagSelected.epcMask = new CSLibrary.Structures.S_MASK(EntryEPC);
            _cslReaderService.reader!.rfid.Options.TagSelected.epcMaskOffset = 0;
            _cslReaderService.reader!.rfid.Options.TagSelected.epcMaskLength = (uint)(EntryEPC.Length) * 4;

            _cslReaderService.reader!.rfid.StartOperation(CSLibrary.Constants.Operation.TAG_SELECTED);

            //_cslReaderService.reader!.rfid.StartOperation(CSLibrary.Constants.Operation.TAG_EXESEARCHING);
            _cslReaderService.reader!.rfid.StartOperation(CSLibrary.Constants.Operation.TAG_RANGING);

            //beep differently rssi is exceeding threshold (percentage)
            _Threshold = 80;
        }

        void StopGeiger()
        {
            _startInventory = false;
            StartGeigerButtonText = "Start";

            _cslReaderService.reader!.rfid.StopOperation();
        }

        public override async Task OnAppearing()
        {
            await base.OnAppearing();

            // Cancel RFID event handler
            _cslReaderService.reader!.rfid.ClearEventHandler();
            // Key Button event handler
            _cslReaderService.reader.notification.ClearEventHandler();

            _cslReaderService.reader!.rfid.OnAsyncCallback += new EventHandler<CSLibrary.Events.OnAsyncCallbackEventArgs>(TagSearchOneEvent!);

            // Key Button event handler
            _cslReaderService.reader!.notification.OnKeyEvent += new EventHandler<CSLibrary.Notification.HotKeyEventArgs>(HotKeys_OnKeyEvent!);

            EntryEPC = _cslReaderService._SELECT_EPC;
        }

        public override async Task OnDisappearing()
        {
            StopGeiger();
            // Cancel RFID event handler
            _cslReaderService.reader!.rfid.ClearEventHandler();
            // Key Button event handler
            _cslReaderService.reader.notification.ClearEventHandler();

            await base.OnDisappearing();
            
        }

        void InventorySetting()
        {
            // Cancel old setting
            _cslReaderService.reader!.rfid.CancelAllSelectCriteria();
            _cslReaderService.reader!.rfid.SetPowerSequencing(0);

            // Set Geiger parameters
            _cslReaderService.reader!.rfid.SetInventoryDuration(_cslReaderService.config!.RFID_Antenna_Dwell);
            _cslReaderService.reader!.rfid.SetTagDelayTime((uint)_cslReaderService.config!.RFID_CompactInventoryDelayTime); // for CS108 only
            _cslReaderService.reader!.rfid.SetIntraPacketDelayTime((uint)_cslReaderService.config!.RFID_IntraPacketDelayTime); // for CS710S only
            _cslReaderService.reader!.rfid.SetDuplicateEliminationRollingWindow(0);
            _cslReaderService.config!.RFID_FixedQParms.qValue = 1;
            _cslReaderService.config!.RFID_FixedQParms.toggleTarget = 1;
            _cslReaderService.reader!.rfid.SetFixedQParms(_cslReaderService.config!.RFID_FixedQParms);
            _cslReaderService.reader!.rfid.SetCurrentSingulationAlgorithm(CSLibrary.Constants.SingulationAlgorithm.FIXEDQ);
            _cslReaderService.reader!.rfid.SetRSSIFilter(CSLibrary.Constants.RSSIFILTERTYPE.DISABLE);

            // Multi bank inventory
            _cslReaderService.reader!.rfid.Options.TagRanging.flags = CSLibrary.Constants.SelectFlags.SELECT;
            _cslReaderService.reader!.rfid.Options.TagRanging.multibanks = 0;
            _cslReaderService.reader!.rfid.Options.TagRanging.compactmode = true;
            _cslReaderService.reader!.rfid.Options.TagRanging.focus = _cslReaderService.config!.RFID_Focus;
        }

        void InventoryLedSetting()
        {
            // Optimatize for LED Light

            // Cancel old setting
            _cslReaderService.reader!.rfid.CancelAllSelectCriteria();
            _cslReaderService.reader!.rfid.SetPowerSequencing(0);

            _cslReaderService.reader!.rfid.SetTagDelayTime(0);
            _cslReaderService.reader!.rfid.SetIntraPacketDelayTime(0);
            _cslReaderService.reader!.rfid.SetDuplicateEliminationRollingWindow(0);
            _cslReaderService.reader!.rfid.SetInventoryDuration(0);
            _cslReaderService.config!.RFID_FixedQParms.qValue = 0;
            _cslReaderService.config!.RFID_FixedQParms.toggleTarget = 1;
            _cslReaderService.reader!.rfid.SetFixedQParms(_cslReaderService.config!.RFID_FixedQParms);
            _cslReaderService.reader!.rfid.SetCurrentSingulationAlgorithm(CSLibrary.Constants.SingulationAlgorithm.FIXEDQ);
            _cslReaderService.reader!.rfid.SetRSSIFilter(CSLibrary.Constants.RSSIFILTERTYPE.DISABLE);
            switch (_cslReaderService.reader!.rfid.GetModel())
            {
                case CSLibrary.RFIDDEVICE.MODEL.CS710S:
                    if (_cslReaderService.reader!.rfid.GetCountry() == 1)
                        _cslReaderService.reader!.rfid.SetCurrentLinkProfile(241);
                    else
                        _cslReaderService.reader!.rfid.SetCurrentLinkProfile(244);
                    break;

                default:
                    _cslReaderService.reader!.rfid.SetCurrentLinkProfile(_cslReaderService.config!.RFID_Profile);
                    break;
            }
            _cslReaderService.reader!.rfid.SetOperationMode(CSLibrary.Constants.RadioOperationMode.CONTINUOUS);
            _cslReaderService.reader!.rfid.SetTagGroup(Selected.ASSERTED, Session.S0, SessionTarget.A);

            // Multi bank inventory
            _cslReaderService.reader!.rfid.Options.TagRanging.flags = CSLibrary.Constants.SelectFlags.SELECT;
            _cslReaderService.reader!.rfid.Options.TagRanging.compactmode = false;
            _cslReaderService.reader!.rfid.Options.TagRanging.focus = false;

            _cslReaderService.reader!.rfid.Options.TagRanging.multibanks = 1;
            _cslReaderService.reader!.rfid.Options.TagRanging.bank1 = CSLibrary.Constants.MemoryBank.USER;
            _cslReaderService.reader!.rfid.Options.TagRanging.offset1 = 112;
            _cslReaderService.reader!.rfid.Options.TagRanging.count1 = 1;
        }

        public void TagSearchOneEvent(object sender, CSLibrary.Events.OnAsyncCallbackEventArgs e)
        {
            switch (e.type)
            {
                //case CSLibrary.Constants.CallbackType.TAG_SEARCHING:
                case CSLibrary.Constants.CallbackType.TAG_RANGING:

                    int dBmRssi = (int)(Math.Round(((CSLibrary.Structures.TagCallbackInfo)e.info).rssi - 106.98));
                    _noTagCount = 0;
                    double signalPercentage;

                    _noTagCount = 0;

                    if (dBmRssi >= _cslReaderService.config?.PowerUpperLimitIndBm)
                    {
                        signalPercentage = 1.00;
                    }
                    else if (dBmRssi <= _cslReaderService.config?.PowerLowerLimitIndBm)
                    {
                        signalPercentage = 0.00;
                    }
                    else
                    {
                        int range = _cslReaderService.config!.PowerUpperLimitIndBm - _cslReaderService.config!.PowerLowerLimitIndBm;
                        int scale = dBmRssi - _cslReaderService.config!.PowerLowerLimitIndBm;
                        float pct = (float)scale / (float)range;
                        signalPercentage = pct;
                    }

                    if (_progressBarRollingWindow.Count >= 20)
                    {
                        _progressBarRollingWindow.Dequeue();
                        _progressBarRollingWindow.Enqueue(signalPercentage);
                    }
                    else
                        _progressBarRollingWindow.Enqueue(signalPercentage);

                    ProgressbarRSSIValue = _progressBarRollingWindow.Average();
                    RadialGaugeRSSIValue = (int)(ProgressbarRSSIValue * 100);

                    _rssi = RadialGaugeRSSIValue;
                    OnPropertyChanged(nameof(rssi));
                    break;
            }
        }

        void StateChangedEvent(object sender, CSLibrary.Events.OnStateChangedEventArgs e)
        {
            switch (e.state)
            {
                case CSLibrary.Constants.RFState.IDLE:
                    break;
            }
        }
        void HotKeys_OnKeyEvent(object sender, CSLibrary.Notification.HotKeyEventArgs e)
        {

            if (e.KeyCode == CSLibrary.Notification.Key.BUTTON)
            {
                if (e.KeyDown)
                {
                    StartGeiger();
                }
                else
                {
                    StopGeiger();
                }
            }
        }

        void PlayBeepSound()
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                CSLibrary.Debug.WriteLine("Threshold {0}", _Threshold);

                if (_rssi == 0)
                {
                    _noTagCount++;

                    //if no tags found in 2 seconds, clear progress bar
                    if (_noTagCount > 40)
                    {
                        ProgressbarRSSIValue = 0.00;
                        RadialGaugeRSSIValue = 0;
                        _progressBarRollingWindow.Clear();

                    }

                }
                else
                {
                    if (_beepSoundCount == 0 && _rssi >= 0 && _rssi < 80)
                        await SoundPlayer.PlaySound(_audioManager, SoundSelect.BEEPHIGH);

                        _beepSoundCount++;

                    if (_rssi >= _Threshold)
                    {
                        await SoundPlayer.PlaySound(_audioManager, SoundSelect.BEEP3S);
                        _beepSoundCount = 1;
                        _rssi = 0;
                    }
                    else if (_rssi >= 50)
                    {
                        if (_beepSoundCount >= 5)
                        {
                            _beepSoundCount = 0;
                            _rssi = 0;
                        }
                    }
                    else if (_rssi >= 40)
                    {
                        if (_beepSoundCount >= 10)
                        {
                            _beepSoundCount = 0;
                            _rssi = 0;
                        }
                    }
                    else if (_rssi >= 30)
                    {
                        if (_beepSoundCount >= 20)
                        {
                            _beepSoundCount = 0;
                            _rssi = 0;
                        }
                    }
                    else if (_rssi >= 20)
                    {
                        if (_beepSoundCount >= 40)
                        {
                            _beepSoundCount = 0;
                            _rssi = 0;
                        }
                 
                    }
                }
            });
        }

        public void Dispose() 
        {
            if (timer is not null)
            {
                timer.Stop();
                timer = null;
            }
        }
    }
}
