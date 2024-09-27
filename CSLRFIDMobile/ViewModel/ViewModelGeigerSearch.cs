using Controls.UserDialogs.Maui;
using CSLRFIDMobile.Services;
using CSLRFIDMobile.Model;
using System.Windows.Input;
using Plugin.Maui.Audio;
using CSLRFIDMobile.Helper;
using CSLRFIDMobile.View;

namespace CSLRFIDMobile.ViewModel
{
    public partial class ViewModelGeigerSearch : BaseViewModel, IDisposable
    {
        private readonly IUserDialogs _userDialogs;
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

        public ViewModelGeigerSearch(IUserDialogs userDialogs, CSLReaderService cslReaderService, IAudioManager audioManager)
        {
            _userDialogs = userDialogs;
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
            MainThread.BeginInvokeOnMainThread(() =>
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
                        SoundPlayer.PlaySound(_audioManager, "beephigh.mp3");

                        _beepSoundCount++;

                    if (_rssi >= _Threshold)
                    {
                        SoundPlayer.PlaySound(_audioManager, "beep3s1khz.mp3");
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
