using FXGuild.CrimFan.Common;
using FXGuild.CrimFan.Config;
using FXGuild.CrimFan.Game.World;
using UnityEngine;

namespace FXGuild.CrimFan.Game
{
    public sealed class Match
    {
        private GameConfig m_Config;
        private Keyboard m_Keyboard;

        public Team TeamLeft { get; private set; }
        public Team TeamRight { get; private set; }

        public Match(GameConfig i_Config)
        {
            m_Config = i_Config;
            m_Keyboard = Keyboard.Create(i_Config.KeyboardConfig);
            TeamLeft = new Team(HorizontalDir.LEFT, i_Config.KeyboardConfig.NumKeys);
            TeamRight = new Team(HorizontalDir.RIGHT, i_Config.KeyboardConfig.NumKeys);
        }

        public void Stop()
        {
            Object.Destroy(m_Keyboard.gameObject);
        }
    }
}