using AudioSwitcher.AudioApi.CoreAudio;

namespace RemoteControlBot
{
    public static class VolumeManager
    {
        public static bool IsBadMuteRequest { get; private set; }

        private static readonly CoreAudioController _audioController;

        static VolumeManager()
        {
            _audioController = new(); // too slow, to remove it use PreInit()
            IsBadMuteRequest = false;
        }

        public static void PreInit()
        {
            return;
        }


        public static int GetCurrentVolumeLevel()
        {
            return (int)PlaybackDevice().Volume;
        }

        public static void ChangeVolume(int level)
        {
            PlaybackDevice().Volume += level;
        }

        public static void Mute()
        {
            if (IsMuted())
            {
                IsBadMuteRequest = true;
                return;
            }

            IsBadMuteRequest = false;
            PlaybackDevice().Mute(true);
        }

        public static void UnMute()
        {
            if (!IsMuted())
            {
                IsBadMuteRequest = true;
                return;
            }

            IsBadMuteRequest = false;
            PlaybackDevice().Mute(false);
        }

        private static bool IsMuted()
        {
            return PlaybackDevice().IsMuted;
        }

        private static CoreAudioDevice PlaybackDevice()
        {
            return _audioController.DefaultPlaybackDevice;
        }
    }
}
