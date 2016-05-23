namespace FXGuild.CrimFan.Common
{
    public static class Utils
    {
        #region Static methods

        public static bool OperatorEqualHelper<T>(T i_A, T i_B)
        {
            if (ReferenceEquals(i_A, i_B))
            {
                return true;
            }
            if (ReferenceEquals(i_A, null) || ReferenceEquals(i_B, null))
            {
                return false;
            }
            return i_A.Equals(i_B);
        }

        public static bool OperatorNotEqualHelper<T>(T i_A, T i_B)
        {
            if (ReferenceEquals(i_A, i_B))
            {
                return false;
            }
            if (ReferenceEquals(i_A, null) || ReferenceEquals(i_B, null))
            {
                return true;
            }
            return !i_A.Equals(i_B);
        }

        #endregion
    }

    public enum HorizontalDir
    {
        LEFT,
        RIGHT
    }
}