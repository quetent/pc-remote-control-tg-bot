using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AudioSwitcher.AudioApi.CoreAudio;

namespace RemoteControlBot
{
    public class VolumeManager
    {
        private static readonly CoreAudioController _audioController = new();

        public int ChangeVolume(int level)
        {
            _audioController.DefaultPlaybackDevice.Volume += level;

            return (int)_audioController.DefaultPlaybackDevice.Volume;
        }
    }
}
