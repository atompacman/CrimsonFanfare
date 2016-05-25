using JetBrains.Annotations;
using Sanford.Multimedia.Midi;
using UnityEngine;

namespace FXGuild.CrimFan.Audio.Midi
{
    public sealed class MidiControllerInputSource : MidiInputSource
    {
        #region Nested types

        private enum ButtonState
        {
            IDLE,
            HIT,
            HELD,
            RELEASED
        }

        #endregion

        #region Private fields

        private InputDevice m_Device;

        private byte[] m_HitVelocity;

        private bool[] m_KeyPressed;

        private ButtonState[] m_KeyStates;

        #endregion

        #region Destructors

        // @TODO clean me
        ~MidiControllerInputSource()
        {
            Dispose();
        }

        #endregion

        #region Static methods

        public static MidiControllerInputSource Create(InputDevice i_Device, int i_NumKeys)
        {
            var src = new GameObject().AddComponent<MidiControllerInputSource>();
            src.m_Device = i_Device;
            src.m_KeyStates = new ButtonState[i_NumKeys];
            src.m_KeyPressed = new bool[i_NumKeys];
            src.m_HitVelocity = new byte[i_NumKeys];

            // Keys start in idle state
            for (var i = 0; i < i_NumKeys; ++i)
            {
                src.m_KeyPressed[i] = false;
                src.m_KeyStates[i] = ButtonState.IDLE;
            }

            // Subscribe MIDI event handler
            src.m_Device.ChannelMessageReceived += src.OnChannelMessageReceived;
            src.m_Device.StartRecording();

            return src;
        }

        #endregion

        #region Methods

        public override bool IsKeyHit(Pitch i_Pitch)
        {
            return CheckKeyState(i_Pitch, ButtonState.HIT);
        }

        public override bool IsKeyReleased(Pitch i_Pitch)
        {
            return CheckKeyState(i_Pitch, ButtonState.RELEASED);
        }

        public override bool IsKeyPressed(Pitch i_Pitch)
        {
            return !CheckKeyState(i_Pitch, ButtonState.IDLE);
        }

        public override float GetHitVelocity(Pitch i_Pitch)
        {
            return m_HitVelocity[i_Pitch.ToMidi() - FirstKey.ToMidi()] / 127f;
        }

        // @TODO clean me
        public void Dispose()
        {
            if (m_Device == null || m_Device.IsDisposed)
            {
                return;
            }
            m_Device.StopRecording();
            m_Device.Dispose();
        }

        private bool CheckKeyState(Pitch i_Pitch, ButtonState i_State)
        {
            return m_KeyStates[i_Pitch.ToMidi() - FirstKey.ToMidi()] == i_State;
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
            var idx = msg.Data1 - FirstKey.ToMidi();
            m_KeyPressed[idx] = msg.Command == ChannelCommand.NoteOn;
            m_HitVelocity[idx] = (byte) msg.Data2;
        }

        [UsedImplicitly]
        private void Update()
        {
            for (var i = 0; i < m_KeyPressed.Length; ++i)
            {
                if (m_KeyPressed[i])
                {
                    m_KeyStates[i] = m_KeyStates[i] == ButtonState.HIT ||
                                     m_KeyStates[i] == ButtonState.HELD
                        ? ButtonState.HELD
                        : ButtonState.HIT;
                }
                else
                {
                    m_KeyStates[i] = m_KeyStates[i] == ButtonState.RELEASED ||
                                     m_KeyStates[i] == ButtonState.IDLE
                        ? ButtonState.IDLE
                        : ButtonState.RELEASED;
                }
            }
        }

        #endregion
    }
}