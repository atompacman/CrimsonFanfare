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

        #endregion

        #region Methods

        [UsedImplicitly]
        private void Update()
        {
            // Do nothing if configuration has not changed
            if (Configuration == m_PrevConfig)
            {
                return;
            }

            // Start new match
            Match.CreateComponent(Configuration, this);
            m_PrevConfig = new GameConfig(Configuration);
        }

        #endregion
    }
}