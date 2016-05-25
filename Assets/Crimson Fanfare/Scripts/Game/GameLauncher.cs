using FXGuild.CrimFan.Config;
using JetBrains.Annotations;
using UnityEngine;

namespace FXGuild.CrimFan.Game
{
    public sealed class GameLauncher : MonoBehaviour
    {
        #region Private fields

        private Match m_Match;
        private GameConfig m_PrevConfig;

        #endregion

        #region Public fields

        public GameConfig Configuration;

        #endregion

        #region Methods

        [UsedImplicitly]
        private void Update()
        {
            if (Configuration == m_PrevConfig)
            {
                return;
            }

            // ReSharper disable once UseNullPropagation
            if (m_Match != null)
            {
                m_Match.Stop();
            }

            m_Match = new Match(Configuration);
            m_PrevConfig = new GameConfig(Configuration);
        }

        #endregion
    }
}