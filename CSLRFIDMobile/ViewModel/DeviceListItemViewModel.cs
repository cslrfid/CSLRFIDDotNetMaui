
using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.Contracts;
using System.Xml.Linq;
using static CSLibrary.RFIDDEVICE;

namespace CSLRFIDMobile.ViewModel
{
    public class DeviceListItemViewModel : BaseViewModel
    {
        public IDevice Device { get; private set; }
        public MODEL BTServiceType { get; private set; }
        
        public Guid Id => Device.Id;
        public string IdString {
            get {
                if (DeviceInfo.Current.Platform == DevicePlatform.iOS)
                    return Id.ToString();

                string idString = Id.ToString().ToUpper();
                string macString = idString.Substring(idString.Length - 12, 2) + ":";
                macString += idString.Substring(idString.Length - 10, 2) + ":";
                macString += idString.Substring(idString.Length - 8, 2) + ":";
                macString += idString.Substring(idString.Length - 6, 2) + ":";
                macString += idString.Substring(idString.Length - 4, 2) + ":";
                macString += idString.Substring(idString.Length - 2, 2);
                return macString;
            } 
        }
        public string Model => BTServiceType.ToString();

        public bool isConnected;
        public bool IsConnected
        {
            get => isConnected;
            set => SetProperty(ref isConnected, Device.State == DeviceState.Connected);
        }

        public int rssi;
        public bool Rssi
        {
            get => isConnected;
            set => SetProperty(ref rssi, Device.Rssi);
        }
        public string Name => Device.Name;

        public DeviceListItemViewModel(IDevice device, MODEL BTServiceType)
        {
            this.Device = device;
            this.BTServiceType = BTServiceType;
        }

        public void Update(IDevice? newDevice = null)
        {
            if (newDevice != null)
            {
                Device = newDevice;
            }
        }
    }
}
