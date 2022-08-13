using ManagedBass;

namespace LowRezJam22.Engine.Sounds
{

    internal class SoundState : IDisposable
    {
        private static bool _bassInit = false;
        private int _handle = -1;
        private float _volume = 1;
        public bool Looping = false;

        public SoundState(string file, float volume, bool looping)
        {
            if (!_bassInit)
            {
                ManagedBass.Bass.Init();
                _bassInit = true;
            }
            _volume = volume;
            Looping = looping;
            _handle = ManagedBass.Bass.CreateStream(file);
        }

        public void Play()
        {
            ManagedBass.Bass.ChannelSetFX(_handle, EffectType.Volume, 1);
            ManagedBass.Bass.ChannelSetAttribute(_handle, ChannelAttribute.Volume, _volume);
            ManagedBass.Bass.ChannelPlay(_handle, true);
        }

        public void Stop()
        {
            Looping = false;
            ManagedBass.Bass.ChannelStop(_handle);
        }

        public bool IsPlaying()
        {
            return ManagedBass.Bass.ChannelIsActive(_handle) == PlaybackState.Playing;
        }

        public void Dispose()
        {
            if (IsPlaying())
            {
                Stop();
            }
            ManagedBass.Bass.StreamFree(_handle);
        }
    }
}
