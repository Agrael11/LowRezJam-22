using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LowRezJam22.Engine.Sounds
{
    internal static class SoundManager
    {
        private static List<SoundState> _states = new();
        
        public static void Update()
        {
            for (int i = _states.Count -1; i >= 0; i--)
            {
                if (!_states[i].IsPlaying())
                {
                    if (_states[i].Looping)
                    {
                        _states[i].Play();
                    }
                    else
                    {
                        _states[i].Dispose();
                        _states.RemoveAt(i);
                    }
                }
            }
        }

        public static void StopAndRemoveAll()
        {
            for (int i = _states.Count - 1; i >= 0; i--)
            {
                if (_states[i].IsPlaying())
                {
                    _states[i].Stop();
                }
                _states[i].Dispose();
                _states.RemoveAt(i);
            }
        }

        public static SoundState Add(string file, float volume, bool looping)
        {
            var state = new SoundState(file, volume, looping);
            state.Play();
            _states.Add(state);
            return state;
        }
    }
}
