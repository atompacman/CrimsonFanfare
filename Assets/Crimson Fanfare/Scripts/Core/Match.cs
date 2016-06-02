using FXG.CrimFan.Audio;
using FXG.CrimFan.Common;
using FXG.CrimFan.Config;
using FXG.CrimFan.World;
using JetBrains.Annotations;
using UnityEngine;

namespace FXG.CrimFan.Core
{
    public sealed class Match : MonoBehaviour
    {
        #region Nested types

        public enum Ownership
        {
            TEAM_LEFT,
            TEAM_RIGHT,
            NEUTRAL
        }

        #endregion

        #region Private fields

        private Keyboard m_Keyboard;

        #endregion

        #region Properties

        public Team TeamLeft { get; private set; }

        public Team TeamRight { get; private set; }

        #endregion

        #region Static methods

        public static Match CreateComponent(GameConfig i_Config, GameObject i_Parent)
        {
            var match = i_Parent.AddComponent<Match>();
            match.m_Keyboard = Keyboard.CreateObject(i_Config.KeyboardConfig, match);
            match.TeamLeft = Team.CreateObject(HorizontalDir.LEFT, Color.green, match);
            match.TeamRight = Team.CreateObject(HorizontalDir.RIGHT, Color.blue, match);
            return match;
        }

        #endregion

        #region Methods

        public Ownership GetKeyOwnership(Pitch i_Pitch)
        {
            var idx = i_Pitch.ToMidi() - m_Keyboard.Configuration.FirstKey.ToMidi();
            return idx < TeamLeft.TerritorySize
                ? Ownership.TEAM_LEFT
                : idx >= m_Keyboard.Configuration.NumKeys - TeamRight.TerritorySize
                    ? Ownership.TEAM_RIGHT
                    : Ownership.NEUTRAL;
        }

        public Team GetTeam(HorizontalDir i_Side)
        {
            return i_Side == HorizontalDir.LEFT ? TeamLeft : TeamRight;
        }

        [CanBeNull]
        public Team GetTeam(Pitch i_Key)
        {
            var ownership = GetKeyOwnership(i_Key);
            return ownership == Ownership.NEUTRAL
                ? null
                : ownership == Ownership.TEAM_LEFT
                    ? TeamLeft
                    : TeamRight;
        }

        public Team GetEnemyTeamOf(Team i_Team)
        {
            return i_Team.Side == HorizontalDir.LEFT ? TeamRight : TeamLeft;
        }

        #endregion
    }
}