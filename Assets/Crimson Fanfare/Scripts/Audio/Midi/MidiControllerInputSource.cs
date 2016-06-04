using FXG.CrimFan.World;
using JetBrains.Annotations;
using Sanford.Multimedia.Midi;
using UnityEngine;

namespace FXG.CrimFan.Audio.Midi
{
    public sealed class MidiControllerInputSource : MidiInputSource
    {
        #region Private fields

        private InputDevice m_Device;
        private bool[] m_KeyPressed;

        #endregion

        #region Static methods

        public static MidiControllerInputSource CreateComponent(Keyboard i_Keyboard,
            InputDevice i_Device)
        {
            var numKeys = i_Keyboard.Configuration.NumKeys;
            var src = i_Keyboard.gameObject.AddComponent<MidiControllerInputSource>();
            src.m_Device = i_Device;
            src.m_KeyPressed = new bool[numKeys];

            // Keys start in idle state
            for (var i = 0; i < numKeys; ++i)
            {
                src.m_KeyPressed[i] = false;
            }

            // Subscribe MIDI event handler
            src.m_Device.ChannelMessageReceived += src.OnChannelMessageReceived;
            src.m_Device.StartRecording();

            return src;
        }

        #endregion

        #region Methods

        protected override void UpdateKey(int i_Idx)
        {
            if (m_KeyPressed[i_Idx])
            {
                if (KeyStates[i_Idx] == KeyState.HIT ||
                    KeyStates[i_Idx] == KeyState.HELD)
                {
                    KeyStates[i_Idx] = KeyState.HELD;
                }
                else
                {
                    KeyStates[i_Idx] = KeyState.HIT;
                    HitTime[i_Idx] = Time.fixedTime;
                }
            }
            else
            {
                if (KeyStates[i_Idx] == KeyState.RELEASED ||
                    KeyStates[i_Idx] == KeyState.IDLE)
                {
                    KeyStates[i_Idx] = KeyState.IDLE;
                }
                else
                {
                    KeyStates[i_Idx] = KeyState.RELEASED;
                    ReleaseTime[i_Idx] = Time.fixedTime;
                }
            }
        }

        [UsedImplicitly]
        private void OnDestroy()
        {
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

            // Update binary pressed state and velocity
            var idx = msg.Data1 - KeyboardConfig.FirstKey.ToMidi();
            m_KeyPressed[idx] = msg.Command == ChannelCommand.NoteOn;
            HitVelocity[idx] = (byte) msg.Data2;
        }

        #endregion
    }
}