using System;

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BTC
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
    public sealed class Pitch : ICloneable
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

        #region Static methods

        public static bool operator ==(Pitch i_A, Pitch i_B)
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

        public static bool operator !=(Pitch i_A, Pitch i_B)
        {
            return !(i_A == i_B);
        }

        #endregion

        #region Methods

        public override bool Equals(object i_Obj)
        {
            return i_Obj is Pitch && Equals((Pitch) i_Obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Octave * 397) ^ (int) Tone;
            }
        }

        public object Clone()
        {
            return new Pitch(Tone, Octave);
        }

        private bool Equals(Pitch i_Other)
        {
            return Octave == i_Other.Octave && Tone == i_Other.Tone;
        }

        #endregion
    }
}