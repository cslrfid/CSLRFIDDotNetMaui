using Controls.UserDialogs.Maui;
using CSLRFIDMobile.Services;
using CSLRFIDMobile.Model;
using System.Security.Principal;

namespace CSLRFIDMobile.ViewModel
{
    public partial class ViewModelInventory : BaseViewModel
    {
        private readonly IUserDialogs _userDialogs;
        private readonly CSLReaderService _cslReaderService;

        public ObservableCollection<TagInfoViewModel> TagInfoList { get; set; } = new();
        public ObservableCollection<BARCODEInfoViewModel> BarcodeData { get; set; } = new();

        private bool _InventoryScanning = false;
        private bool _BarcodeScanning = false;
        public bool _KeyDown = false;

        public Action? ClearRfidListView { get; set; }

        public ViewModelInventory(IUserDialogs userDialogs, CSLReaderService cslReaderService)
        {
            _userDialogs = userDialogs;
            _cslReaderService = cslReaderService;

            _cslReaderService.reader!.barcode.FastBarcodeMode(false);

            InventorySetting();
        }

        [ObservableProperty]
        public string startInventoryButtonText = "Start Inventory";
        [ObservableProperty]
        public string labelVoltage = String.Empty;
        [ObservableProperty]
        public string labelVoltageTextColor = "Black";
        [ObservableProperty]
        public int tagCount = 0;
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsRfidReaderMode))]
        [NotifyPropertyChangedFor(nameof(IsBarcodeReaderMode))]
        public string readerModeImage = "rfid.png";

        [ObservableProperty]
        public bool switchFlashTagsIsToggled = false;

        public bool IsRfidReaderMode => ReaderModeImage == "rfid.png";
        public bool IsBarcodeReaderMode => ReaderModeImage == "barcode.png";


        [RelayCommand]
        void StartInventoryButton()
        {
            if (IsRfidReaderMode)
            {
                if (!_InventoryScanning)
                {
                    StartInventory();
                }
                else
                {
                    StopInventory();
                }
            }
            else
            {
                if (_BarcodeScanning)
                {
                    BarcodeStop();
                }
                else
                {
                    if (_cslReaderService.reader!.BLEBusy)
                    {
                        _userDialogs.ShowToast("Configuring Reader, Please Wait", null, TimeSpan.FromSeconds(1));
                    }
                    else
                    {
                        BarcodeStart();
                    }
                }
            }
            
        }

        [RelayCommand]
        private void ClearButton()
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                if (IsRfidReaderMode)
                {
                    lock (TagInfoList)
                    {
                        TagInfoList.Clear();
                        TagCount = 0;
                    }
                }
                else
                {
                    lock (BarcodeData)
                    {
                        BarcodeData.Clear();
                    }
                }
            });
        }

        [RelayCommand]
        private async Task TagSelected(object obj)
        {
            TagInfoViewModel tag =(TagInfoViewModel)((SelectedItemChangedEventArgs)obj).SelectedItem;

            if (await _userDialogs.ConfirmAsync($"Select the tag '{tag.EPC}'?"))
            {
                _cslReaderService._SELECT_EPC = tag.EPC;
            }

            ClearRfidListView?.Invoke();
        }

        [RelayCommand]
        private void ReadModeChange()
        {
            if (ReaderModeImage == "rfid.png")
            {
                ReaderModeImage = "barcode.png";
                StartInventoryButtonText = "Start Scan";
                ClearButton();
                SetEvent(true);
                _cslReaderService.reader!.barcode.FastBarcodeMode(true);
            }
            else
            {
                ReaderModeImage = "rfid.png";
                StartInventoryButtonText = "Start Inventory";
                ClearButton();
                SetEvent(true);
                _cslReaderService.reader!.barcode.FastBarcodeMode(false);
            }
        }

        public override async Task OnAppearing()
        {
            await base.OnAppearing();
            SetEvent(true);
        }

        public override async Task OnDisappearing()
        {
            await base.OnDisappearing();
            SetEvent(false);
        }

        private void SetEvent(bool enable)
        {
            // Cancel RFID event handler
            _cslReaderService.reader?.rfid.ClearEventHandler();

            // Cancel Barcode event handler
            _cslReaderService.reader?.barcode.ClearEventHandler();

            // Key Button event handler
            _cslReaderService.reader?.notification.ClearEventHandler();

            if (enable)
            {
                // RFID event handler
                _cslReaderService.reader!.rfid.OnAsyncCallback += new EventHandler<CSLibrary.Events.OnAsyncCallbackEventArgs>(TagInventoryEvent!);
                _cslReaderService.reader!.rfid.OnStateChanged += new EventHandler<CSLibrary.Events.OnStateChangedEventArgs>(StateChangedEvent!);

                // Barcode event handler
                _cslReaderService.reader!.barcode.OnCapturedNotify += new EventHandler<CSLibrary.Barcode.BarcodeEventArgs>(Linkage_CaptureCompleted!);

                // Key Button event handler
                _cslReaderService.reader!.notification.OnKeyEvent += new EventHandler<CSLibrary.Notification.HotKeyEventArgs>(HotKeys_OnKeyEvent!);

                _cslReaderService.BatteryLevelEvent += _cslReaderService_BatteryLevelEvent;
                _cslReaderService.EnableBatteryEvent();
            }
            else
            {
                _cslReaderService.BatteryLevelEvent -= _cslReaderService_BatteryLevelEvent;
            }
        }

        private void _cslReaderService_BatteryLevelEvent(object? sender, CSLBatteryLevelEventArgs e)
        {
            LabelVoltage = e.BatteryValue;
            LabelVoltageTextColor = e.IsLowBattery ? "Red" : "Black";
        }

        void StartInventory()
        {
            if (_InventoryScanning)
            {
                _userDialogs.ShowToast("Configuring Reader, Please Wait", null, TimeSpan.FromSeconds(1));
                return;
            }

            _InventoryScanning = true;
            StartInventoryButtonText = "Stop Inventory";

            _cslReaderService.reader!.rfid.CancelAllSelectCriteria();
            _cslReaderService.reader!.rfid.Options.TagRanging.flags = CSLibrary.Constants.SelectFlags.ZERO;

            if (SwitchFlashTagsIsToggled)
            {
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

                // Select Criteria filter for LED Tag
                _cslReaderService.reader!.rfid.Options.TagRanging.flags = CSLibrary.Constants.SelectFlags.SELECT;
                _cslReaderService.reader!.rfid.Options.TagSelected.bank = CSLibrary.Constants.MemoryBank.TID;
                _cslReaderService.reader!.rfid.Options.TagSelected.Mask = new byte[] { 0xE2, 0x01, 0xE2 };
                _cslReaderService.reader!.rfid.Options.TagSelected.MaskOffset = 0;
                _cslReaderService.reader!.rfid.Options.TagSelected.MaskLength = 24;


                _cslReaderService.reader!.rfid.StartOperation(CSLibrary.Constants.Operation.TAG_PREFILTER);

                _cslReaderService.reader!.rfid.Options.TagRanging.multibanks = 1;
                //_cslReaderService.reader!.rfid.Options.TagRanging.bank1 = CSLibrary.Constants.MemoryBank.TID;
                //_cslReaderService.reader!.rfid.Options.TagRanging.offset1 = 0;
                //_cslReaderService.reader!.rfid.Options.TagRanging.count1 = 6;

                _cslReaderService.reader!.rfid.Options.TagRanging.bank1 = CSLibrary.Constants.MemoryBank.USER;
                _cslReaderService.reader!.rfid.Options.TagRanging.offset1 = 112;
                _cslReaderService.reader!.rfid.Options.TagRanging.count1 = 1;

                _cslReaderService.reader!.rfid.Options.TagRanging.compactmode = false;
                
            }
            else
            {                
                _cslReaderService.reader!.rfid.Options.TagRanging.multibanks = 0;
                _cslReaderService.reader!.rfid.Options.TagRanging.compactmode = true;
                InventorySetting();
            }

            _cslReaderService.reader!.rfid.StartOperation(CSLibrary.Constants.Operation.TAG_PRERANGING);
            _cslReaderService.reader?.rfid.StartOperation(CSLibrary.Constants.Operation.TAG_RANGING);
            ClassBattery.SetBatteryMode(ClassBattery.BATTERYMODE.INVENTORY);
        }

        void StopInventory()
        {
            if (!_InventoryScanning)
                return;

            _cslReaderService.reader?.rfid.StopOperation();
            _InventoryScanning = false;
            StartInventoryButtonText = "Start Inventory";
        }

        void InventorySetting()
        {
            _cslReaderService.reader?.rfid.CancelAllSelectCriteria();
            _cslReaderService.reader!.rfid.Options.TagRanging.flags = CSLibrary.Constants.SelectFlags.ZERO;

            _cslReaderService.reader?.rfid.SetInventoryDuration(_cslReaderService.config?.RFID_Antenna_Dwell);
            _cslReaderService.reader?.rfid.SetTagDelayTime((uint)_cslReaderService.config?.RFID_CompactInventoryDelayTime!); // for CS108 only
            _cslReaderService.reader?.rfid.SetIntraPacketDelayTime((uint)_cslReaderService.config?.RFID_IntraPacketDelayTime!); // for CS710S only
            _cslReaderService.reader?.rfid.SetDuplicateEliminationRollingWindow((uint)_cslReaderService.config?.RFID_DuplicateEliminationRollingWindow!); // for CS710S only
            _cslReaderService.reader?.rfid.SetCurrentLinkProfile((uint)_cslReaderService.config?.RFID_Profile!);
            _cslReaderService.reader?.rfid.SetTagGroup(_cslReaderService.config?.RFID_TagGroup);

            if (_cslReaderService.config?.RFID_Algorithm == CSLibrary.Constants.SingulationAlgorithm.DYNAMICQ)
            {
                _cslReaderService.config!.RFID_DynamicQParms.toggleTarget = _cslReaderService.config!.RFID_ToggleTarget ? 1U : 0;
                _cslReaderService.reader?.rfid.SetDynamicQParms(_cslReaderService.config?.RFID_DynamicQParms);
            }
            else
            {
                _cslReaderService.config!.RFID_FixedQParms.toggleTarget = _cslReaderService.config!.RFID_ToggleTarget ? 1U : 0;
                _cslReaderService.reader?.rfid.SetFixedQParms(_cslReaderService.config!.RFID_FixedQParms);
            }

            _cslReaderService.reader?.rfid.StartOperation(CSLibrary.Constants.Operation.TAG_PRERANGING);

        }

        void BarcodeStart()
        {
            if (_cslReaderService.reader!.barcode.state == CSLibrary.BarcodeReader.STATE.NOTVALID)
            {
                _userDialogs.ShowSnackbar("Barcode module not exists", null, null, TimeSpan.FromSeconds(1));
                return;
            }

            _BarcodeScanning = true;

            _cslReaderService.reader!.barcode.Start();
            StartInventoryButtonText = "Stop Scan";
        }


        void BarcodeStop()
        {
            _BarcodeScanning = false;
            _cslReaderService.reader!.barcode.Stop();

            StartInventoryButtonText = "Start Scan";            
        }


        void Linkage_CaptureCompleted(object sender, CSLibrary.Barcode.BarcodeEventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                switch (e.MessageType)
                {
                    case CSLibrary.Barcode.Constants.MessageType.DEC_MSG:
                        AddOrUpdateBarcodeData((CSLibrary.Barcode.Structures.DecodeMessage)e.Message);
                        break;

                    case CSLibrary.Barcode.Constants.MessageType.ERR_MSG:
                        break;
                }
            });
        }

        private void AddOrUpdateBarcodeData(CSLibrary.Barcode.Structures.DecodeMessage decodeInfo)
        {
            if (decodeInfo != null)
            {
                int cnt = 0;
                bool found = false;

                for (; cnt < BarcodeData.Count; cnt++)
                {
                    if (BarcodeData[cnt].Code == decodeInfo.pchMessage)
                    {
                        BarcodeData[cnt].Count++;
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    BARCODEInfoViewModel item = new();

                    item.Code = decodeInfo.pchMessage;
                    item.Count = 1;
                    item.timeOfRead = DateTime.Now;

                    BarcodeData.Insert(0, item);
                }
            }
        }

        void TagInventoryEvent(object sender, CSLibrary.Events.OnAsyncCallbackEventArgs e)
        {
            if (e.type != CSLibrary.Constants.CallbackType.TAG_RANGING)
                return;
            
            AddOrUpdateTagData(e.info);

        }

        private void AddOrUpdateTagData(CSLibrary.Structures.TagCallbackInfo info)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                lock (TagInfoList)
                {
                    bool found = false;
                    for (int cnt = 0; cnt < TagInfoList.Count; cnt++)
                    {
                        if (TagInfoList[cnt].EPC == info.epc.ToString())
                        {
                            TagInfoList[cnt].Bank1Data = CSLibrary.Tools.Hex.ToString(info.Bank1Data);
                            TagInfoList[cnt].Bank2Data = CSLibrary.Tools.Hex.ToString(info.Bank2Data);
                            TagInfoList[cnt].RSSI = info.rssi;

                            found = true;
                            break;
                        }
                    }

                    if (!found)
                    {
                        TagInfoViewModel item = new TagInfoViewModel();

                        item.timeOfRead = DateTime.Now;
                        item.EPC = info.epc.ToString();
                        item.Bank1Data = CSLibrary.Tools.Hex.ToString(info.Bank1Data);
                        item.Bank2Data = CSLibrary.Tools.Hex.ToString(info.Bank2Data);
                        item.RSSI = info.rssi;

                        item.PC = info.pc.ToUshorts()[0];

                        TagInfoList.Insert(0, item);
                    }

                    TagCount = TagInfoList.Count;

                }
            });
        }

        void StateChangedEvent(object sender, CSLibrary.Events.OnStateChangedEventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                switch (e.state)
                {
                    case CSLibrary.Constants.RFState.IDLE:
                        ClassBattery.SetBatteryMode(ClassBattery.BATTERYMODE.IDLE);

                        if (_cslReaderService.reader?.rfid.GetModelName() == "CS710S")
                        {
                            switch (_cslReaderService.reader?.rfid.LastMacErrorCode)
                            {
                                case 0x00:  // normal end
                                    break;

                                default:
                                    _userDialogs.Alert("Last error : 0x" + _cslReaderService.reader?.rfid.LastMacErrorCode.ToString("X4"));
                                    break;
                            }
                        }
                        else
                        {
                            switch (_cslReaderService.reader?.rfid.LastMacErrorCode)
                            {
                                case 0x00:  // normal end
                                    break;

                                case 0x0309:    // 
                                    _userDialogs.Alert("Too near to metal, please move CS108 away from metal and start inventory again.");
                                    break;

                                default:
                                    _userDialogs.Alert("Mac error : 0x" + _cslReaderService.reader?.rfid.LastMacErrorCode.ToString("X4"));
                                    break;
                            }
                        }
                        break;
                }
            });
        }

        void HotKeys_OnKeyEvent(object sender, CSLibrary.Notification.HotKeyEventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                if (e.KeyCode == CSLibrary.Notification.Key.BUTTON)
                {
                    if (e.KeyDown)
                    {
                        if (!_KeyDown && IsRfidReaderMode)
                            StartInventory();
                        _KeyDown = true;
                    }
                    else
                    {
                        if (_KeyDown && IsRfidReaderMode)
                            StopInventory();
                        _KeyDown = false;
                    }
                }
            });
        }

    }
}
