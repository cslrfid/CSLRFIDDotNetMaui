using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CSLRFIDMobile.Services
{
    /// <summary>
    /// Application settings model for persisting user preferences
    /// </summary>
    public class AppSettings
    {
        /// <summary>
        /// Serial number of the last connected RFID reader device
        /// </summary>
        public string CSLLinkedDevice { get; set; } = String.Empty;

        /// <summary>
        /// BLE device UUID (Guid string) of the last connected reader
        /// </summary>
        public string CSLLinkedDeviceId { get; set; } = String.Empty;
    }

    /// <summary>
    /// Service for managing application state and persistent settings
    /// </summary>
    public class AppStateService
    {
        public AppSettings Settings { get; set; } = new AppSettings();

        private const string ConfigFileName = "appconfig.cfg";

        public AppStateService()
        {
            // Load configuration asynchronously on startup
            _ = LoadConfig();
        }

        /// <summary>
        /// Load application settings from persistent storage
        /// </summary>
        public async Task<bool> LoadConfig()
        {
            try
            {
                string contentJSON = String.Empty;
                string configFile = Path.Combine(FileSystem.Current.AppDataDirectory, ConfigFileName);

                if (File.Exists(configFile))
                {
                    using (StreamReader reader = new StreamReader(configFile))
                    {
                        contentJSON = await reader.ReadToEndAsync();
                        reader.Close();
                    }
                }

                var setting = String.IsNullOrEmpty(contentJSON) ? null : JsonConvert.DeserializeObject<AppSettings>(contentJSON);

                if (setting != null)
                {
                    Settings = setting;
                }
                else
                {
                    Settings = new AppSettings();
                }

                return true;
            }
            catch (Exception ex)
            {
                CSLibrary.Debug.WriteLine($"Error loading app config: {ex}");
                return false;
            }
        }

        /// <summary>
        /// Save application settings to persistent storage
        /// </summary>
        public async Task<bool> SaveConfig()
        {
            try
            {
                string configFile = Path.Combine(FileSystem.Current.AppDataDirectory, ConfigFileName);

                string contentJSON = JsonConvert.SerializeObject(Settings);
                using (StreamWriter writer = new StreamWriter(configFile))
                {
                    await writer.WriteAsync(contentJSON);
                    writer.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                CSLibrary.Debug.WriteLine($"Error saving app config: {ex}");
                return false;
            }
        }

        /// <summary>
        /// Reset application settings to defaults
        /// </summary>
        public void ResetConfig()
        {
            Settings.CSLLinkedDevice = String.Empty;
            Settings.CSLLinkedDeviceId = String.Empty;
        }

        /// <summary>
        /// Check if a device is currently linked for auto-reconnect
        /// </summary>
        public bool IsDeviceLinked()
        {
            return !string.IsNullOrEmpty(Settings.CSLLinkedDevice) &&
                   !string.IsNullOrEmpty(Settings.CSLLinkedDeviceId);
        }
    }
}
