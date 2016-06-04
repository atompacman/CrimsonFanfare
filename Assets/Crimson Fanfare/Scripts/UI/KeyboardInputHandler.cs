using FXG.CrimFan.Common;
using FXG.CrimFan.Core;
using FXG.CrimFan.UI.State;
using FXG.CrimFan.World;
using JetBrains.Annotations;
using System;
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
            pim.SetState(new YoloState(i_Keyboard.Match.TeamLeft));
            pim.SetState(new YoloState(i_Keyboard.Match.TeamRight));
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
                });
        }

        private void ExecuteForAllKeys(Action<MenuState, Key, bool> i_Action)
        {
            ExecuteForAllTeamKeys(HorizontalDir.LEFT, i_Action);
            ExecuteForAllTeamKeys(HorizontalDir.RIGHT, i_Action);
        }

        private void ExecuteForAllTeamKeys(HorizontalDir i_Side,
            Action<MenuState, Key, bool> i_Action)
        {
            var numKeys = m_Match.Keyboard.Configuration.NumKeys;
            var leftEnd = Mathf.Max(numKeys / 2, m_Match.TeamLeft.TerritorySize);
            leftEnd = Mathf.Min(leftEnd, numKeys - m_Match.TeamRight.TerritorySize);
            if (i_Side == HorizontalDir.LEFT)
            {
                ExecuteForAllTeamKeys(0, leftEnd, m_LeftTeamState, i_Action);
            }
            else
            {
                ExecuteForAllTeamKeys(leftEnd, numKeys, m_RightTeamState, i_Action);
            }
        }

        private void ExecuteForAllTeamKeys(int i_KeyBeg, int i_KeyEnd, MenuState i_State,
            Action<MenuState, Key, bool> i_Action)
        {
            var kb = m_Match.Keyboard;

            for (var i = i_KeyBeg; i < i_KeyEnd; ++i)
            {
                var pitch = kb.GetPitch(i);
                var key = kb.GetKey(pitch);
                var ownership = m_Match.GetKeyOwnership(pitch);
                var isOnTerritory = ownership != Match.Ownership.NEUTRAL;

                Debug.Assert(ownership != i_State.Team.EnemyTeam.AssociatedOwnership);

                i_Action(i_State, key, isOnTerritory);
            }
        }

        [UsedImplicitly]
        private void Update()
        {
            ExecuteForAllKeys((i_State, i_Key, i_IsOnTerritory) =>
            {
                var midiSrc = m_Match.Keyboard.MidiSource;
                if (midiSrc.IsKeyHit(i_Key.Pitch))
                {
                    i_State.OnKeyboardKeyHit(i_Key, i_IsOnTerritory);
                }
                if (midiSrc.IsKeyPressed(i_Key.Pitch))
                {
                    i_State.OnKeyboardKeyPressed(i_Key, i_IsOnTerritory);
                }
                if (midiSrc.IsKeyReleased(i_Key.Pitch))
                {
                    i_State.OnKeyboardKeyReleased(i_Key, i_IsOnTerritory);
                }
            });
        }

        #endregion
    }
}