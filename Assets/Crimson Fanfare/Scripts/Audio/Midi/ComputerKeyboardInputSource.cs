using System;
using UnityEngine;

namespace FXGuild.CrimFan.Audio.Midi
{
    public sealed class ComputerKeyboardInputSource : MidiInputSource
    {
        #region Compile-time constants

        private const float VELOCITY = 1f;

        #endregion

        #region Private fields

        private int m_NumKeys;

        #endregion

        #region Static methods

        public static ComputerKeyboardInputSource Create(int i_NumKeys)
        {
            var obj = new GameObject();
            var src = obj.AddComponent<ComputerKeyboardInputSource>();
            src.m_NumKeys = i_NumKeys;
            return src;
        }

        #endregion

        #region Methods

        public override bool IsKeyHit(Pitch i_Pitch)
        {
            return CheckKeyState(i_Pitch, Input.GetKeyDown);
        }

        public override bool IsKeyReleased(Pitch i_Pitch)
        {
            return CheckKeyState(i_Pitch, Input.GetKeyUp);
        }

        public override bool IsKeyPressed(Pitch i_Pitch)
        {
            return CheckKeyState(i_Pitch, Input.GetKey);
        }

        public override float GetHitVelocity(Pitch i_Pitch)
        {
            return VELOCITY;
        }

        private bool CheckKeyState(Pitch i_Pitch, Func<string, bool> i_InputMethod)
        {
            var relativeKey = i_Pitch.ToMidi() - FirstKey.ToMidi();

            // Piano keyboard cannot be mapped on the computer keyboard
            if (relativeKey < Mathf.Min(m_NumKeys, 13))
            {
                return i_InputMethod(((char) (relativeKey + 'a')).ToString());
            }
            if (relativeKey >= Mathf.Max(0, m_NumKeys - 13))
            {
                return i_InputMethod(((char) (relativeKey - (m_NumKeys - 26) + 'a')).ToString());
            }

            return false;
        }

        #endregion
    }
}