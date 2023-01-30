using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AudioSwitcher.AudioApi.CoreAudio;

namespace RemoteControlBot
{
    public static class VolumeManager
    {
        private static readonly CoreAudioController _audioController = new();

        public static int ChangeVolume(int level)
        {
            PlaybackDevice().Volume += level;

            return (int)PlaybackDevice().Volume;
        }

        public static int Mute()
        {
            PlaybackDevice().Mute(true);

            return 0;
        }

        public static int UnMute()
        {
            PlaybackDevice().Mute(false);

            return 0;
        }

        private static CoreAudioDevice PlaybackDevice()
        {
            return _audioController.DefaultPlaybackDevice;
        } 
    }
}
