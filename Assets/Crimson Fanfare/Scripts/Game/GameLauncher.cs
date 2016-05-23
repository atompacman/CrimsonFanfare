using FXGuild.CrimFan.Config;
using JetBrains.Annotations;
using UnityEngine;

namespace FXGuild.CrimFan.Game
{
    public sealed class GameLauncher : MonoBehaviour
    {
        public GameConfig Configuration;
        private GameConfig m_PrevConfig;
        private Match m_Match;

        [UsedImplicitly]
        private void Update()
        {
            if (Configuration == m_PrevConfig)
            {
                return;
            }

            if (m_Match != null)
            {
                m_Match.Stop();
            }

            m_Match = new Match(Configuration);
            m_PrevConfig = new GameConfig(Configuration);
        }
    }
}