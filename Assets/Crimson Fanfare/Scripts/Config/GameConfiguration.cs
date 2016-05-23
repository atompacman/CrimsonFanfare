using FXGuild.CrimFan.Common;
using System;

namespace FXGuild.CrimFan.Config
{
    [Serializable]
    public sealed class GameConfig
    {
        public KeyboardConfig KeyboardConfig;

        
        public GameConfig()
        {
        }

        public GameConfig(GameConfig i_ToCopy)
        {
            KeyboardConfig = new KeyboardConfig(i_ToCopy.KeyboardConfig);
        }

        public static bool operator ==(GameConfig i_A, GameConfig i_B)
        {
            return Utils.OperatorEqualHelper(i_A, i_B);
        }

        public static bool operator !=(GameConfig i_A, GameConfig i_B)
        {
            return Utils.OperatorNotEqualHelper(i_A, i_B);
        }
        
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
    }
}