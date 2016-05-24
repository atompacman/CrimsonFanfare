using FXGuild.CrimFan.Audio;
using FXGuild.CrimFan.Common;
using FXGuild.CrimFan.Config;
using FXGuild.CrimFan.Game.World;
using UnityEngine;

namespace FXGuild.CrimFan.Game
{
    public sealed class Match
    {
        public enum KeyOwnership
        {
            TEAM_LEFT,
            TEAM_RIGHT,
            NEUTRAL
        }

        private GameConfig m_Config;
        private Keyboard m_Keyboard;

        public Team TeamLeft { get; private set; }
        public Team TeamRight { get; private set; }

        public Match(GameConfig i_Config)
        {
            m_Config = i_Config;
            m_Keyboard = Keyboard.Create(i_Config.KeyboardConfig, this);
            TeamLeft = new Team(HorizontalDir.LEFT, Color.green);
            TeamRight = new Team(HorizontalDir.RIGHT, Color.blue);
        }

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
            else if (idx >= m_Keyboard.Config.NumKeys - TeamRight.TerritorySize)
            {
                return KeyOwnership.TEAM_RIGHT;
            }
            else
            {
                return KeyOwnership.NEUTRAL;
            }
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
                ? TeamLeft : 
                TeamRight;
        }
    }
}