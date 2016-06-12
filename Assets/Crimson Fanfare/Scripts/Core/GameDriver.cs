using FXG.CrimFan.Config;
using JetBrains.Annotations;
using UnityEngine;

// ReSharper disable once UseNullPropagation

namespace FXG.CrimFan.Core
{
    public sealed class GameDriver : MonoBehaviour
    {
        #region Private fields

        private GameConfig m_PrevConfig;

        #endregion

        #region Public fields

        /// <summary>
        ///     Exposed in Unity inspector
        /// </summary>
        public GameConfig Configuration;

        private Match m_Match;

        private bool m_MustCreateNewMatch;

        #endregion

        #region Methods

        [UsedImplicitly]
        private void Start()
        {
            m_MustCreateNewMatch = true;
        }

        [UsedImplicitly]
        private void Update()
        {
            if (m_MustCreateNewMatch)
            {
                // Start new match
                m_Match = Match.CreateObject(Configuration, this);
                m_PrevConfig = new GameConfig(Configuration);
                m_MustCreateNewMatch = false;
            }

            // Do nothing if configuration has not changed
            if (Configuration == m_PrevConfig && !m_Match.IsOver)
            {
                return;
            }

            // Destroy current match
            Destroy(transform.Find(Match.GAME_OBJ_NAME).gameObject);
            m_MustCreateNewMatch = true;
        }

        #endregion
    }
}