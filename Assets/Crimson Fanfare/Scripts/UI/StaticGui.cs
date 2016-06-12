using FXG.CrimFan.Common;
using FXG.CrimFan.Core;
using UnityEngine;

// ReSharper disable InconsistentNaming

namespace FXG.CrimFan.UI
{
    public sealed class StaticGui : MonoBehaviour
    {
        #region Compile-time constants

        private const string PREFAB_NAME = "PermanantGui";

        #endregion

        #region Runtime constants

        private static readonly GameObject PREFAB;

        #endregion

        #region Constructors

        static StaticGui()
        {
            PREFAB = Utils.LoadPrefab(PREFAB_NAME);
        }

        #endregion

        #region Static methods

        public static StaticGui CreateObject(Match i_Match)
        {
            var gui = Instantiate(PREFAB).GetComponent<StaticGui>();
            gui.transform.SetParent(i_Match.transform, false);
            TeamHpGuiElement.InitializeObject(i_Match.TeamLeft, gui);
            TeamHpGuiElement.InitializeObject(i_Match.TeamRight, gui);
            return gui;
        }

        #endregion
    }
}