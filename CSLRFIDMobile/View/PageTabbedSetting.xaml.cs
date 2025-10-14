using CSLRFIDMobile.Services;

namespace CSLRFIDMobile.View
{
    public partial class PageTabbedSetting : ContentPage
    {
        private readonly CSLReaderService _cslReaderService;

        public PageTabbedSetting(CSLReaderService cslReaderService)
        {
            InitializeComponent();
            _cslReaderService = cslReaderService;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Inject content from existing pages into tab hosts
            var operationPage = new PageSettingOperation(_cslReaderService);
            var operationContent = operationPage.Content;
            operationPage.Content = null;
            OperationHost.Content = operationContent;

            var administrationPage = new PageSettingAdministration(_cslReaderService);
            var administrationContent = administrationPage.Content;
            administrationPage.Content = null;
            AdministrationHost.Content = administrationContent;

            var powerSequencingPage = new PageSettingPowerSequencing(_cslReaderService);
            var powerSequencingContent = powerSequencingPage.Content;
            powerSequencingPage.Content = null;
            PowerSequencingHost.Content = powerSequencingContent;

            var antennaPage = new PageSettingAntenna(_cslReaderService);
            var antennaContent = antennaPage.Content;
            antennaPage.Content = null;
            AntennaHost.Content = antennaContent;

            // Show PowerSequencing or Antenna tab based on reader model
            var model = _cslReaderService.reader!.rfid.GetModelName();
            var isPsVisible = model == "CS108" || model == "CS710S";
            PowerSequencingTab.IsVisible = isPsVisible;
            AntennaTab.IsVisible = !isPsVisible;
        }
    }
}
