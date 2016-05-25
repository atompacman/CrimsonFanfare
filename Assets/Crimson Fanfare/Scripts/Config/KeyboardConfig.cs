using System;
using FXGuild.CrimFan.Audio;
using FXGuild.CrimFan.Common;
using UnityEngine;

namespace FXGuild.CrimFan.Config
{
    [Serializable]
    public sealed class KeyboardConfig
    {
        #region Public fields

        public Vector3 BlackKeyScale;

        public string DeviceName;

        public Pitch FirstKey;

        [Range(0f, 0.1f)]
        public float GapWidth;

        [Range(36, 88)]
        public int NumKeys;

        public Vector3 WhiteKeyScale;

        #endregion

        #region Constructors

        public KeyboardConfig()
        {
        }

        public KeyboardConfig(KeyboardConfig i_ToCopy)
        {
            BlackKeyScale = i_ToCopy.BlackKeyScale;
            DeviceName = i_ToCopy.DeviceName;
            FirstKey = i_ToCopy.FirstKey;
            GapWidth = i_ToCopy.GapWidth;
            NumKeys = i_ToCopy.NumKeys;
            WhiteKeyScale = i_ToCopy.WhiteKeyScale;
        }

        #endregion

        #region Static methods

        public static bool operator ==(KeyboardConfig i_A, KeyboardConfig i_B)
        {
            return Utils.OperatorEqualHelper(i_A, i_B);
        }

        public static bool operator !=(KeyboardConfig i_A, KeyboardConfig i_B)
        {
            return Utils.OperatorNotEqualHelper(i_A, i_B);
        }

        #endregion

        #region Methods

        public override int GetHashCode()
        {
            // @TODO
            throw new NotImplementedException();
        }

        public override bool Equals(object i_Other)
        {
            if (!(i_Other is KeyboardConfig))
            {
                return false;
            }
            var o = (KeyboardConfig) i_Other;
            return BlackKeyScale == o.BlackKeyScale
                   && DeviceName == o.DeviceName
                   && FirstKey == o.FirstKey
                   && Mathf.Approximately(GapWidth, o.GapWidth)
                   && NumKeys == o.NumKeys
                   && WhiteKeyScale == o.WhiteKeyScale;
        }

        #endregion
    }
}