using Sanford.Multimedia.Midi;
using UnityEngine;

namespace FXGuild.CrimFan.Audio.Midi
{
    public abstract class MidiInputSource : MonoBehaviour
    {
        #region Private fields

        protected Pitch FirstKey { get; private set; }

        #endregion

        #region Constructors

        public static MidiInputSource Create(string i_DeviceName, int i_NumKeys, Pitch i_FirstKey)
        {
            // Look for a input device with the specified name
            InputDevice device = null;
            Debug.Log("[BTC] Looking for input MIDI controller");
            for (var i = 0; i < InputDevice.DeviceCount; ++i)
            {
                var cap = InputDevice.GetDeviceCapabilities(i);
                if (cap.name == i_DeviceName)
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
                Debug.Log("Could not find MIDI controller \"" + i_DeviceName + "\". Using computer keyboard instead.");
                src = ComputerKeyboardInputSource.Create(i_NumKeys);
            }
            else
            {
                src = MidiControllerInputSource.Create(device, i_NumKeys);
            }

            // Initialize common MidiInputSource members
            src.FirstKey = i_FirstKey;

            return src;
        }
        
        #endregion

        #region Static methods

        protected static int ConvertPitch2Midi(Pitch i_Pitch)
        {
            return (i_Pitch.Octave + 3) * 12 + (int) i_Pitch.Tone;
        }

        #endregion

        #region Methods

        public abstract bool IsKeyHit(Pitch i_Pitch);

        public abstract bool IsKeyReleased(Pitch i_Pitch);

        public abstract bool IsKeyPressed(Pitch i_Pitch);

        public abstract float GetHitVelocity(Pitch i_Pitch);

        #endregion
    }
}