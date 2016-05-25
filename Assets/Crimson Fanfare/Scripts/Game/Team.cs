using FXGuild.CrimFan.Common;
using UnityEngine;

namespace FXGuild.CrimFan.Game
{
    public sealed class Team
    {
        #region Compile-time constants

        private const int MIN_INIT_TERRITORY_SIZE = 5;

        #endregion

        #region Constructors

        public Team(HorizontalDir i_Side, Color i_Color)
        {
            Army = new Army(i_Side);
            Side = i_Side;
            TerritorySize = MIN_INIT_TERRITORY_SIZE;
            Color = i_Color;
        }

        #endregion

        #region Properties

        public Army Army { get; private set; }

        public HorizontalDir Side { get; private set; }

        public int TerritorySize { get; private set; }

        public Color Color { get; private set; }

        #endregion
    }
}