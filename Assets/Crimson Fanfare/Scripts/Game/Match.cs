using FXGuild.CrimFan.Audio;
using FXGuild.CrimFan.Common;
using FXGuild.CrimFan.Config;
using FXGuild.CrimFan.Game.World;
using UnityEngine;
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local

namespace FXGuild.CrimFan.Game
{
    public sealed class Match
    {
        #region Nested types

        public enum KeyOwnership
        {
            TEAM_LEFT,
            TEAM_RIGHT,
            NEUTRAL
        }

        #endregion

        #region Private fields

        private GameConfig m_Config;

        private readonly Keyboard m_Keyboard;

        #endregion

        #region Constructors

        public Match(GameConfig i_Config)
        {
            m_Config = i_Config;
            m_Keyboard = Keyboard.Create(i_Config.KeyboardConfig, this);
            TeamLeft = new Team(HorizontalDir.LEFT, Color.green);
            TeamRight = new Team(HorizontalDir.RIGHT, Color.blue);
        }

        #endregion

        #region Properties

        public Team TeamLeft { get; private set; }

        public Team TeamRight { get; private set; }

        #endregion

        #region Methods

        public void Stop()
        {
            Object.Destroy(m_Keyboard.gameObject);
        }

        public KeyOwnership GetKeyOwnership(Pitch i_Pitch)
        {
            var idx = i_Pitch.ToMidi() - m_Keyboard.Config.FirstKey.ToMidi();
            if (idx < TeamLeft.TerritorySize)
            {
                return KeyOwnership.TEAM_LEFT;
            }
            if (idx >= m_Keyboard.Config.NumKeys - TeamRight.TerritorySize)
            {
                return KeyOwnership.TEAM_RIGHT;
            }
            return KeyOwnership.NEUTRAL;
        }

        public Team GetTeam(HorizontalDir i_Side)
        {
            return i_Side == HorizontalDir.LEFT ? TeamLeft : TeamRight;
        }

        public Team GetTeam(Pitch i_Key)
        {
            var ownership = GetKeyOwnership(i_Key);
            return ownership == KeyOwnership.NEUTRAL
                ? null
                : ownership == KeyOwnership.TEAM_LEFT
                    ? TeamLeft
                    : TeamRight;
        }

        #endregion
    }
}