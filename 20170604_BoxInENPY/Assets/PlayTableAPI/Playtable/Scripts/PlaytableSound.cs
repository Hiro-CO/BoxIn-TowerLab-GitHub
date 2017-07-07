using UnityEngine;
using System.Collections;
using System;

namespace Playmove
{
    public class PlaytableSound : MonoBehaviour
    {
        protected float _volume;
        /// <summary>
        /// Volume needs to be from 0 to 1
        /// </summary>
        public virtual float Volume
        {
            get
            {
                return _volume;
            }
            set
            {
                _volume = value;
                ChangeTableVolume(_volume);
            }
        }

        protected bool _mute;
        public virtual bool Mute
        {
            get
            {
                return _mute;
            }
            set
            {
                _mute = value;
                if (PlaytableWin32.Instance != null)
                {
                    if (_mute)
                        PlaytableWin32.Instance.DoMute();
                    else
                        PlaytableWin32.Instance.DoUnMute();
                }
            }
        }

        private Action _muteCallback, _volumeCallback;

        protected virtual void Start()
        {
            if (PlaytableWin32.Instance != null)
                PlaytableWin32.Instance.GetMute(SynchronizeMuteState);
        }

        /// <summary>
        /// Get mute is a request to Playtable, this request runs in a callback mode,
        /// so the function will set the property Mute instead of returning its value
        /// </summary>
        /// <param name="callback">After updating the Mute property a callback can be invoked</param>
        public void GetMute(Action callback = null)
        {
            _muteCallback = callback;
            if (PlaytableWin32.Instance != null)
                PlaytableWin32.Instance.GetMute(SynchronizeMuteState);
        }

        /// <summary>
        /// Get volume is a request to Playtable, this request runs in a callback mode,
        /// so the function will set the property Volume instead of returning its value
        /// </summary>
        /// <param name="callback">After updating the Volume property a callback can be invoked</param>
        public void GetVolume(Action callback = null)
        {
            if (PlaytableWin32.Instance != null)
                PlaytableWin32.Instance.GetVolume(SynchronizeVolume);
        }

        private void SynchronizeVolume(int volume)
        {
            Volume = volume;

            if (_volumeCallback != null)
                _volumeCallback();
            _volumeCallback = null;
        }

        private void SynchronizeMuteState(string text)
        {
            if (string.IsNullOrEmpty(text))
                Debug.LogError("String is null or empty. Verify your PlayTableAPI version.");
            else
                Mute = text == "true";

            if (_muteCallback != null)
                _muteCallback();
            _muteCallback = null;
        }

        private void ChangeTableVolume(float volume)
        {
            if (PlaytableWin32.Instance != null)
                PlaytableWin32.Instance.SetVolume(((int)(volume * 100)));
        }
    }
}