using FXG.CrimFan.Common;
using UnityEngine;

namespace FXG.CrimFan.Core
{
    public sealed class Team : MonoBehaviour
    {
        #region Compile-time constants

        private const int MIN_INIT_TERRITORY_SIZE = 5;

        #endregion

        #region Properties

        public Army Army { get; private set; }

        public HorizontalDir Side { get; private set; }

        public int TerritorySize { get; private set; }

        public Color Color { get; private set; }

        public Match Match { get; private set; }

        #endregion

        #region Static methods

        public static Team CreateObject(HorizontalDir i_Side, Color i_Color, Match i_Match)
        {
            var team = new GameObject("Team " + i_Side).AddComponent<Team>();
            team.transform.parent = i_Match.transform;
            team.Side = i_Side;
            team.TerritorySize = MIN_INIT_TERRITORY_SIZE;
            team.Color = i_Color;
            team.Match = i_Match;
            team.Army = Army.CreateComponent(team);
            return team;
        }

        #endregion
    }
}