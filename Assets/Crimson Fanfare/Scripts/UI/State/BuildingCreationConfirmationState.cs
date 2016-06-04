using FXG.CrimFan.World;
using UnityEngine;

namespace FXG.CrimFan.UI.State
{
    public sealed class BuildingCreationConfirmationState : MenuState
    {
        #region Compile-time constants

        private const float BUILD_LOC_FLASH_SPEED = 6f;

        #endregion

        #region Private fields

        private readonly Key m_BuildLocation;

        #endregion

        #region Constructors

        public BuildingCreationConfirmationState(Key i_BuildLocation)
        {
            m_BuildLocation = i_BuildLocation;
        }

        #endregion

        #region Methods

        public override void InitializeKey(Key i_Key, bool i_IsOnTerritory)
        {
            i_Key.SetColor(Color.gray);
        }

        public override bool OnKeyboardKeyReleased(Key i_Key, bool i_IsOnTerritory)
        {
            if (i_Key == m_BuildLocation)
            {
                ChangeState(new BuildingCreationState(i_Key));
            }
            else
            {
                ChangeState(new StandardState());
            }
            return true;
        }

        public override void UpdateKey(Key i_Key, bool i_IsOnTerritory)
        {
            if (i_Key == m_BuildLocation)
            {
                SetBuildLocationColor(Color.green);
            }
        }

        private void SetBuildLocationColor(Color i_Color)
        {
            var factor = 0.5f * Mathf.Cos(Time.fixedTime * BUILD_LOC_FLASH_SPEED) + 0.5f;
            m_BuildLocation.SetColor(Color.Lerp(i_Color, Color.white * 0.2f, factor));
        }

        #endregion
    }
}