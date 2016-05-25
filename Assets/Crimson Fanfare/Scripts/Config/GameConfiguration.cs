using System;
using FXGuild.CrimFan.Common;

namespace FXGuild.CrimFan.Config
{
    [Serializable]
    public sealed class GameConfig
    {
        #region Public fields

        public KeyboardConfig KeyboardConfig;

        #endregion

        #region Constructors

        public GameConfig()
        {
        }

        public GameConfig(GameConfig i_ToCopy)
        {
            KeyboardConfig = new KeyboardConfig(i_ToCopy.KeyboardConfig);
        }

        #endregion

        #region Static methods

        public static bool operator ==(GameConfig i_A, GameConfig i_B)
        {
            return Utils.OperatorEqualHelper(i_A, i_B);
        }

        public static bool operator !=(GameConfig i_A, GameConfig i_B)
        {
            return Utils.OperatorNotEqualHelper(i_A, i_B);
        }

        #endregion

        #region Methods

        public override bool Equals(object i_Obj)
        {
            if (!(i_Obj is GameConfig))
            {
                return false;
            }

            var o = (GameConfig) i_Obj;
            return KeyboardConfig == o.KeyboardConfig;
        }

        public override int GetHashCode()
        {
            // @TODO
            throw new NotImplementedException();
        }

        #endregion
    }
}