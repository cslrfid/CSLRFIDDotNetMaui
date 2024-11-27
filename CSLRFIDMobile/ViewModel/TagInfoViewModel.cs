using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSLRFIDMobile.ViewModel
{
    public partial class TagInfoViewModel : BaseViewModel
    {
        [ObservableProperty]
        public string _EPC = String.Empty;
        [ObservableProperty]
        public string _EPC_ORG = String.Empty;
        [ObservableProperty]        
        public string _Bank1Data = String.Empty;
        [ObservableProperty]
        public string _Bank2Data = String.Empty;

        private float _RSSI;
        public float RSSI
        {
            get
            {
                return (float)Math.Round(_RSSI);
            }
            set
            {
                this.SetProperty(ref _RSSI, value);
            }
        }

        [ObservableProperty]
        public Int16 _Phase;
        [ObservableProperty]
        public string _Channel = String.Empty;
        [ObservableProperty]
        public UInt16 _PC;

        public DateTime timeOfRead;
    }

    public partial class BARCODEInfoViewModel : BaseViewModel
    {
        [ObservableProperty]
        public string code = String.Empty;
        [ObservableProperty]
        public uint count = 0;

        public DateTime timeOfRead;
    }
}
