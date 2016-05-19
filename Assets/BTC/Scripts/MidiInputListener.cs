using Sanford.Multimedia.Midi;
using UnityEngine;

namespace BTC
{
    public sealed class MidiInputListener
    {
        #region Private fields

        private readonly InputDevice m_Device;

        private readonly int m_FirstKey;

        private readonly bool[] m_NotePressed;
        private readonly byte[] m_NoteVelocity;

        #endregion

        #region Constructors

        public MidiInputListener(string i_DeviceName, int i_NumKeys, Pitch i_FirstKey)
        {
            // Look for midi controller input device
            Debug.Log("[BTC] Looking for input MIDI controller");
            for (var i = 0; i < InputDevice.DeviceCount; ++i)
            {
                var cap = InputDevice.GetDeviceCapabilities(i);

                if (cap.name == i_DeviceName)
                {
                    m_Device = new InputDevice(i);
                }

                Debug.LogFormat("{0} - Version: {1} - Support: {2} - Mid: {3} - PID: {4}",
                    cap.name, cap.driverVersion, cap.support, cap.mid, cap.pid);
            }

            if (m_Device == null)
            {
                // Log warning if device was not found
                Debug.LogWarning("Could not find MIDI controller \"" + i_DeviceName + "\"");
            }
            else
            {
                // Subscribe MIDI event handlers
                m_Device.StartRecording();
                m_Device.ChannelMessageReceived += OnChannelMessageReceived;
            }

            m_NotePressed = new bool[i_NumKeys];
            m_NoteVelocity = new byte[i_NumKeys];
            m_FirstKey = ConvertPitch2Midi(i_FirstKey);
        }

        #endregion

        #region Destructors

        ~MidiInputListener()
        {
            Dispose();
        }

        #endregion

        #region Static methods

        private static int ConvertPitch2Midi(Pitch i_Pitch)
        {
            return (i_Pitch.Octave + 3) * 12 + (int) i_Pitch.Tone;
        }

        #endregion

        #region Methods

        public bool IsNotePressed(Pitch i_Pitch)
        {
            return m_NotePressed[ConvertPitch2Midi(i_Pitch) - m_FirstKey];
        }

        public float GetNoteVelocity(Pitch i_Pitch)
        {
            return m_NoteVelocity[ConvertPitch2Midi(i_Pitch) - m_FirstKey] / 127f;
        }

        public void Dispose()
        {
            if (m_Device == null || m_Device.IsDisposed)
            {
                return;
            }
            m_Device.StopRecording();
            m_Device.Dispose();
        }

        private void OnChannelMessageReceived(object i_O, ChannelMessageEventArgs i_Args)
        {
            var msg = i_Args.Message;

            // Only react to NoteOn and NoteOff events
            if (msg.Command != ChannelCommand.NoteOn && msg.Command != ChannelCommand.NoteOff)
            {
                return;
            }

            //Save note pressed state and velocity
            var idx = msg.Data1 - m_FirstKey;
            m_NoteVelocity[idx] = (byte) msg.Data2;
            m_NotePressed[idx] = msg.Command == ChannelCommand.NoteOn;
        }

        #endregion
    }
}