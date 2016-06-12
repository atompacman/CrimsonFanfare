using FXG.CrimFan.Common;
using FXG.CrimFan.Core;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

// ReSharper disable SpecifyACultureInStringConversionExplicitly

namespace FXG.CrimFan.UI
{
    public sealed class TeamHpGuiElement : MonoBehaviour
    {
        #region Compile-time constants

        private const string LEFT_PREFAB_NAME = "LeftTeamHP";
        private const string RIGHT_PREFAB_NAME = "RightTeamHP";

        #endregion

        #region Private fields

        private Team m_Team;

        #endregion

        #region Static methods

        public static void InitializeObject(Team i_Team, StaticGui i_Gui)
        {
            var name = i_Team.Side == HorizontalDir.LEFT ? LEFT_PREFAB_NAME : RIGHT_PREFAB_NAME;
            var elem = i_Gui.transform.FindChild(name).gameObject.GetComponent<TeamHpGuiElement>();
            elem.m_Team = i_Team;
        }

        #endregion

        #region Methods

        [UsedImplicitly]
        private void Update()
        {
            GetComponentInChildren<Text>().text = (int) m_Team.Hp + "/" + m_Team.MaxHp;
        }

        #endregion
    }
}