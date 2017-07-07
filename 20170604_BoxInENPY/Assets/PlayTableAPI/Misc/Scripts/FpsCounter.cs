using UnityEngine;
using System.Collections;

namespace Playmove
{
    public class FpsCounter
    {
        public static float FPS { get; private set; }

        private static float _acummulatedFps = 0;
        private static float _timer = 0.5f;
        private static int _frames = 0;

        public static void UpdateFPS()
        {
            _timer -= Time.deltaTime;
            _acummulatedFps += Time.timeScale / Time.deltaTime;
            ++_frames;

            if (_timer <= 0.0)
            {
                FPS = _acummulatedFps / _frames;

                _timer = 0.5f;
                _acummulatedFps = 0;
                _frames = 0;
            }
        }
    }
}