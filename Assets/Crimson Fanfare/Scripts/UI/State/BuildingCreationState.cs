using FXG.CrimFan.World;
using UnityEngine;

namespace FXG.CrimFan.UI.State
{
    public sealed class BuildingCreationState : MenuState
    {
        #region Private fields

        private readonly Key m_BuildLocation;

        #endregion

        #region Constructors

        public BuildingCreationState(Key i_BuildLocation)
        {
            m_BuildLocation = i_BuildLocation;
        }

        #endregion

        #region Methods

        public override void InitializeKey(Key i_Key, bool i_IsOnTerritory)
        {
            i_Key.Color = i_Key == m_BuildLocation ? Color.yellow : i_Key.DefaultColor;
        }

        #endregion
    }
}