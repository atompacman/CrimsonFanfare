using FXG.CrimFan.Config;
using FXG.CrimFan.World;
using JetBrains.Annotations;
using Sanford.Multimedia.Midi;
using UnityEngine;

namespace FXG.CrimFan.Audio.Midi
{
    public abstract class MidiInputSource : MonoBehaviour
    {
        #region Nested types

        public enum KeyState
        {
            IDLE,
            HIT,
            HELD,
            RELEASED
        }

        #endregion

        #region Protected fields

        protected KeyState[] KeyStates;

        protected float[] HitTime;

        protected float[] ReleaseTime;

        protected byte[] HitVelocity;


        #endregion

        #region Properties

        protected KeyboardConfig KeyboardConfig { get; private set; }

        #endregion

        #region Abstract methods

        protected abstract void UpdateKey(int i_Idx);

        #endregion

        #region Static methods

        public static MidiInputSource CreateComponent(Keyboard i_Keyboard)
        {
            // Look for a input device with the specified name
            InputDevice device = null;
            Debug.Log("[BTC] Looking for input MIDI controller");
            for (var i = 0; i < InputDevice.DeviceCount; ++i)
            {
                var cap = InputDevice.GetDeviceCapabilities(i);
                if (cap.name == i_Keyboard.Configuration.DeviceName)
                {
                    device = new InputDevice(i);
                }
                Debug.LogFormat("{0} - Version: {1} - Support: {2} - Mid: {3} - PID: {4}",
                    cap.name, cap.driverVersion, cap.support, cap.mid, cap.pid);
            }

            // If device was not found, use the computer keyboard as the midi input source
            MidiInputSource src;
            if (device == null)
            {
                Debug.Log("Could not find MIDI controller \"" + i_Keyboard.Configuration.DeviceName +
                          "\". Using computer keyboard instead.");
                src = ComputerKeyboardInputSource.CreateComponent(i_Keyboard);
            }
            else
            {
                src = MidiControllerInputSource.CreateComponent(i_Keyboard, device);
            }

            // Initialize common MidiInputSource members
            var numKeys = i_Keyboard.Configuration.NumKeys;
            src.KeyboardConfig = i_Keyboard.Configuration;
            src.KeyStates = new KeyState[numKeys];
            src.HitTime = new float[numKeys];
            src.ReleaseTime = new float[numKeys];
            src.HitVelocity = new byte[numKeys];

            // Keys start in idle state
            for (var i = 0; i < numKeys; ++i)
            {
                src.KeyStates[i] = KeyState.IDLE;
                src.HitTime[i] = float.MinValue;
                src.ReleaseTime[i] = float.MinValue;
                src.HitVelocity[i] = byte.MaxValue / 2;
            }

            return src;
        }

        #endregion

        #region Methods
        
        public KeyState GetKeyState(Pitch i_Pitch)
        {
            return KeyStates[IndexOf(i_Pitch)];
        }

        public float GetElapsedTimeSinceHit(Pitch i_Pitch)
        {
            return Time.fixedTime - HitTime[IndexOf(i_Pitch)];
        }

        public float GetElapsedTimeSinceRelease(Pitch i_Pitch)
        {
            return Time.fixedTime - ReleaseTime[IndexOf(i_Pitch)];
        }

        public float GetHitVelocity(Pitch i_Pitch)
        {
            return HitVelocity[IndexOf(i_Pitch)] / 127f;
        }

        protected int IndexOf(Pitch i_Pitch)
        {
            return i_Pitch.ToMidi() - KeyboardConfig.FirstKey.ToMidi();
        }

        [UsedImplicitly]
        private void Update()
        {
            for (var i = 0; i < KeyboardConfig.NumKeys; ++i)
            {
                UpdateKey(i);
            }
        }

        #endregion
    }
}