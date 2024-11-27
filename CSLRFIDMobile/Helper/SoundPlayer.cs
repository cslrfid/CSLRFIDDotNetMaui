using Microsoft.Maui.Storage;
using Plugin.Maui.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSLRFIDMobile.Helper
{
    public enum SoundSelect
    {
        BEEP3S,
        BEEPHIGH,
        BEEPLOW
    }

    public static class SoundPlayer
    {
        private static IAudioPlayer? PlayerBeep3s;

        private static IAudioPlayer? PlayerBeepHigh;

        private static IAudioPlayer? PlayerBeepLow;

        public static async Task PlaySound(IAudioManager manager, SoundSelect select)
        {
            if (PlayerBeep3s == null)
            {
                Stream wavStream = await FileSystem.Current.OpenAppPackageFileAsync("beep3s1khz.mp3");
                PlayerBeep3s = manager.CreatePlayer(wavStream);
            }
            else
                PlayerBeep3s.Pause();

            if (PlayerBeepHigh == null)
            {
                Stream wavStream = await FileSystem.Current.OpenAppPackageFileAsync("beephigh.mp3");
                PlayerBeepHigh = manager.CreatePlayer(wavStream);
            }
            else
                PlayerBeepHigh.Pause();

            if (PlayerBeepLow == null)
            {
                Stream wavStream = await FileSystem.Current.OpenAppPackageFileAsync("beeplow.mp3");
                PlayerBeepLow = manager.CreatePlayer(wavStream);
            }
            else
                PlayerBeepLow.Pause();

            switch (select)
            {
                case SoundSelect.BEEP3S:
                    PlayerBeep3s?.Play();
                    break;
                case SoundSelect.BEEPHIGH:
                    PlayerBeepHigh?.Play();
                    break;
                case SoundSelect.BEEPLOW:
                    PlayerBeepLow?.Play();
                    break;
                default:
                    break;
            }
        }
    }
}
