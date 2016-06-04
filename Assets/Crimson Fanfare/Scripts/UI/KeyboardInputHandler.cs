using System;
using FXG.CrimFan.Audio.Midi;
using FXG.CrimFan.Common;
using FXG.CrimFan.Core;
using FXG.CrimFan.UI.State;
using FXG.CrimFan.World;
using JetBrains.Annotations;
using UnityEngine;

namespace FXG.CrimFan.UI
{
    public sealed class KeyboardInputHandler : MonoBehaviour
    {
        #region Private fields

        private Match m_Match;
        private MenuState m_LeftTeamState;
        private MenuState m_RightTeamState;

        #endregion

        #region Static methods

        public static KeyboardInputHandler CreateComponent(Keyboard i_Keyboard)
        {
            var pim = i_Keyboard.gameObject.AddComponent<KeyboardInputHandler>();
            pim.m_Match = i_Keyboard.Match;
            pim.SetState(new StandardState(i_Keyboard.Match.TeamLeft));
            pim.SetState(new StandardState(i_Keyboard.Match.TeamRight));
            return pim;
        }

        #endregion

        #region Methods

        public void SetState(MenuState i_NewState)
        {
            if (i_NewState.Team.Side == HorizontalDir.LEFT)
            {
                m_LeftTeamState = i_NewState;
            }
            else
            {
                m_RightTeamState = i_NewState;
            }

            ExecuteForAllTeamKeys(i_NewState.Team.Side,
                (i_State, i_Key, i_IsOnTerritory) =>
                {
                    i_State.InitializeKey(i_Key, i_IsOnTerritory);
                    return false;
                });
        }

        private void ExecuteForAllKeys(Func<MenuState, Key, bool, bool> i_Func)
        {
            ExecuteForAllTeamKeys(HorizontalDir.LEFT, i_Func);
            ExecuteForAllTeamKeys(HorizontalDir.RIGHT, i_Func);
        }

        private void ExecuteForAllTeamKeys(HorizontalDir i_Side,
            Func<MenuState, Key, bool, bool> i_Func)
        {
            var numKeys = m_Match.Keyboard.Configuration.NumKeys;
            var leftEnd = Mathf.Max(numKeys / 2, m_Match.TeamLeft.TerritorySize);
            leftEnd = Mathf.Min(leftEnd, numKeys - m_Match.TeamRight.TerritorySize);
            if (i_Side == HorizontalDir.LEFT)
            {
                ExecuteForAllTeamKeys(0, leftEnd, m_LeftTeamState, i_Func);
            }
            else
            {
                ExecuteForAllTeamKeys(leftEnd, numKeys, m_RightTeamState, i_Func);
            }
        }

        private void ExecuteForAllTeamKeys(int i_KeyBeg, int i_KeyEnd, MenuState i_State,
            Func<MenuState, Key, bool, bool> i_Func)
        {
            for (var i = i_KeyBeg; i < i_KeyEnd; ++i)
            {
                var pitch = m_Match.Keyboard.GetPitch(i);
                var key = m_Match.Keyboard.GetKey(pitch);
                var ownership = m_Match.GetKeyOwnership(pitch);
                var isOnTerritory = ownership != Match.Ownership.NEUTRAL;
                if (i_Func(i_State, key, isOnTerritory))
                {
                    return;
                }
            }
        }

        [UsedImplicitly]
        private void Update()
        {
            ExecuteForAllKeys((i_State, i_Key, i_IsOnTerritory) =>
            {
                i_State.UpdateKey(i_Key, i_IsOnTerritory);

                switch (m_Match.Keyboard.MidiSource.GetKeyState(i_Key.Pitch))
                {
                case MidiInputSource.KeyState.IDLE:
                    i_State.OnKeyboardKeyIdle(i_Key, i_IsOnTerritory);
                    break;

                case MidiInputSource.KeyState.HIT:
                    i_State.OnKeyboardKeyHit(i_Key, i_IsOnTerritory);
                    break;

                case MidiInputSource.KeyState.HELD:
                    i_State.OnKeyboardKeyHeld(i_Key, i_IsOnTerritory);
                    break;

                case MidiInputSource.KeyState.RELEASED:
                    return i_State.OnKeyboardKeyReleased(i_Key, i_IsOnTerritory);

                default:
                    throw new ArgumentOutOfRangeException();
                }

                return false;
            });
        }

        #endregion
    }
}