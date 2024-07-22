using Android.App;
using Android.Content.PM;
using Android.OS;

namespace CSLRFIDMobile
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle? bundle)
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate
            {
                return true;
            };

            base.OnCreate(bundle);

            Platform.Init(this, bundle);
            RequestedOrientation = ScreenOrientation.Portrait;
            Permissions.RequestAsync<ReadWriteStoragePerms>();
        }

    }

    public class ReadWriteStoragePerms : Permissions.BasePlatformPermission
    {
        public override (string androidPermission, bool isRuntime)[] RequiredPermissions =>
            new List<(string androidPermission, bool isRuntime)>
            {
                (global::Android.Manifest.Permission.ReadExternalStorage, true),
                (global::Android.Manifest.Permission.WriteExternalStorage, true)
            }.ToArray();
    }
}
