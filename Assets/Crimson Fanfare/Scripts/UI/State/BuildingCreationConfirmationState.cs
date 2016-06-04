using FXG.CrimFan.World;
using FXG.CrimFan.World.Buildings;
using UnityEngine;

namespace FXG.CrimFan.UI.State
{
    public sealed class BuildingCreationConfirmationState : MenuState
    {
        #region Compile-time constants

        private const float BUILD_LOC_FLASH_SPEED = 6f;

        #endregion

        #region Private fields

        private readonly float m_CreationTime;
        private readonly Key m_BuildLocation;

        #endregion

        #region Constructors

        public BuildingCreationConfirmationState(Key i_BuildLocation)
        {
            m_CreationTime = Time.fixedTime;
            m_BuildLocation = i_BuildLocation;
        }

        #endregion

        #region Methods

        public override void InitializeKey(Key i_Key, bool i_IsOnTerritory)
        {
            i_Key.Color = Color.gray;
        }

        public override bool OnKeyboardKeyReleased(Key i_Key, bool i_IsOnTerritory)
        {
            if (i_Key == m_BuildLocation)
            {
                //ChangeState(new BuildingCreationState(i_Key));
                i_Key.SetBuilding<Barrack>();
                ChangeState(new StandardState());
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
            var factor = 0.5f * Mathf.Cos((Time.fixedTime - m_CreationTime) * BUILD_LOC_FLASH_SPEED) + 0.5f;
            m_BuildLocation.Color = Color.Lerp(Color.white * 0.2f, i_Color, factor);
        }

        #endregion
    }
}