
using NAudio.CoreAudioApi;

namespace RemoteControlBot
{
    public static class VolumeManager
    {
        public static bool IsBadMuteRequest { get; private set; }
        public static bool IsMuted { get; private set; }

        private static AudioEndpointVolume AudioEndpointVolume { get { return PlaybackDevice.AudioEndpointVolume; } }

        public static int VolumeLevel
        {
            get
            {
                return GetVolumeLevel();
            }
            private set
            {
                ChangeVolumeLevel(value);
            }
        }

        private static MMDevice PlaybackDevice { get { return GetDefaultPlaybackDevice(); } }

        public static void ChangeVolumeLevel(int changeLevel)
        {
            AudioEndpointVolume.MasterVolumeLevelScalar = (float)(VolumeLevel + changeLevel) / 100;
        }

        public static void Mute()
        {
            if (IsMuted)
            {
                IsBadMuteRequest = true;
                return;
            }

            IsBadMuteRequest = false;
            AudioEndpointVolume.Mute = IsMuted = true;
        }

        public static void UnMute()
        {
            if (!IsMuted)
            {
                IsBadMuteRequest = true;
                return;
            }

            IsBadMuteRequest = false;
            AudioEndpointVolume.Mute = IsMuted = false;
        }

        private static MMDevice GetDefaultPlaybackDevice()
        {
            return new MMDeviceEnumerator().GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
        }

        private static int GetVolumeLevel()
        {
            return (int)(AudioEndpointVolume.MasterVolumeLevelScalar * 100);
        }
    }
}
