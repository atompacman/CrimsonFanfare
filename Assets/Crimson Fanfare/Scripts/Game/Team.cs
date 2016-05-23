using FXGuild.CrimFan.Common;

namespace FXGuild.CrimFan.Game
{
    public sealed class Team
    {
        #region Compile-time constants

        private const int MIN_INIT_TERRITORY_SIZE = 5;

        #endregion

        #region Constructors

        public Team(HorizontalDir i_Side, int i_NumKeysOnKeyboard)
        {
            Side = i_Side;
            TerritoryEnd = Side == HorizontalDir.LEFT
                ? MIN_INIT_TERRITORY_SIZE
                : i_NumKeysOnKeyboard - MIN_INIT_TERRITORY_SIZE - 1;
        }

        #endregion

        #region Properties

        public HorizontalDir Side { get; private set; }

        public int TerritoryEnd { get; private set; }

        #endregion
    }
}