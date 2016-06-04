using FXG.CrimFan.Common;
using FXG.CrimFan.UI;
using UnityEngine;

// ReSharper disable ConvertPropertyToExpressionBody

// ReSharper disable once ConvertPropertyToExpressionBody

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

        public Match.Ownership AssociatedOwnership
        {
            get
            {
                return Side == HorizontalDir.LEFT
                    ? Match.Ownership.TEAM_LEFT
                    : Match.Ownership.TEAM_RIGHT;
            }
        }

        public Team EnemyTeam
        {
            get { return Side == HorizontalDir.LEFT ? Match.TeamRight : Match.TeamLeft; }
        }

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