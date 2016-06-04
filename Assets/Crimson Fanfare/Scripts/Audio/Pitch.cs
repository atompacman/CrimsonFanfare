using System;
using FXG.CrimFan.Common;

// ReSharper disable ConvertPropertyToExpressionBody

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace FXG.CrimFan.Audio
{
    public enum Tone
    {
        C,
        C_SHARP,
        D,
        E_FLAT,
        E,
        F,
        F_SHARP,
        G,
        A_FLAT,
        A,
        B_FLAT,
        B
    }

    public static class Tones
    {
        #region Static methods

        public static bool IsOnWhiteKeys(Tone i_Tone)
        {
            return i_Tone.ToString().Length == 1;
        }

        public static Tone NextTone(Tone i_Tone)
        {
            return (Tone) (((int) i_Tone + 1) % ((int) Tone.B + 1));
        }

        public static Tone PreviousTone(Tone i_Tone)
        {
            return (Tone) (((int) i_Tone + (int) Tone.B) % ((int) Tone.B + 1));
        }

        #endregion
    }

    public static class Pitches
    {
        #region Static methods

        public static Pitch NextPitch(Pitch i_Pitch)
        {
            var tone = Tones.NextTone(i_Pitch.Tone);
            return new Pitch(tone, tone == Tone.C ? i_Pitch.Octave + 1 : i_Pitch.Octave);
        }

        public static Pitch PreviousPitch(Pitch i_Pitch)
        {
            var tone = Tones.PreviousTone(i_Pitch.Tone);
            return new Pitch(tone, tone == Tone.B ? i_Pitch.Octave - 1 : i_Pitch.Octave);
        }

        #endregion
    }

    [Serializable]
    public sealed class Pitch
    {
        #region Public fields

        public int Octave;
        public Tone Tone;

        #endregion

        #region Constructors

        public Pitch(Tone i_Tone, int i_Octave)
        {
            Tone = i_Tone;
            Octave = i_Octave;
        }

        #endregion

        #region Properties

        public bool IsOnWhiteKeys
        {
            get { return Tones.IsOnWhiteKeys(Tone); }
        }

        #endregion

        #region Static methods

        public static bool operator ==(Pitch i_A, Pitch i_B)
        {
            return Utils.OperatorEqualHelper(i_A, i_B);
        }

        public static bool operator !=(Pitch i_A, Pitch i_B)
        {
            return Utils.OperatorNotEqualHelper(i_A, i_B);
        }

        #endregion

        #region Methods

        public int ToMidi()
        {
            return (Octave + 3) * 12 + (int) Tone;
        }

        public override int GetHashCode()
        {
            return ToMidi();
        }

        public override bool Equals(object i_Other)
        {
            if (!(i_Other is Pitch))
            {
                return false;
            }
            var o = (Pitch) i_Other;
            return Octave == o.Octave && Tone == o.Tone;
        }

        public override string ToString()
        {
            return Tone + " " + Octave;
        }

        #endregion
    }
}