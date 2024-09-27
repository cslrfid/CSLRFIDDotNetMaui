using Plugin.Maui.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSLRFIDMobile.Helper
{
    public static class SoundPlayer
    {
        public static void PlaySound(IAudioManager manager, string wavFile)
        {
            var wavFolder = Path.Combine(FileSystem.Current.AppDataDirectory, "Resources", "Raw");
            if (!Directory.Exists(wavFolder))
            {
                return;
            }
            string wavFileFullPath = Path.Combine(wavFolder, wavFile);

            Stream wavStream = File.OpenRead(wavFileFullPath);

            var player = manager.CreatePlayer(wavStream);
            player.Play();
        }
    }
}
