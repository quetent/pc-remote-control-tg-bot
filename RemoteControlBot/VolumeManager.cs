using AudioSwitcher.AudioApi.CoreAudio;

namespace RemoteControlBot
{
    public static class VolumeManager
    {
        private static bool _badMuteRequest;

        private static readonly CoreAudioController _audioController;

        static VolumeManager()
        {
            _audioController = new(); // too slow, to remove it use PreInit()
            _badMuteRequest = false;
        }

        public static void PreInit()
        {
            return;
        }

        public static bool IsBadMuteRequest()
        {
            return _badMuteRequest;
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
                _badMuteRequest = true;
                return;
            }

            _badMuteRequest = false;
            PlaybackDevice().Mute(true);
        }

        public static void UnMute()
        {
            if (!IsMuted())
            {
                _badMuteRequest = true;
                return;
            }

            _badMuteRequest = false;
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
