using FXG.CrimFan.Audio.Midi;
using FXG.CrimFan.Core;
using FXG.CrimFan.World;
using UnityEngine;

// ReSharper disable ConvertPropertyToExpressionBody

namespace FXG.CrimFan.UI.State
{
    public abstract class MenuState
    {
        #region Constructors

        protected MenuState(Team i_Team = null)
        {
            Team = i_Team;
            CreationTime = Time.fixedTime;
        }

        #endregion

        #region Properties

        public Team Team { get; private set; }

        protected float CreationTime { get; private set; }

        protected MidiInputSource MidiSource
        {
            get { return Team.Match.Keyboard.MidiSource; }
        }

        #endregion

        #region Methods

        public virtual void InitializeKey(Key i_Key, bool i_IsOnTerritory)
        {
        }

        public virtual void OnKeyboardKeyIdle(Key i_Key, bool i_IsOnTerritory)
        {
        }

        public virtual void OnKeyboardKeyHit(Key i_Key, bool i_IsOnTerritory)
        {
        }

        public virtual void OnKeyboardKeyHeld(Key i_Key, bool i_IsOnTerritory)
        {
        }

        public virtual bool OnKeyboardKeyReleased(Key i_Key, bool i_IsOnTerritory)
        {
            return false;
        }

        public virtual void UpdateKey(Key i_Key, bool i_IsOnTerritory)
        {
        }

        protected void ChangeState(MenuState i_NewState)
        {
            i_NewState.Team = Team;
            Team.Match.KeyboardInputHandler.SetState(i_NewState);
        }

        #endregion
    }
}