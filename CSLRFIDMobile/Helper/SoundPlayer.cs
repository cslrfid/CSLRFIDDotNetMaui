using Microsoft.Maui.Storage;
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
        public static async Task PlaySound(IAudioManager manager, string wavFile)
        {
            Stream wavStream = await FileSystem.Current.OpenAppPackageFileAsync(wavFile);

            var player = manager.CreatePlayer(wavStream);
            player.Play();
        }
    }
}
