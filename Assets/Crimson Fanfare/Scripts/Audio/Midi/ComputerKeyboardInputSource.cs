using FXG.CrimFan.World;
using UnityEngine;

namespace FXG.CrimFan.Audio.Midi
{
    public sealed class ComputerKeyboardInputSource : MidiInputSource
    {
        #region Static methods

        public new static ComputerKeyboardInputSource CreateComponent(Keyboard i_Keyboard)
        {
            return i_Keyboard.gameObject.AddComponent<ComputerKeyboardInputSource>();
        }

        #endregion

        #region Methods

        protected override void UpdateKey(int i_Idx)
        {
            // Map key to keyboard name
            string keyName = null;
            if (i_Idx < Mathf.Min(KeyboardConfig.NumKeys, 13))
            {
                keyName = ((char) (i_Idx + 'a')).ToString();
            }
            if (i_Idx >= Mathf.Max(0, KeyboardConfig.NumKeys - 13))
            {
                keyName = ((char) (i_Idx - (KeyboardConfig.NumKeys - 26) + 'a')).ToString();
            }
            if (keyName == null)
            {
                return;
            }

            if (Input.GetKeyDown(keyName))
            {
                KeyStates[i_Idx] = KeyState.HIT;
                HitTime[i_Idx] = Time.fixedTime;
            }
            else if (Input.GetKeyUp(keyName))
            {
                KeyStates[i_Idx] = KeyState.RELEASED;
                ReleaseTime[i_Idx] = Time.fixedTime;
            }
            else if (Input.GetKey(keyName))
            {
                KeyStates[i_Idx] = KeyState.HELD;
            }
            else
            {
                KeyStates[i_Idx] = KeyState.IDLE;
            }
        }

        #endregion
    }
}