﻿using Sanford.Multimedia.Midi;
using UnityEngine;

namespace FXGuild.CrimFan.Audio.Midi
{
    public sealed class MidiControllerInputSource : MidiInputSource
    {
        private enum ButtonState
        {
            IDLE,
            HIT,
            HELD,
            RELEASED
        }

        private InputDevice m_Device;
        private ButtonState[] m_KeyStates;
        private bool[] m_KeyPressed;
        private byte[] m_HitVelocity;
        
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

        private bool CheckKeyState(Pitch i_Pitch, ButtonState i_State)
        {
            return m_KeyStates[ConvertPitch2Midi(i_Pitch) - ConvertPitch2Midi(FirstKey)] == i_State;
        }

        public override float GetHitVelocity(Pitch i_Pitch)
        {
            return m_HitVelocity[ConvertPitch2Midi(i_Pitch) - ConvertPitch2Midi(FirstKey)] / 127f;
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

        private void OnChannelMessageReceived(object i_O, ChannelMessageEventArgs i_Args)
        {
            var msg = i_Args.Message;

            // Only react to NoteOn and NoteOff events
            if (msg.Command != ChannelCommand.NoteOn && msg.Command != ChannelCommand.NoteOff)
            {
                return;
            }

            // Update binary pressed state and velocity
            var idx = msg.Data1 - ConvertPitch2Midi(FirstKey);
            m_KeyPressed[idx] = msg.Command == ChannelCommand.NoteOn;
            m_HitVelocity[idx] = (byte) msg.Data2;
        }
        
        private void Update()
        {
            for (var i = 0; i < m_KeyPressed.Length; ++i)
            {
                if (m_KeyPressed[i])
                {
                    m_KeyStates[i] = m_KeyStates[i] == ButtonState.HIT || m_KeyStates[i] == ButtonState.HELD
                        ? ButtonState.HELD
                        : ButtonState.HIT;
                }
                else
                {
                    m_KeyStates[i] = m_KeyStates[i] == ButtonState.RELEASED || m_KeyStates[i] == ButtonState.IDLE
                        ? ButtonState.IDLE
                        : ButtonState.RELEASED;
                }
            }
        }

        // @TODO clean me
        ~MidiControllerInputSource()
        {
            Dispose();
        }
    }
}
