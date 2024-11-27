using CSLRFIDMobile.Services;


namespace CSLRFIDMobile.View
{
    public partial class PageAbout : ContentPage
    {
        private readonly CSLReaderService _cslReaderService;

        public PageAbout(CSLReaderService cslReaderService)
        {
            InitializeComponent();

            _cslReaderService = cslReaderService;

            if (DeviceInfo.Current.Platform == DevicePlatform.iOS)
            {
                this.IconImageSource = ImageSource.FromFile("icons8-Settings-50-4-30x30.png");
            }

            labelModel.Text = "Model " + _cslReaderService.reader!.rfid.GetFullModelName();
            //labelAppVer.Text = "Application Version " + DependencyService.Get<IAppVersion>().GetVersion() + "-" + DependencyService.Get<IAppVersion>().GetBuild().ToString();
            labelLibVer.Text = "Library Version " + _cslReaderService.reader!.GetVersion().ToString();
            labelBtFwVer.Text = "Bluetooth Firmware Version " + Version2String(_cslReaderService.reader!.bluetoothIC.GetFirmwareVersion());
            labelRFIDFwVer.Text = "RFID Firmware Version " + (_cslReaderService.reader!.rfid.GetFirmwareVersionString());
            if (_cslReaderService.reader!.rfid.GetModelName() == "CS710S")
                labelSiliconlabFwVer.Text = "ATMEL IC Firmware Version " + Version2String(_cslReaderService.reader!.siliconlabIC.GetFirmwareVersion());
            else
                labelSiliconlabFwVer.Text = "SiliconLab IC Firmware Version " + Version2String(_cslReaderService.reader!.siliconlabIC.GetFirmwareVersion());
            labelSerialNumber.Text = "Reader Serial Number " + _cslReaderService.reader!.siliconlabIC.GetSerialNumberSync();
            labelPCBSerialNumber.Text = "PCB Serial Number " + _cslReaderService.reader!.rfid.GetPCBAssemblyCode();
        }

        string Version2String(uint ver)
        {
            return string.Format("{0}.{1}.{2}", (ver >> 16) & 0xff, (ver >> 8) & 0xff, ver & 0xff);
        }

        string GetPCBVersion ()
        {
            try
            {
                var ver = _cslReaderService.reader!.siliconlabIC.GetPCBVersion();

                if (ver.Substring(2, 1) != "0")
                    return ver.Substring(0, 1) + "." + ver.Substring(1, 2);
                else
                    return ver.Substring(0, 1) + "." + ver.Substring(1, 1);
            }
            catch(Exception ex)
            {
                return "No PCB Version";
            }
        }

        public async void buttonOpenPrivacypolicyClicked(object sender, EventArgs args)
        {
            await Launcher.OpenAsync(new Uri("https://www.convergence.com.hk/apps-privacy-policy/"));
        }



    }
}
