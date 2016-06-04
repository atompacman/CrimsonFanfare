using UnityEditor;
using UnityEngine;

namespace FXG.CrimFan.Common
{
    public enum HorizontalDir
    {
        LEFT,
        RIGHT
    }

    public static class Utils
    {
        #region Static methods

        public static GameObject LoadPrefab(string i_Name)
        {
            return Resources.Load<GameObject>("Prefabs/" + i_Name);
        }

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

        public static HorizontalDir OppositeDir(HorizontalDir i_Dir)
        {
            return (HorizontalDir) (((int) i_Dir + 1) % 2);
        }

        public static Color GetInverse(this Color i_Color)
        {
            return new Color(1 - i_Color.r, 1 - i_Color.g, 1 - i_Color.b);
        }

        #endregion
    }
}